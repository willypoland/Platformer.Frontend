using System;
using Api;
using Game.Scripts.Common;
using Game.Scripts.Data;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class SceneConverter
    {
        private readonly GameConfig _config;

        public SceneConverter(GameConfig config)
        {
            _config = config;
        }
        
        public RectInt ToCoreRect(Rect viewRect) =>
            ToCoreRect(viewRect, _config.UnitScale, _config.FlipY);

        public Rect ToViewRect(RectInt coreRect) =>
            ToViewRect(coreRect, _config.UnitScale, _config.FlipY);

        public Platform ToCorePlatform(int i, PlatformType type, Rect viewRect) => 
            ToCorePlatform(i, type, ToCoreRect(viewRect));

        public static Platform ToCorePlatform(int i, PlatformType type, RectInt coreRect)
        {
            return new Platform
            {
                Id = i,
                Type = type,
                Width = coreRect.width,
                Height = coreRect.height,
                Position = coreRect.position,
            };
        }

        public static RectInt ToCoreRect(Rect viewRect, float scale, bool flipY)
        {
            float x = viewRect.x;
            float y = viewRect.y + viewRect.height;
            if (flipY)
                y = -y;
            
            Vector2Int pos = (new Vector2(x, y) * scale).RoundToInt();
            Vector2Int size = (viewRect.size * scale).RoundToInt();
            return new RectInt(pos, size);
        }

        public static Rect ToViewRect(RectInt coreRect, float scale, bool flipY)
        {
            int x = coreRect.x;
            int y = coreRect.y + coreRect.height;
            if (flipY)
                y = -y;

            Vector2 pos = new Vector2(x, y) / scale;
            Vector2 size = (Vector2)coreRect.size / scale;
            return new Rect(pos, size);
        }

    }
}