using Api;
using UnityEngine;


namespace Game.Scripts.Services
{
    public interface IViewObjectFactory
    {
        void ShowShape(IGameObject obj, Color color);

        void ShowPlayer(IPlayer player);

        void Clear();
    }
}