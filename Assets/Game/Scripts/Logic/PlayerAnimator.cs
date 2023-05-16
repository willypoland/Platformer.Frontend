using System;
using Api;
using UnityEngine;


namespace Game.Scripts.Logic
{

    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private FixedAnimationController _controller;

        public void UpdateState(IPlayer player)
        {
            // int index = (int) player.State;
            // var sprite = _controller.GetSprite(index, player.StateFrame);
            // _renderer.sprite = sprite;
        }
    }
}