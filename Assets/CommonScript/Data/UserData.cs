using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public static UserData instance = new UserData();

    const int playerCount = 4;

    public Dictionary<string, int> variableDict;//消さない

    public KeySet[] playersKeySet;
    public WeaponSet[] playersWeapon;
    public CharacterID[] characters;
    public CharaSet charaSet;
    public int selectedStageNo;
    public int winnerIndex;
    public int winnerMoney;
    public StageSet stages;

    Weapon[] weaponData;

    private UserData()
    {
        playersKeySet = new KeySet[playerCount] {
            new KeySet(KeySetName.KeyboardL), new KeySet(KeySetName.KeyboardR),
            new KeySet(KeySetName.KeyboardL), new KeySet(KeySetName.KeyboardR) };

        winnerIndex = 0;
        winnerMoney = 1000000;

        playersWeapon = new WeaponSet[playerCount] {
            new WeaponSet(), new WeaponSet(),
            new WeaponSet(), new WeaponSet() };
        characters = new CharacterID[playerCount];
        charaSet = new CharaSet();

        variableDict = SaveManager.LoadVariableDict();

        stages = new StageSet();
        selectedStageNo = 0;
    }
}