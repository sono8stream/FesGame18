using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KoitanLib{
    public  class KoitanInput:MonoBehaviour{
        public static List<Dictionary<ButtonID, KoitanButton>> ButtonTable = new List<Dictionary<ButtonID, KoitanButton>>();
        public static List<Dictionary<Axis, KoitanAxis>> AxisTable = new List<Dictionary<Axis, KoitanAxis>>();

        public static List<int> connectionOrderNumber = new List<int>();

        public static string[] controllerNames;

        private static bool isConnecting;
        private static int controllerCount;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            connectionOrderNumber.Add(0);
            SetButtonAxis();
        }

        private void Update()
        {
            if(controllerCount!=Input.GetJoystickNames().Length){
                SetButtonAxis();
                connectionOrderNumber.Clear();
                connectionOrderNumber.Add(0);
            }
            controllerCount = Input.GetJoystickNames().Length;

            if (isConnecting){
                string button_text = "";
                List<int> keepingButtons = new List<int>();
                for (int i = 1; i <= 10; i++)
                {
                    keepingButtons.Clear();
                    for (int j = 0; j <= 19; j++)
                    {
                        //デバッグ
                        string button_name = "joystick " + i.ToString() + " button " + j.ToString();

                        if (Input.GetKey(button_name))
                        {
                            keepingButtons.Add(i);
                        }

                        //接続順
                        if (keepingButtons.Count >= 2)
                        {
                            if (connectionOrderNumber.Contains(i) == false)
                            {
                                connectionOrderNumber.Add(i);
                            }
                        }
                    }
                }
            }
        }

        public static void SetButtonAxis()
        {
            controllerNames = Input.GetJoystickNames();
            ButtonTable.Clear();
            AxisTable.Clear();
            for (int i = 0; i < controllerNames.Length; i++)
            {
                switch (controllerNames[i])
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
            }
        }

        public static bool GetButton(ButtonID button,int conNum = -1){
            if(conNum==-1){
                for (int i = 0; i < controllerNames.Length;i++){
                    if (ButtonTable[connectionOrderNumber[i]][button].GetButton()) return true;
                }
                return false;
            }
            else return ButtonTable[connectionOrderNumber[conNum]][button].GetButton();
        }

        public static bool GetButtonDown(ButtonID button, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerNames.Length; i++)
                {
                    if (ButtonTable[connectionOrderNumber[i]][button].GetButtonDown()) return true;
                }
                return false;
            }
            else return ButtonTable[connectionOrderNumber[conNum]][button].GetButtonDown();
        }

        public static bool GetButtonUp(ButtonID button, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerNames.Length; i++)
                {
                    if (ButtonTable[connectionOrderNumber[i]][button].GetButtonUp()) return true;
                }
                return false;
            }
            else return ButtonTable[connectionOrderNumber[conNum]][button].GetButtonUp();
        }

        public static float GetAxis(Axis axis, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerNames.Length; i++)
                {
                    float value = AxisTable[connectionOrderNumber[i]][axis].GetAxis();
                    if (value !=0) return value;
                }
                return 0;
            }
            else return AxisTable[connectionOrderNumber[conNum]][axis].GetAxis();
        }

        public static float GetAxisDown(Axis axis, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerNames.Length; i++)
                {
                    float value = AxisTable[connectionOrderNumber[i]][axis].GetAxisDown();
                    if (value != 0) return value;
                }
                return 0;
            }
            else return AxisTable[connectionOrderNumber[conNum]][axis].GetAxisDown();
        }

        public static float GetAxisUp(Axis axis, int conNum = -1)
        {
            if (conNum == -1)
            {
                for (int i = 0; i < controllerNames.Length; i++)
                {
                    float value = AxisTable[connectionOrderNumber[i]][axis].GetAxisUp();
                    if (value != 0) return value;
                }
                return 0;
            }
            else return AxisTable[connectionOrderNumber[conNum]][axis].GetAxisUp();
        }

        public static void StartConnection(){
            isConnecting = true;
        }

        public static void StopConnection(){
            isConnecting = false;
            SetButtonAxis();
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


