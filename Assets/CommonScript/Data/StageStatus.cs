using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatus
{
    public readonly string name,description;
    public readonly Sprite backSprite, sumbnailSprite;

    public StageStatus(string name,string description,
        Sprite back = null,Sprite sumbnail=null)
    {
        this.name = name;
        this.description = description;
        backSprite = back;
        sumbnailSprite = sumbnail;
    }
}
