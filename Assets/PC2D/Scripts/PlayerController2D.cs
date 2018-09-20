using UnityEngine;
using KoitanLib;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class PlayerController2D : MonoBehaviour
{
    public PlatformerMotor2D _motor;
    public PC2D.AnimaController anim;
    public bool isPlayable;

    private Player owner;
    [SerializeField]
    private int orderNo;

    // Use this for initialization
    void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
        anim = GetComponent<PC2D.AnimaController>();
        owner = GetComponent<Player>();
        orderNo = owner.PlayerID;
        Debug.Log(orderNo);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayable) {
            _motor.normalizedXMovement = 0;
            return;
        }

        if (Mathf.Abs(KoitanInput.GetAxis(Axis.L_Horizontal,orderNo))
            > PC2D.Globals.INPUT_THRESHOLD)
        {
            _motor.normalizedXMovement
                = KoitanInput.GetAxis(Axis.L_Horizontal, orderNo);
        }
        else
        {
            _motor.normalizedXMovement = 0;
        }

        // Jump?
        if (KoitanInput.GetButtonDown(ButtonID.X, orderNo))
        {
            _motor.Jump();
        }

        _motor.jumpingHeld = KoitanInput.GetButton(ButtonID.X, orderNo);

        if (KoitanInput.GetAxis(Axis.L_Vertical, orderNo)
            > PC2D.Globals.FAST_FALL_THRESHOLD)
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
        if (KoitanInput.GetButtonDown(ButtonID.A, orderNo))
        {
            if (owner.aroundItem && owner.havingItem == false)
            {
                owner.aroundItem.PickUpReaction(owner);
            }
            else if (owner.havingItem)
            {
                anim._animator.Play("throw");
            }
            else if (owner.aroundStand)
            {
                if (owner.aroundStand.isStand)
                {
                    owner.aroundStand.LevelUpStand();
                }
                else
                {
                    owner.aroundStand.CreateStand(owner);
                }
            }
            else
            {
                anim.Attack1();
                StartCoroutine(owner.Koutyoku(0.6f));
            }
        }

        if (KoitanInput.GetButtonDown(ButtonID.B, orderNo))
        {
            anim.Attack2();
            StartCoroutine(owner.Koutyoku(0.5f));
        }
    }
}