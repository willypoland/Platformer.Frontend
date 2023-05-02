using UnityEngine;


namespace Game.Scripts.Common
{
    public static class DebugDraw
    {
        public static void Rect(Rect rect, Color color, float time = 0f, bool depthTest = false)
        {
            var lb = rect.position;
            var rt = rect.position + rect.size;
            var rb = new Vector2(rt.x, lb.y);
            var lt = new Vector2(lb.x, rt.y);

            Debug.DrawLine(lb, rb, color, time, depthTest);
            Debug.DrawLine(lb, lt, color, time, depthTest);
            Debug.DrawLine(rt, rb, color, time, depthTest);
            Debug.DrawLine(rt, lt, color, time, depthTest);
        }

        public static void Circle(Vector3 center, Vector3 axis, float radius, int edges, Color color, float time = 0f, bool depthTest = false)
        {
            float angle = 360f / edges;
            Quaternion rot = Quaternion.AngleAxis(angle, axis);

            Vector3 a = Vector3.up * radius;
            for (int i = 0; i < edges; i++)
            {
                Vector3 b = rot * a;
                Debug.DrawLine(center + a, center + b, color, time, depthTest);
                a = b;
            }
        }

        public static void Cirlce2D(Vector2 center, float radius, int edges, Color color, float time = 0f, bool depthTest = false)
        {
            Circle(center, Vector3.forward, radius, edges, color, time, depthTest);
        }
    }
}