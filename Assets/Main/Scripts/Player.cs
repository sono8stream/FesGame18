using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Anima2D;

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
    private float finishMutekiTime = 0;
    public GameObject mesh;
    private bool meshActive = true;

    [SerializeField]
    Material material;

	private float stopTime;
    
    public PlayerStatus Status { get; private set; }
	public GameObject coinPrefab;
    public Counter disableCounter;
    private int beforeMoney;
    [SerializeField]
    GameObject moneyText;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<PC2D.AnimaController>();
		playerController = GetComponent<PlayerController2D>();
	}

    // Update is called once per frame
    void Update()
    {
        playerController._motor.numOfAirJumps
            = (int)Status.TempStatus[(int)StatusNames.airJumpLims];

        //無敵解除
        if(finishMutekiTime > 0){
            finishMutekiTime -= Time.deltaTime;
            if (meshActive == true){
                StartCoroutine(Flashing());
            }
        }
        else{
            state = State.HUTU;
        }
        int deltaMoney = Status.money - beforeMoney;
        if (deltaMoney>0){
            GameObject obj = Instantiate(moneyText, transform.position + Vector3.up*2 + Vector3.back, Quaternion.identity);
            obj.GetComponent<ScoreAppear>().SetText("+" + deltaMoney.ToString(), new Color(1, 0, 0),transform);
        }
        else if(deltaMoney<0){
            GameObject obj = Instantiate(moneyText, transform.position + Vector3.up * 2 + Vector3.back, Quaternion.identity);
            obj.GetComponent<ScoreAppear>().SetText(deltaMoney.ToString(), new Color(0, 0, 1), transform);
        }
        beforeMoney = Status.money;
    }

    public void Damage(SubHitBox subHitBox)
    {
        if (state == State.MUTEKI) return;

        state = State.MUTEKI;
        //お金を落とす
        LoseMoney();
        if (havingItem) havingItem.ReleaseReaction(this);

        Vector2 vec = subHitBox.Angle;
        vec = new Vector2(vec.x * subHitBox.hitBox.owner.anim.muki, vec.y);
        StartCoroutine(anim.Damage(vec, subHitBox.Hitlag));
        StartCoroutine(Koutyoku(subHitBox.stopTime));
        //StartCoroutine(FinishMUTEKI(subHitBox.stopTime * 1.5f));
        finishMutekiTime = subHitBox.stopTime * 1.5f;
    }

    private void LoseMoney()
    {
        float loseRatio = 0.1f;
        if (Status.money <= 0) return;

        GameObject moneyObj = Instantiate(
            coinPrefab, transform.position, Quaternion.identity);
        Rigidbody2D tmpRb = moneyObj.AddComponent<Rigidbody2D>();
        tmpRb.AddForce(new Vector2(Random.Range(-300f, 300f), 600f));
        int loseMoney = (int)(Status.money * loseRatio);
        moneyObj.GetComponent<Money>().value = loseMoney;
        moneyObj.transform.localScale
                = Vector3.one * (1 + loseMoney * 0.0001f);
        moneyObj.transform.Translate(Vector3.up * (2.5f + loseMoney * 0.00005f));
        Status.money = (int)(Status.money * (1 - loseRatio));
    }

	public IEnumerator HitStop(float time){
		anim._animator.speed = 0f;
		yield return new WaitForSeconds(time);
		anim._animator.speed = 1f;
	}

	public IEnumerator Koutyoku(float time)
    {
        playerController.isPlayable = false;
        float prevTime = Time.fixedTime;
        while (time > 0)
        {
            yield return null;
            time = ShortenRigor(time);
            time -= Time.fixedTime - prevTime;
            prevTime = Time.fixedTime;
        }
        playerController.isPlayable = true;
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

    float ShortenRigor(float remainTime)
    {
        if (Input.anyKeyDown)
        {
            remainTime -= 0.05f;
        }
        return remainTime;
    }

    IEnumerator Flashing(){
        mesh.SetActive(false);
        meshActive = false;
        yield return new WaitForSeconds(0.1f);
        mesh.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        meshActive = true;
    }

    public void SetTeam(TeamColor color, Material material,Transform statusT, int id)
    {
        Transform mesh = transform.Find("mesh");
        int borderCount = mesh.childCount - 5;
        for (int i = 0; i < borderCount; i++)
        {
            mesh.GetChild(i).GetComponent<SpriteMeshInstance>().sharedMaterial
                = material;
        }
        teamColor = color;
        PlayerID = id;
        Status = GetComponent<PlayerStatus>();
        Status.moneyText = statusT.Find("moneyText").GetComponent<Text>();
        Status.materialsTransform = statusT.Find("MaterialCounter");
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
    ORANGE,
    GREEN,
}