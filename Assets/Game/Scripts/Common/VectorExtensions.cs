using UnityEngine;


namespace Game.Scripts.Common
{
    public static class VectorExtensions
    {
        public static Vector2Int RoundToInt(this Vector2 self) =>
            new Vector2Int(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));
        
        public static Vector3Int RoundToInt(this Vector3 self) =>
            new Vector3Int(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y), Mathf.RoundToInt(self.z));

        public static Vector2 ToFloat(this Vector2Int self) =>
            new Vector2(self.x, self.y);
    }
}