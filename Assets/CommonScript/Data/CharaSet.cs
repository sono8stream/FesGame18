using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSet
{
    public Dictionary<CharacterID, CharaStatus> characters;

    public CharaSet()
    {
        characters = new Dictionary<CharacterID, CharaStatus>();
        characters.Add(CharacterID.Boy1, new CharaStatus("Boy1", "test", 10, 15, 10));
        characters.Add(CharacterID.Boy2, new CharaStatus("Boy2", "test", 15, 10, 10));
        characters.Add(CharacterID.Girl1, new CharaStatus("Girl1", "test", 5, 15, 15));
        characters.Add(CharacterID.Girl2, new CharaStatus("Girl2", "test", 15, 5, 15));
    }

    public CharaStatus this[CharacterID id]
    {
        get { return characters[id]; }
    }
}