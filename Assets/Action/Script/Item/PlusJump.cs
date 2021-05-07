using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusJump : Item
{
    protected override void EffectFire(PlayerStatus playerStatus)
    {
        playerStatus.ChangeStatus((int)StatusNames.airJumpLims, 1, 15);
    }
}
