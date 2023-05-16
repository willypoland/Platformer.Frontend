using System;
using UnityEngine;


namespace Api
{
    public readonly unsafe ref struct Location
    {
        private readonly bool _is1stPlayer;
        private readonly Vector2Int _position1stPlayer;
        private readonly Vector2Int _position2ndPlayer;
        private readonly Platform* _platforms;
        private readonly ulong _platformsCount;

        public Location(bool is1st, Vector2Int pos1st, Vector2Int pos2nd, ReadOnlySpan<Platform> platforms)
        {
            _is1stPlayer = is1st;
            _position1stPlayer = pos1st;
            _position2ndPlayer = pos2nd;

            fixed (Platform* p = platforms)
                _platforms = p;

            _platformsCount = (ulong)platforms.Length;
        }

        public bool IsFirstPlayer => _is1stPlayer;
        public Vector2Int PositionFirstPlayer => _position1stPlayer;
        public Vector2Int PositionSecondPlayer => _position2ndPlayer;
        public ReadOnlySpan<Platform> Platforms => new(_platforms, (int)_platformsCount);
    }
}