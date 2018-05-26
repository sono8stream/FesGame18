using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMaterialItem : Item
{
    [SerializeField]
    int count = 1;

    protected override void EffectFire(PlayerStatus playerStatus)
    {
        int matCount = System.Enum.GetNames(typeof(MaterialNames)).Length;
        for(int i = 0; i < matCount;i++)
        {
            playerStatus.AddMaterial(i, count);
        }
    }
}