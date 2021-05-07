using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    public int value = 0;
    [SerializeField]
    AudioClip getSE;

    private void Start()
    {

    }

    protected override void EffectFire(PlayerStatus playerStatus)
    {
        playerStatus.money += value;
        SoundPlayer.Find().PlaySE(getSE);
    }
}