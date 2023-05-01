using System;
using System.Collections.Generic;
using UnityEngine;


namespace Api
{
    internal class GameStateBridge : IGameState
    {
        private int _frame;
        private PlayerBridge[] _players = Array.Empty<PlayerBridge>();
        private int _playersCount;
        private GameObjectBridge[] _platforms = Array.Empty<GameObjectBridge>();
        private int _platformsCount;
        private GameObjectBridge[] _meleeAttacks = Array.Empty<GameObjectBridge>();
        private int _meleeAttacksCount;

        void IGameState.Update(byte[] rawData, int length)
        {
            var ser = Ser.GameState.Parser.ParseFrom(rawData, 0, length);
            _frame = ser.Frame;

            Copy(ser.Players, ref _players, ref _playersCount, (src, dest) => dest.Update(src));
            Copy(ser.Platforms, ref _platforms, ref _platformsCount, (src, dest) => dest.Update(src));
            Copy(ser.MeleeAttacks, ref _meleeAttacks, ref _meleeAttacksCount, (src, dest) => dest.Update(src));
        }

        int IGameState.Frame => _frame;
        ReadOnlySpan<IPlayer> IGameState.Players => _players[.._playersCount];
        ReadOnlySpan<IGameObject> IGameState.Platforms => _platforms[.._platformsCount];
        ReadOnlySpan<IGameObject> IGameState.MeleeAttacks => _meleeAttacks[.._meleeAttacksCount];

        private static void Copy<TSrc, TDest>(
            IReadOnlyList<TSrc> source,
            ref TDest[] dest,
            ref int count,
            Action<TSrc, TDest> update)
            where TDest : new()
        {
            if (source.Count > dest.Length)
                Array.Resize(ref dest, source.Count);

            for (int i = count; i < dest.Length; i++)
                dest[i] = new TDest();

            for (int i = 0; i < source.Count; i++)
                update.Invoke(source[i], dest[i]);

            count = source.Count;
        }


        private class PlayerBridge : IPlayer
        {
            private GameObjectBridge _object = new();
            
            public void Update(Ser.Player player)
            {
                _object.Update(player.Obj);
                State = (Api.PlayerState) (int) player.State;
                StateFrame = player.StateFrame;
                OnGround = player.OnGround;
                OnDamage = player.OnDamage;
                LeftDireciton = player.LeftDirection;
                CurrentHealth = player.CurrentHealth;
                MaxHealth = player.MaxHealth;
            }

            private static InputMap MapInput(int bs)
            {
                var input = new InputMap();
                input.LeftPressed = (bs & (1 << 0)) > 0;
                input.RightPressed = (bs & (1 << 1)) > 0;
                input.UpPressed = (bs & (1 << 2)) > 0;
                input.DownPressed = (bs & (1 << 3)) > 0;
                input.LeftMouseClicked = (bs & (1 << 4)) > 0;
                return input;
            }

            public IGameObject Object => _object;
            public PlayerState State { get; private set; }
            public int StateFrame { get; private set; }
            public bool OnGround { get; private set; }
            public bool OnDamage { get; private set; }
            public bool LeftDireciton { get; private set; }
            public int CurrentHealth { get; private set; }
            public int MaxHealth { get; private set; }
        }


        private class GameObjectBridge : IGameObject
        {
            public void Update(Ser.GameObject source)
            {
                Size = new Vector2(source.Width, source.Height);
                Position = new Vector2(source.Position.X, source.Position.Y);
                Velocity = new Vector2(source.Velocity.X, source.Velocity.Y);
            }

            public Vector2 Size { get; private set; }
            public Vector2 Position { get; private set; }
            public Vector2 Velocity { get; private set; }
        }
    }
}