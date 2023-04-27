using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class PlatfromScene : MonoBehaviour
    {
        public Platform[] GatherPlatforms()
        {
            var config = Resources.Load<GameConfig>(AssetPath.GameConfig);
            var markers = GetComponentsInChildren<PlatformMarker>();
            int count = markers.Length;
            var platforms = new Platform[count];

            for (int i = 0; i < count; i++)
                platforms[i] = ToPlatfrom(i, markers[i], config);

            return platforms;
        }
        
        public Platform ToPlatfrom(int id, PlatformMarker marker, GameConfig config)
        {
            var rect = marker.GetRect();
            Vector2Int min = (rect.min * config.UnitMultiply * config.UnitScale).RoundToInt();
            Vector2Int size = (rect.size * config.UnitScale).RoundToInt();

            return new Platform()
            {
                Id = id,
                Type = marker.Type,
                Width = size.x,
                Height = size.y,
                Position = min
            };
        }

    }
}
