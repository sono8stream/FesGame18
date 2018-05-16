
using UnityEngine;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class PlayerController2D : MonoBehaviour
{
    private PlatformerMotor2D _motor;
    public PC2D.AnimaController anim;
    private Player playerInfo;
    private int PID;
    private string sPID;
	public bool isPlayable = true;

    // Use this for initialization
    void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
        anim = GetComponent<PC2D.AnimaController>();
        playerInfo = GetComponent<Player>();
        PID = playerInfo.PlayerID;
        sPID = PID.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayable){
            if (Mathf.Abs(Input.GetAxis(PC2D.Input.HORIZONTAL + sPID)) > PC2D.Globals.INPUT_THRESHOLD)
            {
                _motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL + sPID);
            }
            else
            {
                _motor.normalizedXMovement = 0;
            }

            // Jump?
            if (Input.GetButtonDown(PC2D.Input.JUMP + sPID))
            {
                _motor.Jump();
            }

            _motor.jumpingHeld = Input.GetButton(PC2D.Input.JUMP + sPID);

            if (Input.GetAxis(PC2D.Input.VERTICAL + sPID) < -PC2D.Globals.FAST_FALL_THRESHOLD)
            {
                _motor.fallFast = true;
            }
            else
            {
                _motor.fallFast = false;
            }

            if (Input.GetButtonDown(PC2D.Input.DASH))
            {
                _motor.Dash();
            }

            //攻撃
            if (Input.GetButtonDown(PC2D.Input.ATTACK + sPID))
            {
                anim.Attack1();
            }             
			else if (Input.GetButtonDown(PC2D.Input.SUBATTACK + sPID))
            {
                anim.Attack2();
            } 
        }
		else{
			//入力キャンセル
			_motor.normalizedXMovement = 0;
		}
    }
}
