using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickCallback
{
    void OnClick(Player player, Player.Tool tool);
}
