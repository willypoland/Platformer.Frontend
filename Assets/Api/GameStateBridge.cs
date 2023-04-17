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
            public void Update(Ser.Player player)
            {
                _object.Update(player.Obj);
                _state = (Api.PlayerState) (int) player.State;
                _stateFrame = player.StateFrame;
                _prevInput = MapInput(player.PrevInput);
                _onGround = player.OnGround;
                _onDamage = player.OnDamage;
                _leftDireciton = player.LeftDirection;
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

            private readonly GameObjectBridge _object = new();
            private PlayerState _state;
            private int _stateFrame;
            private InputMap _prevInput;
            private bool _onGround;
            private bool _onDamage;
            private bool _leftDireciton;

            IGameObject IPlayer.Object => _object;
            PlayerState IPlayer.State => _state;
            int IPlayer.StateFrame => _stateFrame;
            InputMap IPlayer.PrevNput => _prevInput;
            bool IPlayer.OnGround => _onGround;
            bool IPlayer.OnDamage => _onDamage;
            bool IPlayer.LeftDireciton => _leftDireciton;
        }


        private class GameObjectBridge : IGameObject
        {
            public void Update(Ser.GameObject source)
            {
                _size = new Vector2(source.Width, source.Height);
                _position = new Vector2(source.Position.X, source.Position.Y);
                _velocity = new Vector2(source.Velocity.X, source.Velocity.Y);
            }

            private Vector2 _size;
            private Vector2 _position;
            private Vector2 _velocity;

            Vector2 IGameObject.Size => _size;
            Vector2 IGameObject.Position => _position;
            Vector2 IGameObject.Velocity => _velocity;
        }
    }
}