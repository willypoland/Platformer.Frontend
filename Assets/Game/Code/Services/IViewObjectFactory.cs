using Api;
using UnityEngine;


namespace Game.Code.Logic
{
    public interface IViewObjectFactory
    {
        void ShowShape(IGameObject obj, Color color);

        void ShowPlayer(IPlayer player);

        void Clear();
    }
}