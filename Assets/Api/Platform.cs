using UnityEngine;


namespace Api
{
    public struct Platform
    {
        public int Id;
        public PlatformType Type;
        public int Width;
        public int Height;
        public Vector2Int Position;

        public override string ToString() => $"[{Id}]<{Type}> {Width}x{Height} {Position}";

        public Platform(int id, int type, int width, int height, int x, int y)
        {
            Id = id;
            Type = (PlatformType)type;
            Width = width;
            Height = height;
            Position = new Vector2Int(x, y);
        }
    }
}