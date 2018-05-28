
using UnityEngine;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class PlayerController2D : MonoBehaviour
{
    private PlatformerMotor2D _motor;
    public PC2D.AnimaController anim;
	private KeyInput keyInput;
	private Player owner;       

    // Use this for initialization
    void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
        anim = GetComponent<PC2D.AnimaController>();
		keyInput = GetComponent<KeyInput>();
		owner = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
			if (Mathf.Abs(keyInput.GetAxis(PC2D.Input.HORIZONTAL)) > PC2D.Globals.INPUT_THRESHOLD)
            {
				_motor.normalizedXMovement = keyInput.GetAxis(PC2D.Input.HORIZONTAL);
            }
            else
            {
                _motor.normalizedXMovement = 0;
            }

            // Jump?
			if (keyInput.GetButtonDown(PC2D.Input.JUMP))
            {
                _motor.Jump();
            }

			_motor.jumpingHeld = keyInput.GetButton(PC2D.Input.JUMP);
            
			if (keyInput.GetAxis(PC2D.Input.VERTICAL) < -PC2D.Globals.FAST_FALL_THRESHOLD)
            {
                _motor.fallFast = true;
            }
            else
            {
                _motor.fallFast = false;
            }

		/*
			if (keyInput.GetButtonDown(PC2D.Input.DASH))
            {
                _motor.Dash();
            }
            */

		//ボタン１
		if (keyInput.GetButtonDown(PC2D.Input.ATTACK))
		{
			if (owner.aroundItem && owner.havingItem == false)
			{
				owner.aroundItem.PickUpReaction(owner);
			}
			else if (owner.havingItem)
			{
				anim._animator.Play("throw");
			}
			else if(owner.aroundStand)
			{
                if (owner.aroundStand.isStand)
                {
                    owner.aroundStand.LevelUpStand();
                }
                else
                {
                    owner.aroundStand.CreateStand();
                }
			}
			else
			{
				anim.Attack1();
			}
		}          

		if (keyInput.GetButtonDown(PC2D.Input.SUBATTACK))
        {
            anim.Attack2();
        } 
    }
}
