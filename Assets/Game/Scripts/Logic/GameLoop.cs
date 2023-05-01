using Api;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class GameLoop : MonoBehaviour
    {
        public Platform[] GatherPlatforms()
        {
            var config = Resources.Load<GameConfig>(AssetPath.GameConfig);
            var markers = GetComponentsInChildren<PlatformMarker>();
            int count = markers.Length;
            var platforms = new Platform[count];

            for (int i = 0; i < count; i++)
                platforms[i] = markers[i].ToPlatform(i, config);

            return platforms;
        }

    }
}
