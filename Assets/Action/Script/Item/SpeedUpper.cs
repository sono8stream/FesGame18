﻿public class SpeedUpper : Item
{
    protected override void EffectFire(PlayerStatus playerStatus)
    {
        int index = (int)StatusNames.speed;
        float val = playerStatus.TempStatus[index] * 2f;
        playerStatus.ChangeStatus((int)StatusNames.speed, val,2);
    }
}