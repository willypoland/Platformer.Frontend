using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Game;
using Game.Scripts.Logic;
using Ser;
using TMPro;
using UnityEngine;


public class PlayerStateView : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtPosition;
    [SerializeField] private TMP_Text _txtVelocity;
    [SerializeField] private TMP_Text _txtAnimation;
    [SerializeField] private TMP_Text _txtStateFrame;
    [SerializeField] private TMP_Text _txtState;
    [SerializeField] private TMP_Text _txtOnGround;
    [SerializeField] private TMP_Text _txtOnDamage;
    [SerializeField] private TMP_Text _txtLeftDir;
    [SerializeField] private TMP_Text _txtPrevInput;

    private Camera _camera;

    public void Set(Player player, Vector3 world, Sprite sprite, FixedAnimation anim, int index)
    {
        if (!gameObject.activeSelf)
            return;
        
        _camera ??= Camera.main;

        var screen = _camera.WorldToScreenPoint(world);
        transform.position = screen;

        _txtPosition.SetText("pos: ({0:1}  {1:1})", player.Obj.Position.X, player.Obj.Position.Y);
        _txtVelocity.SetText("vel: ({0:2}  {1:2})", player.Obj.Velocity.X, player.Obj.Velocity.Y);
        _txtAnimation.SetText($"{sprite.name} ({anim.SpriteCount} {anim.TimeInFrames} {anim.Repeate})");
        _txtStateFrame.SetText("{0} -> {1}", player.StateFrame, (float)index);
        _txtState.SetText(GetStateName(player.State));
        UpdateColor(_txtOnGround, player.OnGround);
        UpdateColor(_txtOnDamage, player.OnDamage);
        UpdateColor(_txtLeftDir, player.LeftDirection);
        UpdateBits(_txtPrevInput, player.PrevInput);
    }

    private void UpdateBits(TMP_Text text, int bits)
    {
        float b0 = (bits & (1 << 0)) > 0 ? 1f : 0f;
        float b1 = (bits & (1 << 1)) > 0 ? 1f : 0f;
        float b2 = (bits & (1 << 2)) > 0 ? 1f : 0f;
        float b3 = (bits & (1 << 3)) > 0 ? 1f : 0f;
        float b4 = (bits & (1 << 4)) > 0 ? 1f : 0f;
        text.SetText("{0}{1}{2}{3}{4}", b0, b1, b2, b3, b4);
    }

    private void UpdateColor(TMP_Text text, bool value)
    {
        var col = text.color;
        col.a = value ? 1f : 0.2f;
        text.color = col;
    }

    private string GetStateName(Ser.PlayerState state)
    {
        return state switch
        {
            Ser.PlayerState.Death => "Death",
            Ser.PlayerState.Falling => "Falling",
            Ser.PlayerState.Idle => "Idle",
            Ser.PlayerState.Jump => "Jump",
            Ser.PlayerState.Landing => "Landing",
            Ser.PlayerState.Run => "Run",
            Ser.PlayerState.AttackOnGround => "AttackOnGround",
            _ => state.ToString()
        };
    }
}
