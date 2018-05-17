using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public static UserData instance = new UserData();

    const int playerCount = 2;

    public KeySet[] playersKeySet;
    public WeaponSet[] playersWeapon;
    public int winnerIndex;
    public int winnerScore;
    public Dictionary<string, int> variableDict;//消さない

    Weapon[] weaponData;

    private UserData()
    {
        playersKeySet = new KeySet[playerCount] {
            new KeySet(KeySetName.KeyboardL), new KeySet(KeySetName.KeyboardR) };

        winnerIndex = 0;
        winnerScore = 1000000;

        playersWeapon = new WeaponSet[playerCount] {
            new WeaponSet(), new WeaponSet() };

        variableDict = SaveManager.LoadVariableDict();
    }
}