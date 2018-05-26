using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandManager : MonoBehaviour
{
    public bool isStand;
    public Player owner;
    public GameObject smokeEffect;

    [SerializeField]
    GameObject[] materialImageObj;

    private GameObject subjectObj;
    private SpriteRenderer spriteRenderer;
    private Stand stand;
    private GameObject popupObj;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        stand = GetComponentInChildren<Stand>();
        stand.gameObject.SetActive(false);
        popupObj = transform.Find("popup").gameObject;
        popupObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (stand.owner == null)
        {
            isStand = false;
        }
    }

    //Collisionの方はいらない予定
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.owner = collision.GetComponent<Player>();
            owner.aroundStand = this;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().aroundStand = null;
            owner = null;
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

    public void CreateStand()
    {
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
        stand.gameObject.SetActive(true);
        stand.owner = this.owner;
        isStand = true;
        PopupMessage();
        popupObj.GetComponent<Animator>().Play("popup");
        popupObj.SetActive(true);
    }

    public void LevelUpStand()
    {
        if (stand.LevelUp(owner.Status))
        {
            Instantiate(smokeEffect, transform.position, Quaternion.identity);
            PopupMessage();
            popupObj.GetComponent<Animator>().Play("popup");
        }
    }

    void PopupMessage()
    {
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