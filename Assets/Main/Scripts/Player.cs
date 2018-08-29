using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class Player : Reactor {
    public float HP;
    public int PlayerID;
    public State state;
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
	public GameObject coinPrefab;
    public Counter disableCounter;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<PC2D.AnimaController>();
		playerController = GetComponent<PlayerController2D>();
		keyInput = GetComponent<KeyInput>();
		keyInput.isPlayable = false;
        Debug.Log(keyInput.isPlayable);
        Status = GetComponent<PlayerStatus>();
	}

    // Update is called once per frame
    void Update()
    {
        if (stopTime >= 0)
        {
            keyInput.isPlayable = false;
            stopTime -= Time.deltaTime;
        }
        else
        {
            keyInput.isPlayable = true;
        }

        playerController._motor.numOfAirJumps
            = (int)Status.TempStatus[(int)StatusNames.airJumpLims];
    }

    public void Damage(SubHitBox subHitBox)
    {
        if (state != State.HUTU) return;

        state = State.MUTEKI;
        //金を落とす
        LoseMoney(subHitBox.stopTime * 2);
        if (havingItem) havingItem.ReleaseReaction(this);
        Vector2 vec = subHitBox.Angle;
        vec = new Vector2(vec.x * subHitBox.hitBox.owner.anim.muki, vec.y);
        StartCoroutine(anim.Damage(vec, subHitBox.Hitlag));
        Koutyoku(subHitBox.stopTime);
    }

    private void LoseMoney(float time)
    {
        float loseRatio = 0.1f;
        if (Status.money <= 0) return;

        GameObject moneyObj = Instantiate(
            coinPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity);
        Rigidbody2D tmpRb = moneyObj.AddComponent<Rigidbody2D>();
        tmpRb.AddForce(new Vector2(Random.Range(-300f, 300f), 600f));
        moneyObj.GetComponent<Money>().value = (int)(Status.money * loseRatio);
        Status.money = (int)(Status.money * (1 - loseRatio));
        StartCoroutine(FinishMUTEKI(time));
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

    public IEnumerator Kaizyo()
    {
        while (true)
        {
            yield return null;
            if (stopTime <= 0)
            {
                anim._animator.CrossFade("Idle", 0.1f);
                break;
            }
        }
    }

    public IEnumerator FinishMUTEKI(float time)
    {
        yield return new WaitForSeconds(time);
        state = State.HUTU;
    }

	public override void DamageReaction(SubHitBox subHitBox)
	{
		throw new System.NotImplementedException();
	}
    
}

public enum State
{
    HUTU,
    MUTEKI
}

public enum TeamColor
{
    RED,
    BLUE,
    YELLOW,
    GREEN
}