using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerAnim : MonoBehaviour
{
    const string wipeIntervalName = "WipeInterval";

    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GetComponent<Animator>().SetTrigger("Wipe");
        }
    }

    public void SetWipeInterval(int val)
    {
        animator.SetInteger(wipeIntervalName, val);
    }

    public void IterateWipeInterval()
    {
        int val=animator.GetInteger(wipeIntervalName);
        SetWipeInterval(val + 1);
    }
}