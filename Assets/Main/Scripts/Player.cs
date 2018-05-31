using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class Player : Reactor {
    public float HP;
    public int PlayerID;
    public enum State
    {
        HUTU,
        MUTEKI
    }
    public State state;
    public enum TeamColor{
        RED,
        BLUE,
        YELLOW,
        GREEN
    }
    public TeamColor teamColor;

	public PC2D.AnimaController anim;
	public PlayerController2D playerController;
	public Weapon havingWeapon;
	public PickableItem havingItem;
	public PickableItem aroundItem;
	public StandManager aroundStand;
	public bool isThrowable;
	public InstantiateMissile instantiateMissile;
	public Transform handPos;

	private float stopTime;

	public KeyInput keyInput;
    public PlayerStatus Status { get; private set; }

	// Use this for initialization
	void Start () {
		anim = GetComponent<PC2D.AnimaController>();
		playerController = GetComponent<PlayerController2D>();
		keyInput = GetComponent<KeyInput>();
		keyInput.isPlayable = false;
        Debug.Log(keyInput.isPlayable);
        Status = GetComponent<PlayerStatus>();
	}
	
	// Update is called once per frame
	void Update () { 
		if (stopTime >= 0)
		{
			keyInput.isPlayable = false;
			stopTime -= Time.deltaTime;           
			//anim._animator.Play("Idle");
		}
		else{
			keyInput.isPlayable = true;
			//anim._animator.CrossFade("Idle", 0.05f);
		}        
	}

	public void Damage(SubHitBox subHitBox){
		Vector2 vec = subHitBox.Angle;
		vec = new Vector2(vec.x * subHitBox.hitBox.owner.anim.muki, vec.y);
		StartCoroutine(anim.Damage(vec, subHitBox.Hitlag));
		Koutyoku(subHitBox.stopTime);
	}

	public IEnumerator HitStop(float time){
		anim._animator.speed = 0f;
		yield return new WaitForSeconds(time);
		anim._animator.speed = 1f;
	}

	public void Koutyoku(float time){
		stopTime = time;
		StartCoroutine(Kaizyo());
	}

    /*
	public void InisiateAttack(){
		GameObject tmpObj = Instantiate(instantiateMissile.bullet,instantiateMissile.transform.position,instantiateMissile.bullet.transform.rotation);
		Vector2 vec = instantiateMissile.speed;
		vec = new Vector2(vec.x * anim.muki, vec.y);
		tmpObj.GetComponent<Rigidbody2D>().velocity = vec;
		tmpObj.GetComponent<HitBox>().player = this;
	}
	*/

	public IEnumerator Kaizyo(){
		while(true){
			yield return null;
			if(stopTime<=0){
				Debug.Log("プレイIDle");
				//anim._animator.Play("Idle");
				anim._animator.CrossFade("Idle", 0.1f);
				//anim._animator.SetTrigger("koutyoku");
				break;
			}
		}
	}

	public override void DamageReaction(SubHitBox subHitBox)
	{
		throw new System.NotImplementedException();
	}
}