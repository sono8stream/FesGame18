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
        playerController._motor.numOfAirJumps
            = (int)Status.TempStatus[(int)StatusNames.airJumpLims];
    }

    public void Damage(SubHitBox subHitBox)
    {
        if (state != State.HUTU) return;

        state = State.MUTEKI;
        //お金を落とす
        LoseMoney();
        if (havingItem) havingItem.ReleaseReaction(this);
        Vector2 vec = subHitBox.Angle;
        vec = new Vector2(vec.x * subHitBox.hitBox.owner.anim.muki, vec.y);
        StartCoroutine(anim.Damage(vec, subHitBox.Hitlag));
        StartCoroutine(Koutyoku(subHitBox.stopTime));
        StartCoroutine(FinishMUTEKI(subHitBox.stopTime * 2));
    }

    private void LoseMoney()
    {
        float loseRatio = 0.1f;
        if (Status.money <= 0) return;

        GameObject moneyObj = Instantiate(
            coinPrefab, transform.position + Vector3.up * 2.5f, Quaternion.identity);
        Rigidbody2D tmpRb = moneyObj.AddComponent<Rigidbody2D>();
        tmpRb.AddForce(new Vector2(Random.Range(-300f, 300f), 600f));
        moneyObj.GetComponent<Money>().value = (int)(Status.money * loseRatio);
        Status.money = (int)(Status.money * (1 - loseRatio));
    }

	public IEnumerator HitStop(float time){
		anim._animator.speed = 0f;
		yield return new WaitForSeconds(time);
		anim._animator.speed = 1f;
	}

	IEnumerator Koutyoku(float time)
    {
        keyInput.isPlayable = false;
        yield return new WaitForSeconds(time);
        keyInput.isPlayable = true;
        anim._animator.CrossFade("Idle", 0.1f);
	}

    IEnumerator FinishMUTEKI(float time)
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