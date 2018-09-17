﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KoitanLib
{
    public class KoitanInput : MonoBehaviour
    {
        public static List<Dictionary<ButtonID, KoitanButton>> ButtonTable = new List<Dictionary<ButtonID, KoitanButton>>();
        public static List<Dictionary<Axis, KoitanAxis>> AxisTable = new List<Dictionary<Axis, KoitanAxis>>();

        private static List<string> controllerNames;
        private static List<int> orderList;
        private static bool isConnecting;
        private static int controllerCount;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            controllerCount = 0;
        }

        private void Update()
        {
            controllerNames = Input.GetJoystickNames().Where(
                x => !string.IsNullOrEmpty(x)).ToList();
            if (controllerCount != controllerNames.Count)
            {//接続数変更時
                SetButtonAxis();
            }
            controllerCount = controllerNames.Count;

            if (Input.GetKeyDown(KeyCode.J))
            {
                isConnecting = true;
            }

            if (isConnecting)
            {
                /*for (int i = 1; i <= 10; i++)
                {
                    List<int> keepingButtons = new List<int>();
                    for (int j = 0; j <= 19; j++)
                    {
                        //デバッグ
                        string button_name 
                            = "joystick " + i.ToString() + " button " + j.ToString();

                        if (Input.GetKey(button_name))
                        {
                            keepingButtons.Add(i);
                        }
                    }
                }*/
                for (int i = 0; i < controllerCount; i++)//再接続設定
                {
                    if (GetButton(ButtonID.A, i) && GetButton(ButtonID.B, i))
                    {
                        orderList.Remove(i);
                        orderList.Add(i);
                        break;
                    }
                }
            }
        }

        public static void SetButtonAxis()
        {
            ButtonTable = new List<Dictionary<ButtonID, KoitanButton>>();
            AxisTable = new List<Dictionary<Axis, KoitanAxis>>();
            orderList = new List<int>();

            string[] joystickNames = Input.GetJoystickNames();
            int controllerIndex = 0;
            for (int i = 0; i < joystickNames.Length
                && controllerIndex < controllerNames.Count; i++)
            {
                if (joystickNames[i] != controllerNames[controllerIndex]) continue;
                switch (joystickNames[i])
                {
                    case "PAD A":
                    case "HORI CO.,LTD  PAD A":
                        ButtonTable.Add(new Dictionary<ButtonID, KoitanButton>
                    {
                        {ButtonID.A,new KoitanButton(ConType.JoyButton,i+1,2)},
                        {ButtonID.B,new KoitanButton(ConType.JoyButton,i+1,1)},
                        {ButtonID.X,new KoitanButton(ConType.JoyButton,i+1,3)},
                        {ButtonID.Y,new KoitanButton(ConType.JoyButton,i+1,0)}
                    });
                        AxisTable.Add(new Dictionary<Axis, KoitanAxis>
                    {
                        {Axis.L_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,0,false,0.1f)},
                        {Axis.L_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,1,false,0.1f)},
                        {Axis.R_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,2,false,0.1f)},
                        {Axis.R_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,3,false,0.1f)},
                        {Axis.Cross_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,4,false,0.1f)},
                        {Axis.Cross_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,5,false,0.1f)}
                    });
                        break;
                    case "Sony Interactive Entertainment Wireless Controller":
                        ButtonTable.Add(new Dictionary<ButtonID, KoitanButton>
                    {
                        {ButtonID.A,new KoitanButton(ConType.JoyButton,i+1,2)},
                        {ButtonID.B,new KoitanButton(ConType.JoyButton,i+1,1)},
                        {ButtonID.X,new KoitanButton(ConType.JoyButton,i+1,3)},
                        {ButtonID.Y,new KoitanButton(ConType.JoyButton,i+1,0)}
                    });
                        AxisTable.Add(new Dictionary<Axis, KoitanAxis>
                    {
                        {Axis.L_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,0,false,0.1f)},
                        {Axis.L_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,1,false,0.1f)},
                        {Axis.R_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,2,false,0.1f)},
                        {Axis.R_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,3,false,0.1f)},
                        {Axis.Cross_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,6,false,0.1f)},
                        {Axis.Cross_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,7,false,0.1f)}
                    });
                        break;
                    case "Wireless Gamepad"://windows joycon(右左共通)
                        ButtonTable.Add(new Dictionary<ButtonID, KoitanButton>
                    {
                        {ButtonID.A,new KoitanButton(ConType.JoyButton,i+1,1)},
                        {ButtonID.B,new KoitanButton(ConType.JoyButton,i+1,0)},
                        {ButtonID.X,new KoitanButton(ConType.JoyButton,i+1,3)},
                        {ButtonID.Y,new KoitanButton(ConType.JoyButton,i+1,2)}
                    });
                        AxisTable.Add(new Dictionary<Axis, KoitanAxis>
                    {
                        {Axis.L_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,8,false,0.1f)},
                        {Axis.L_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,9,true,0.1f)}
                    });
                        break;
                    case "Unknown Joy-Con (L)":
                        ButtonTable.Add(new Dictionary<ButtonID, KoitanButton>
                    {
                        {ButtonID.A,new KoitanButton(ConType.JoyButton,i+1,1)},
                        {ButtonID.B,new KoitanButton(ConType.JoyButton,i+1,0)},
                        {ButtonID.X,new KoitanButton(ConType.JoyButton,i+1,3)},
                        {ButtonID.Y,new KoitanButton(ConType.JoyButton,i+1,2)}
                    });
                        AxisTable.Add(new Dictionary<Axis, KoitanAxis>
                    {
                        {Axis.Cross_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,10,false,0.1f)},
                        {Axis.Cross_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,11,false,0.1f)}
                    });
                        break;
                    case "Unknown Joy-Con (R)":
                        ButtonTable.Add(new Dictionary<ButtonID, KoitanButton>
                    {
                        {ButtonID.A,new KoitanButton(ConType.JoyButton,i+1,1)},
                        {ButtonID.B,new KoitanButton(ConType.JoyButton,i+1,0)},
                        {ButtonID.X,new KoitanButton(ConType.JoyButton,i+1,3)},
                        {ButtonID.Y,new KoitanButton(ConType.JoyButton,i+1,2)}
                    });
                        AxisTable.Add(new Dictionary<Axis, KoitanAxis>
                    {
                        {Axis.Cross_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,10,false,0.1f)},
                        {Axis.Cross_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,11,false,0.1f)}
                    });
                        break;
                    case "HORI PAD 3 TURBO":
                        ButtonTable.Add(new Dictionary<ButtonID, KoitanButton>
                    {
                        {ButtonID.A,new KoitanButton(ConType.JoyButton,i+1,2)},
                        {ButtonID.B,new KoitanButton(ConType.JoyButton,i+1,1)},
                        {ButtonID.X,new KoitanButton(ConType.JoyButton,i+1,3)},
                        {ButtonID.Y,new KoitanButton(ConType.JoyButton,i+1,0)}
                    });
                        AxisTable.Add(new Dictionary<Axis, KoitanAxis>
                    {
                        {Axis.L_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,0,false,0.1f)},
                        {Axis.L_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,1,false,0.1f)},
                        {Axis.R_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,2,false,0.1f)},
                        {Axis.R_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,3,false,0.1f)},
                        {Axis.Cross_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,4,false,0.1f)},
                        {Axis.Cross_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,5,true,0.1f)}
                    });
                        break;
                    default:
                        ButtonTable.Add(new Dictionary<ButtonID, KoitanButton>
                    {
                        {ButtonID.A,new KoitanButton(ConType.JoyButton,i+1,2)},
                        {ButtonID.B,new KoitanButton(ConType.JoyButton,i+1,1)},
                        {ButtonID.X,new KoitanButton(ConType.JoyButton,i+1,3)},
                        {ButtonID.Y,new KoitanButton(ConType.JoyButton,i+1,0)}
                    });
                        AxisTable.Add(new Dictionary<Axis, KoitanAxis>
                    {
                        {Axis.L_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,0,false,0.1f)},
                        {Axis.L_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,1,false,0.1f)},
                        {Axis.R_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,2,false,0.1f)},
                        {Axis.R_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,3,false,0.1f)},
                        {Axis.Cross_Horizontal,new KoitanAxis(ConType.JoyAxis,i+1,4,false,0.1f)},
                        {Axis.Cross_Vertical,new KoitanAxis(ConType.JoyAxis,i+1,5,false,0.1f)}
                    });
                        break;
                }
                orderList.Add(controllerIndex);
                controllerIndex++;
            }
        }

        public static bool GetButton(ButtonID button, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerCount; i++)
                {
                    if (string.IsNullOrEmpty(controllerNames[i])) continue;
                    if (ButtonTable[i].ContainsKey(button)
                        && ButtonTable[i][button].GetButton()) return true;
                }
                return false;
            }
            else return ButtonTable[conNum][button].GetButton();
        }

        public static bool GetButtonDown(ButtonID button, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerCount; i++)
                {
                    if (string.IsNullOrEmpty(controllerNames[i])) continue;
                    if (ButtonTable[i].ContainsKey(button)
                        && ButtonTable[i][button].GetButtonDown()) return true;
                }
                return false;
            }
            else return ButtonTable[conNum][button].GetButtonDown();
        }

        public static bool GetButtonUp(ButtonID button, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerCount; i++)
                {
                    if (String.IsNullOrEmpty(controllerNames[i])) continue;
                    if (ButtonTable[i].ContainsKey(button)
                        && ButtonTable[i][button].GetButtonUp()) return true;
                }
                return false;
            }
            else return ButtonTable[conNum][button].GetButtonUp();
        }

        public static float GetAxis(Axis axis, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerCount; i++)
                {
                    if (String.IsNullOrEmpty(controllerNames[i])
                        || !AxisTable[i].ContainsKey(axis)) continue;
                    float value = AxisTable[i][axis].GetAxis();
                    if (value != 0) return value;
                }
                return 0;
            }
            else return AxisTable[conNum][axis].GetAxis();
        }

        public static float GetAxisDown(Axis axis, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerCount; i++)
                {
                    if (String.IsNullOrEmpty(controllerNames[i])
                        || !AxisTable[i].ContainsKey(axis)) continue;
                    float value = AxisTable[i][axis].GetAxisDown();
                    if (value != 0) return value;
                }
                return 0;
            }
            else return AxisTable[conNum][axis].GetAxisDown();
        }

        public static float GetAxisUp(Axis axis, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerCount; i++)
                {
                    if (String.IsNullOrEmpty(controllerNames[i])
                        || !AxisTable[i].ContainsKey(axis)) continue;
                    float value = AxisTable[i][axis].GetAxisUp();
                    if (value != 0) return value;
                }
                return 0;
            }
            else return AxisTable[conNum][axis].GetAxisUp();
        }

        public static string ControllerNames()
        {
            string s = "";
            for(int i = 0; i < controllerCount; i++)
            {
                s += controllerNames[orderList[i]] + Environment.NewLine;
            }
            return s;
        }

        public static void StartReconnection()
        {
            isConnecting = true;
        }

        public static void EndReconnection()
        {
            isConnecting = false;
        }
    }
}

public enum ButtonID
{
    A,
    B,
    X,
    Y,
    L,
    R
}

public enum Axis
{
    L_Horizontal,
    L_Vertical,
    R_Horizontal,
    R_Vertical,
    Cross_Horizontal,
    Cross_Vertical
}

public enum ButtonName
{
    Jump,
    Attack,
    SubAttack
}

public enum AxisName
{
    MoveX,
    MoveY
}
public enum ConType
{
    JoyButton,
    JoyAxis,
    Key,
    MouseMovement
}


