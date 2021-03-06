﻿using UnityEngine;
using System.Collections;
using UnityChan;

namespace PC2D
{
    /// <summary>
    /// This is a very very very simple example of how an animation system could query information from the motor to set state.
    /// This can be done to explicitly play states, as is below, or send triggers, float, or bools to the animator. Most likely this
    /// will need to be written to suit your game's needs.
    /// </summary>

    public class AnimaController : MonoBehaviour
    {
        public float jumpRotationSpeed;
        public GameObject visualChild;

        private PlatformerMotor2D _motor;
		public Animator _animator;
        private bool _isJumping;
        private bool _currentFacingLeft;
        private Vector3 defaultScale;
		public int muki = 1;
		public GameObject jumpEffectPrefab;
		public GameObject landingEffectPrefab;
		public Transform asimoto;
        private SpringManager[] springManagers;

		PlatformerMotor2D.MotorState state = PlatformerMotor2D.MotorState.Jumping;

        // Use this for initialization
        void Start()
        {
            _motor = GetComponent<PlatformerMotor2D>();
            _animator = visualChild.GetComponent<Animator>();
            _animator.Play("Idle");
            springManagers = GetComponents<SpringManager>();

            _motor.onJump += SetCurrentFacingLeft;
            defaultScale = transform.localScale;

            ChangeDirection(transform.localScale.x > 0 ? 1 : -1);
        }

        // Update is called once per frame
        void Update()
        {
            if (_motor.motorState == PlatformerMotor2D.MotorState.Jumping ||
                _isJumping &&
                    (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
            {
                _isJumping = true;
                _animator.SetBool("ground", false);
				_animator.SetTrigger("jump");


                if (_motor.velocity.x <= -0.1f)
                {
                    _currentFacingLeft = true;
                }
                else if (_motor.velocity.x >= 0.1f)
                {
                    _currentFacingLeft = false;
                }

                Vector3 rotateDir = _currentFacingLeft ? Vector3.forward : Vector3.back;
                visualChild.transform.Rotate(rotateDir, jumpRotationSpeed * Time.deltaTime);
            }
            else
            {
                _isJumping = false;
                _animator.SetBool("ground", true);
                visualChild.transform.rotation = Quaternion.identity;

                if (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast)
                {
                    _animator.Play("Fall");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.WallSliding ||
                         _motor.motorState == PlatformerMotor2D.MotorState.WallSticking)
                {
                    _animator.Play("Cling");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.OnCorner)
                {
                    _animator.Play("On Corner");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping)
                {
                    _animator.Play("Slip");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Dashing)
                {
                    _animator.Play("Dash");
                }
                else
                {
                    if (_motor.velocity.sqrMagnitude >= 0.1f * 0.1f)
                    {
                        _animator.SetBool("run", true);
                    }
                    else
                    {
                        _animator.SetBool("run", false);
                    }
                }
            }

            // Facing
            float valueCheck = _motor.normalizedXMovement;

            if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping ||
                _motor.motorState == PlatformerMotor2D.MotorState.Dashing ||
                _motor.motorState == PlatformerMotor2D.MotorState.Jumping)
            {
                valueCheck = _motor.velocity.x;
            }

            if (valueCheck >= 0.1f)
            {
                ChangeDirection(1);
                    
            }
            else if (valueCheck <= -0.1f)
            {
                ChangeDirection(-1);
            }

            //ジャンプエフェクト
			JumpEffect();
			//着地エフェクト
			LandingEffect();
			state = _motor.motorState;
        }

        //攻撃
        public void Attack1()
        {
            _animator.Play("attack_pipe");
        }

		public void Attack2()
        {
            _animator.Play("attack_gun");
        }

        

        //ダメージ
		public IEnumerator Damage(Vector2 angle,float time){
			_animator.Play("damage");
			yield return new WaitForSeconds(time);
			_motor.velocity = angle;
			_animator.speed = 1.0f;
		}

        private void SetCurrentFacingLeft()
        {
            _currentFacingLeft = _motor.facingLeft;
        }

        //波紋       
		private void JumpEffect(){
			if (state != PlatformerMotor2D.MotorState.Jumping && _motor.motorState == PlatformerMotor2D.MotorState.Jumping){
				Instantiate(jumpEffectPrefab, asimoto.position, Quaternion.identity);	
			}
		}
		private void LandingEffect(){
			if(state != PlatformerMotor2D.MotorState.OnGround && _motor.motorState == PlatformerMotor2D.MotorState.OnGround){
				Instantiate(landingEffectPrefab, asimoto.position, Quaternion.identity);
			}
		}

        //向き調整
        private void ChangeDirection(int mukiValue){
            muki = mukiValue;
            foreach (SpringManager sm in springManagers)
            {
                sm.stiffnessForce = mukiValue;
            }
            Vector3 newScale = defaultScale;
            newScale.x = Mathf.Abs(defaultScale.x) * muki;
            visualChild.transform.localScale = newScale;
        }
    }
}
