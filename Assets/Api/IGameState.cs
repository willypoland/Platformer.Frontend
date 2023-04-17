using System;
using System.Collections.Generic;


namespace Api
{
    public interface IGameState
    {
        public void Update(byte[] rawData, int length);
        
        public int Frame { get; }
        public ReadOnlySpan<IPlayer> Players { get; }
        public ReadOnlySpan<IGameObject> Platforms { get; }
        public ReadOnlySpan<IGameObject> MeleeAttacks { get; }
    }
}