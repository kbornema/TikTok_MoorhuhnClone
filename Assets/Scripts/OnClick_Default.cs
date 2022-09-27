using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick_Default : MonoBehaviour, IClickCallback
{
    [SerializeField]
    private GameObject _root = default;
    [SerializeField]
    private Faction _faction = default;
    [SerializeField]
    private int _points = 100;

    [SerializeField]
    private List<AudioClip> _hits = default;
    [SerializeField]
    private List<AudioClip> _happy = default;

    public void OnClick(Player player, Player.Tool tools)
    {
        if(tools == Player.Tool.Neutral)
        {
            return;
        }

        var comboPoints = (int)(_points * player.MusicComboService.GetHitAccuracy());
        if (player.CurrentFaction == Faction.Neutral)
        {
            if (tools == Player.Tool.Negative)
            {
                player.SetHostile(_faction);
            }
            else if(tools == Player.Tool.Positive)
            {
                player.SetFriend(_faction);
            }
        }

        if (_faction == player.CurrentFaction)
        {
            if (tools == Player.Tool.Negative)
            {
                SfxClip.Play(_hits);
                TriggerNegative(comboPoints, tools);
            }
            else if (tools == Player.Tool.Positive)
            {
                SfxClip.Play(_happy);
                TriggerPositive(comboPoints, tools);
            }
        }
        else
        {
            if (tools == Player.Tool.Negative)
            {
                SfxClip.Play(_hits);
                TriggerPositive(comboPoints, tools);
            }
            else if (tools == Player.Tool.Positive)
            {
                SfxClip.Play(_happy);
                TriggerNegative(comboPoints, tools);
            }
        }

        GameObject.Destroy(_root);
    }

    private void TriggerNegative(int points, Player.Tool tool)
    {
        UIManager.Instance.AddScore(-points);
    }

    private void TriggerPositive(int points, Player.Tool tool)
    {
        var uiManager = UIManager.Instance;
        uiManager.CheckCombo(points, tool, _faction);
        uiManager.AddScore(points);

        uiManager.AddOppositeAmmo(tool, 1);
    }
}
