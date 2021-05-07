using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public static UserData instance = new UserData();

    const int playerLimit = 4;

    public Dictionary<string, int> variableDict;//消さない

    public KeySet[] playersKeySet;
    public WeaponSet[] playersWeapon;
    public CharacterID[] characters;
    public CharaSet charaSet;
    public int selectedStageSceneNo;
    public int winnerIndex;
    public int winnerMoney;
    public int playerCount;
    public StageSet stages;

    Weapon[] weaponData;

    //Add by koitan at 12/2
    public bool[] isCpu = new bool[playerLimit];

    private UserData()
    {
        playersKeySet = new KeySet[playerLimit] {
            new KeySet(KeySetName.KeyboardL), new KeySet(KeySetName.KeyboardR),
            new KeySet(KeySetName.KeyboardL), new KeySet(KeySetName.KeyboardR) };

        winnerIndex = 0;
        winnerMoney = 1000000;

        playersWeapon = new WeaponSet[playerLimit] {
            new WeaponSet(), new WeaponSet(),
            new WeaponSet(), new WeaponSet() };
        characters = new CharacterID[playerLimit];
        playerCount = 2;
        characters[0] = CharacterID.Boy1;
        characters[1] = CharacterID.Girl1;
        charaSet = new CharaSet();

        variableDict = SaveManager.LoadVariableDict();

        stages = new StageSet();
        selectedStageSceneNo = 0;
    }
}