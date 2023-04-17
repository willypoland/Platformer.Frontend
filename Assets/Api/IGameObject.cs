using UnityEngine;


namespace Api
{
    public interface IGameObject
    {
        Vector2 Size { get; }
        Vector2 Position { get; }
        Vector2 Velocity { get; }
    }
}