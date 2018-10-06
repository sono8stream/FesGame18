using UnityEngine;
using KoitanLib;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
public class PlayerController2D : CharacterController2D{

    // Update is called once per frame
    void Update()
    {
        if (!isPlayable)
        {
            _motor.normalizedXMovement = 0;
            return;
        }

        if (Mathf.Abs(KoitanInput.GetAxis(Axis.L_Horizontal, orderNo))
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

        //ボタン１
        if (KoitanInput.GetButtonDown(ButtonID.A, orderNo))
        {
            if (owner.aroundStand && owner.aroundStand.canCreate)
            {
                owner.aroundStand.CreateStand(owner);
            }
            else if (owner.havingItem)
            {
                anim._animator.Play("throw");
            }
            else if (owner.aroundItem)
            {
                owner.aroundItem.PickUpReaction(owner);
            }
        }

        if (KoitanInput.GetButtonDown(ButtonID.Y, orderNo))
        {
            anim.Attack1();
            StartCoroutine(owner.Koutyoku(0.6f));
        }

        if (KoitanInput.GetButtonDown(ButtonID.B, orderNo))
        {
            anim.Attack2();
            StartCoroutine(owner.Koutyoku(0.5f));
        }
    }
}