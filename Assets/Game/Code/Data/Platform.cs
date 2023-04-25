using UnityEngine;


namespace Game
{
    public struct Platform
    {
        public int Id;
        public int Type;
        public int Width;
        public int Height;
        public Vector2Int Position;

        public override string ToString() => $"[{Id}]<{Type}> {Width}x{Height} {Position}";
    }
}