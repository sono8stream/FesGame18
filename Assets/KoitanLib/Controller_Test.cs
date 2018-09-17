using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoitanLib;

public class Controller_Test : MonoBehaviour
{

    public Text joystickAxisText;
    public Text joystickStateText;
    public Text joystickButtonText;
    public Text joystickConnectionOrderText;
    public GameObject cube;
    public Transform[] conSprite;
    public Vector3[] conPos;

    public string conName;

    List<Dictionary<ButtonID, KoitanButton>> ButtonTable = new List<Dictionary<ButtonID, KoitanButton>>();
    List<Dictionary<Axis, KoitanAxis>> AxisTable = new List<Dictionary<Axis, KoitanAxis>>();


    // Use this for initialization
    void Start()
    {

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
            }
        }
        joystickAxisText.text = axis_text;
        joystickButtonText.text = button_text;

        var controllerNames = Input.GetJoystickNames();
        string name_text = "";
        for (int i = 0; i < controllerNames.Length; i++)
        {
            name_text += (i + 1).ToString() + " : " + controllerNames[i] + "\n";
        }
        joystickStateText.text = name_text;

        //デバッグ
        cube.transform.position = new Vector2(Input.GetAxis("joystick 1 analog 0"), -Input.GetAxis("joystick 1 analog 1"));


        /*if (KoitanInput.GetButtonDown(ButtonID.A))
        {
            Debug.Log("Aボタンが押されました");
        }

        if (KoitanInput.GetButtonUp(ButtonID.A))
        {
            Debug.Log("Aボタンが離されました");
        }*/
        int k = 0;

        conSprite[0].localScale = KoitanInput.GetButton(ButtonID.A) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[1].localScale = KoitanInput.GetButton(ButtonID.B) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[2].localScale = KoitanInput.GetButton(ButtonID.X) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[3].localScale = KoitanInput.GetButton(ButtonID.Y) ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[8].localScale = KoitanInput.GetAxis(Axis.Cross_Horizontal) > 0 ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[7].localScale = KoitanInput.GetAxis(Axis.Cross_Horizontal) < 0 ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[6].localScale = KoitanInput.GetAxis(Axis.Cross_Vertical) > 0 ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[5].localScale = KoitanInput.GetAxis(Axis.Cross_Vertical) < 0 ? new Vector2(1.2f, 1.2f) : new Vector2(1, 1);
        conSprite[10].position = new Vector2(conPos[0].x + KoitanInput.GetAxis(Axis.R_Horizontal), conPos[0].y - KoitanInput.GetAxis(Axis.R_Vertical));
        conSprite[11].position = new Vector2(conPos[1].x + KoitanInput.GetAxis(Axis.L_Horizontal), conPos[1].y - KoitanInput.GetAxis(Axis.L_Vertical));

        if (KoitanInput.GetAxisDown(Axis.L_Horizontal) != 0)
        {
            Debug.Log("はじいた:" + KoitanInput.GetAxisDown(Axis.L_Horizontal).ToString());
        }
        if (KoitanInput.GetAxisUp(Axis.L_Horizontal) != 0)
        {
            Debug.Log("戻った:" + KoitanInput.GetAxisUp(Axis.L_Horizontal).ToString());
        }

    }
}