using System;
using UnityEngine;


namespace Api
{
    public ref struct Location
    {
        public bool IsFirstPlayer;
        public Vector2Int PositionFirstPlayer;
        public Vector2Int PositionSecondPlayer;
        public Span<Platform> Platfroms;
    }
}