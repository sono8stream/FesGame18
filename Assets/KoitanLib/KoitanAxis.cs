using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoitanAxis{

    private string name;
    private ConType conType;
    private int axisNum;
    private float deadline;
    private bool isInvert;
    private int positiveButtonNum;
    private int negativeButtonNum;
    private int orderNum;
    private KeyCode positiveKeyCode;
    private KeyCode negativeKeyCode;
    private string positiveName;
    private string negativeName;

    private float fdValue = 0;
    private float cdValue = 0;
    private float dNow = 0;

    private float fuValue = 0;
    private float cuValue = 0;
    private float uNow = 0;

    private bool isAI;
    private float aiValue;


    public KoitanAxis(ConType conType, int orderNum, int axisNum, bool isInvert, float deadline)
    {
        this.conType = conType;
        switch (conType)
        {
            case ConType.JoyAxis:
                this.orderNum = orderNum;
                this.axisNum = axisNum;
                this.isInvert = isInvert;
                this.deadline = deadline;
                this.name = "joystick " + orderNum.ToString() + " analog " + axisNum.ToString();
                break;
        }
    }

    public KoitanAxis(ConType conType, int orderNum, int positiveButtonNum, int negativeButtonNum)
    {
        this.conType = conType;
        switch (conType)
        {
            case ConType.JoyButton:
                this.orderNum = orderNum;
                this.positiveButtonNum = positiveButtonNum;
                this.negativeButtonNum = negativeButtonNum;
                this.positiveName = "joystick " + orderNum.ToString() + " button " + positiveButtonNum.ToString();
                this.negativeName = "joystick " + orderNum.ToString() + " button " + negativeButtonNum.ToString();
                break;
        }
    }

    public KoitanAxis(ConType conType, KeyCode positiveKeyCode, KeyCode negativeKeyCode)
    {
        this.conType = conType;
        switch (conType)
        {
            case ConType.Key:
                this.positiveKeyCode = positiveKeyCode;
                this.negativeKeyCode = negativeKeyCode;
                break;
        }
    }

    public void SetIsAI(bool avilable)
    {
        isAI = avilable;
    }

    public void SetAIValue(float value)
    {
        aiValue = value;
    }

    public float GetAxis(){
        //外部からの操作を受け付ける
        if (isAI) return aiValue;
        switch(conType){
            case ConType.JoyAxis:
                float inputValue = Input.GetAxis(name);
                if (Mathf.Abs(inputValue) < deadline) return 0;
                else{
                    return isInvert ? -inputValue : inputValue;
                }
                break;
            case ConType.JoyButton:
                if (Input.GetKey(positiveName))
                {
                    return 1;
                }
                else if (Input.GetKey(negativeName))
                {
                    return -1;
                }
                else return 0;
                break;
            case ConType.Key:
                if (Input.GetKey(positiveKeyCode))
                {
                    return 1;
                }
                else if (Input.GetKey(negativeKeyCode))
                {
                    return -1;
                }
                else return 0;
                break;
        }
        return 0;
    }

    public float GetAxisDown()
    {
        if(dNow != Time.time){
            dNow = Time.time;
            cdValue = GetAxis();
            if (fdValue == 0 && cdValue != 0)
            {
            }
            else{
                cdValue = 0;
            }
            fdValue = GetAxis();
        }
        return cdValue;
    }

    public float GetAxisUp()
    {
        if (uNow != Time.time)
        {
            uNow = Time.time;
            cuValue = GetAxis();
            if ((fuValue != 0 && cuValue == 0))
            {
                cuValue = fuValue;
            }
            else
            {
                cuValue = 0;
            }
            fuValue = GetAxis();
        }
        return cuValue;
    }

}
