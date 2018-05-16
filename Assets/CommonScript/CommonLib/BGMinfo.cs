using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGMinfo
{
    public AudioClip clip;
    public float loopBeginSec;
    public float loopEndSec;

    public BGMinfo(AudioClip clip, float loopBeginSec = -1, float loopEndSec = -1)
    {
        this.clip = clip;
        this.loopBeginSec = loopBeginSec == -1 ? 0:loopBeginSec;
        this.loopEndSec = loopEndSec == -1 ? clip.length : loopEndSec;
    }
}
