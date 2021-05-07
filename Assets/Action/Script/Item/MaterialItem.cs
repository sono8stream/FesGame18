using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialItem : Item {
    [SerializeField]
    int materialNo;
    [SerializeField]
    int count = 1;

    protected override void EffectFire(PlayerStatus playerStatus)
    {
        playerStatus.AddMaterial(materialNo, count);
    }
}