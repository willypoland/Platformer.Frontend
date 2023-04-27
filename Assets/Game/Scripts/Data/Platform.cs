using Game.Scripts.Logic;
using UnityEngine;


namespace Game.Scripts.Data
{
    public struct Platform
    {
        public int Id;
        public PlatformType Type;
        public int Width;
        public int Height;
        public Vector2Int Position;

        public override string ToString() => $"[{Id}]<{Type}> {Width}x{Height} {Position}";
    }
}