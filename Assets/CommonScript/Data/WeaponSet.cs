using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSet
{
    public const int weaponCount = 2;

    List<WeaponStatus> weaponList;
    int[] weaponIndexes;

    public WeaponSet()
    {
        Initialize();
        weaponIndexes = new int[2] { 0, 1 };
    }

    public WeaponStatus GetWeaponByIndex(int index)
    {
        return weaponList[index];
    }

    public WeaponStatus GetWeaponByType(WeaponType type)
    {
        return weaponList[weaponIndexes[(int)type]];
    }

    public int GetWeaponIndexByIndex(int index)
    {
        return weaponIndexes[index];
    }

    public void SetWeaponStatus(int typeNo,int weaponIndex)
    {
        weaponIndexes[typeNo] = weaponIndex;
    }

    public void Initialize()
    {
        weaponList = new List<WeaponStatus>();
        weaponList.Add(new WeaponStatus("鳴子","よさこいの必需品。", 10, 10, 20));
        weaponList.Add(new WeaponStatus("看板","宣伝になる。", 5, 20, 10));
        weaponList.Add(new WeaponStatus("パイプ椅子","座れる。", 5, 30, 5));
        weaponList.Add(new WeaponStatus("消火器","火を消せる。", 20, 20, 5));
        weaponList.Add(new WeaponStatus("BB弾","弾が打てる。", 25, 10, 10));
    }
}

public enum WeaponType
{
    Main = 0, Sub
}
