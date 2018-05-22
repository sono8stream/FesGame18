using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    public int value;

    private void Start()
    {
        value=0;
    }

    protected override void EffectFire(PlayerStatus playerStatus)
    {
        playerStatus.money += value;
    }
}