using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Test : MonoBehaviour {

    public Text joystickAxisText;
    public Text joystickStateText;
    public Text joystickButtonText;
    public Text joystickConnectionOrderText;
    public GameObject cube;
    public Transform[] conSprite;
    public Vector3[] conPos;

    public List<int> connectionOrderNumber;

    public string conName;

    List<Dictionary<ButtonID, KoitanButton>> ButtonTable = new List<Dictionary<ButtonID, KoitanButton>>();
    List<Dictionary<Axis, KoitanAxis>> AxisTable = new List<Dictionary<Axis, KoitanAxis>>();


    // Use this for initialization
    void Start () {
        for (int i = 0; i < Input.GetJoystickNames().Length;i++){
            connectionOrderNumber.Add(i+1);
        }
        SetButtonAxis();

        conName = Input.GetJoystickNames()[0];

        //デバッグ
        conPos = new Vector3[]{
            conSprite[10].position,
            conSprite[11].position
        };
	}

    // Update is called once per frame
    void Update()
    {
        string axis_text = "";
        string button_text = "";
        List<int> keepingButtons = new List<int>();
        for (int i = 1; i <= 10; i++)
        {
            keepingButtons.Clear();
            for (int j = 0; j <= 19; j++)
            {
                string axis_name = "joystick " + i.ToString() + " analog " + j.ToString();

                if (Input.GetAxis(axis_name) != 0)
                {
                    axis_text += axis_name + " : " + Input.GetAxis(axis_name) + "\n";
                }


                //デバッグ
                string button_name = "joystick " + i.ToString() + " button " + j.ToString();

                if (Input.GetKey(button_name))
                {
                    button_text += button_name + "\n";
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
        joystickAxisText.text = axis_text;
        joystickButtonText.text = button_text;

        string connectionOrder_text = "接続順\n";
        for (int i = 0; i < connectionOrderNumber.Count; i++)
        {
            connectionOrder_text += (i + 1).ToString() + " : " + connectionOrderNumber[i].ToString() + "\n";
        }
        joystickConnectionOrderText.text = connectionOrder_text;

        var controllerNames = Input.GetJoystickNames();
        string name_text = "";
        for (int i = 0; i < controllerNames.Length; i++)
        {
            name_text += (i + 1).ToString() + " : " + controllerNames[i] + "\n";
        }
        joystickStateText.text = name_text;

        //接続順リセット
        if (Input.GetKeyDown(KeyCode.Space))
        {
            connectionOrderNumber.Clear();
        }

        //デバッグ
        cube.transform.position = new Vector2(Input.GetAxis("joystick 1 analog 0"), -Input.GetAxis("joystick 1 analog 1"));


        if (ButtonTable[0][ButtonID.A].GetButtonDown())
        {
            Debug.Log("Aボタンが押されました");
        }

        if (ButtonTable[0][ButtonID.A].GetButtonUp())
        {
            Debug.Log("Aボタンが離されました");
        }
        int k = 1;
        conSprite[0].localScale = ButtonTable[k][ButtonID.A].GetButton() ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[1].localScale = ButtonTable[k][ButtonID.B].GetButton() ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[2].localScale = ButtonTable[k][ButtonID.X].GetButton() ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[3].localScale = ButtonTable[k][ButtonID.Y].GetButton() ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[8].localScale = (AxisTable[k][Axis.Cross_Horizontal].GetAxis() > 0) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[7].localScale = (AxisTable[k][Axis.Cross_Horizontal].GetAxis() < 0) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[6].localScale = (AxisTable[k][Axis.Cross_Vertical].GetAxis() > 0) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[5].localScale = (AxisTable[k][Axis.Cross_Vertical].GetAxis() < 0) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        //conSprite[10].position = new Vector2(conPos[0].x + AxisTable[k][Axis.R_Horizontal].GetAxis(), conPos[0].y - AxisTable[k][Axis.R_Vertical].GetAxis());
        //conSprite[11].position = new Vector2(conPos[1].x + AxisTable[k][Axis.L_Horizontal].GetAxis(), conPos[1].y - AxisTable[k][Axis.L_Vertical].GetAxis());



        if (AxisTable[0][Axis.L_Horizontal].GetAxisDown() != 0)
        {
            Debug.Log("はじいた:" + AxisTable[0][Axis.L_Horizontal].GetAxisDown().ToString());
        }
        if (AxisTable[0][Axis.L_Horizontal].GetAxisUp() != 0)
        {
            Debug.Log("戻った:" + AxisTable[0][Axis.L_Horizontal].GetAxisUp().ToString());
        }
    }

    void SetButtonAxis(){
        var controllerNames = Input.GetJoystickNames();
        ButtonTable.Clear();
        for (int i = 0; i < controllerNames.Length;i++){
            switch(controllerNames[i]){
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
                    break;
            }
        }
    }
}

