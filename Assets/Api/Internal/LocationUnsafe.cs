using UnityEngine;


namespace Api.Internal
{
    internal unsafe struct LocationUnsafe
    {
        public bool Is1stPlayer;
        public Vector2Int Position1stPlayer;
        public Vector2Int Position2ndPlaer;
        public Platform* Platforms;
        public ulong PlatformsCount;
    }
}