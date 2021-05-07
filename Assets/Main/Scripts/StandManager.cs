using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandManager : MonoBehaviour
{
    public bool isStand;
    public bool canCreate;
    public Player owner;
    public GameObject smokeEffect;

    [SerializeField]
    GameObject[] materialImageObj;
    [SerializeField]
    Sprite[] levelSprites;
    [SerializeField]
    Explodable explodable;
    [SerializeField]
    Material[] outlineMaterials;
    [SerializeField]
    AudioClip createSE, levelupSE, resetSE;

    private GameObject subjectObj;
    private Stand stand;
    private ResetPieces resetter;
    private GameObject popupObj;
    private Transform stateTransform;
    private Counter levelCounter;
    private Animator emptyLandAnimator;

    // Use this for initialization
    void Start()
    {
        canCreate = true;
        stand = GetComponentInChildren<Stand>();
        stand.gameObject.SetActive(false);
        resetter = stand.GetComponent<ResetPieces>();
        resetter.standManager = this;
        popupObj = transform.Find("popup").gameObject;
        popupObj.SetActive(false);
        stateTransform = transform.Find("state");
        levelCounter = new Counter(levelSprites.Length);
        emptyLandAnimator = transform.Find("emptyLand").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stand.owner == null)
        {
            isStand = false;
            canCreate = true;
        }
        else if (resetter.onEndExplosion)//リセット処理
        {
            ResetLand();
        }
    }

    //Collisionの方はいらない予定
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().aroundStand = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().aroundStand = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            collision.collider.GetComponent<Player>().aroundStand = this;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            collision.collider.GetComponent<Player>().aroundStand = null;
        }
    }

    public void CreateStand(Player player)
    {
        Instantiate(smokeEffect,
            transform.position + Vector3.down, Quaternion.identity);
        stand.gameObject.SetActive(true);
        this.owner = player;
        Debug.Log(owner);
        stand.owner = player;
        stand.GetComponent<SpriteRenderer>().material
            = outlineMaterials[owner.PlayerID];
        stand.ResetGeneration();
        isStand = true;
        canCreate = false;
        PopupMessage();
        //popupObj.GetComponent<Animator>().Play("popup");
        //popupObj.SetActive(true);
        emptyLandAnimator.SetTrigger("Switch");
        SoundPlayer.Find().PlaySE(createSE);
    }

    public void LevelUpStand()
    {
        if (levelCounter.OnLimit()) return;

        if (stand.LevelUp(owner.Status))
        {
            Instantiate(smokeEffect,
                transform.position + new Vector3(0, -1f, 2f), Quaternion.identity);
            AddLevelSprite();
            if (levelCounter.OnLimit())
            {
                popupObj.SetActive(false);
            }
            else
            {
                PopupMessage();
                popupObj.GetComponent<Animator>().Play("popup");
            }
            SoundPlayer.Find().PlaySE(levelupSE);
        }
    }

    void ResetLand()
    {
        resetter.onEndExplosion = false;
        stand.ResetLevel();
        stand.owner = null;
        stand.gameObject.SetActive(false);
        resetter.ResetP();
        popupObj.SetActive(false);
        foreach(Transform child in stateTransform)
        {
            Destroy(child.gameObject);
        }
        levelCounter.Initialize();
        emptyLandAnimator.SetTrigger("Switch");
        SoundPlayer.Find().PlaySE(resetSE);
    }

    void AddLevelSprite()
    {
        if (levelCounter.OnLimit()) return;
        Transform t = new GameObject().transform;
        t.SetParent(stateTransform);
        SpriteRenderer renderer= t.gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = levelSprites[levelCounter.Now];
        t.localPosition = Vector2.down * levelCounter.Now * 2;
        t.localScale = Vector3.one;
        levelCounter.Count();
    }

    void PopupMessage()
    {
        foreach (Transform child in popupObj.transform)
        {
            Destroy(child.gameObject);
        }

        float currentX = -1.4f;
        float currentY = 0;
        float y = 0.1f;
        float xInterval = 0.35f;
        float yInterval = 0.1f;

        for (int indexI = 0;
            indexI < stand.requiredMaterialIndexes.Length; indexI++)
        {
            for (int countI = 0;
                countI < stand.requiredMaterialCounts[indexI]; countI++)
            {
                GameObject g = Instantiate(
                        materialImageObj[stand.requiredMaterialIndexes[indexI]]);
                g.transform.SetParent(popupObj.transform);
                g.transform.localPosition = new Vector2(currentX, y + currentY);
                currentX += xInterval;
                currentY += yInterval;
            }
            currentX += xInterval * 2;
            currentY = 0;
        }
    }    
    
}