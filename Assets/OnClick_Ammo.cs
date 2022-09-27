using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick_Ammo : MonoBehaviour, IClickCallback
{
    public GameObject Root = default;
    public Player.Tool Tool = default;
    public int Amount = 5;

    public void OnClick(Player player, Player.Tool tool)
    {
        if (tool == Player.Tool.Neutral)
        {
            UIManager.Instance.AddAmmo(Tool, Amount);
            GameObject.Destroy(Root);
        }
    }
}
