using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    protected override void EffectFire(PlayerStatus playerStatus)
    {
        playerStatus.money += 100;
    }
}