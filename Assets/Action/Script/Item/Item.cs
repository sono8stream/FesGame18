using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Item : MonoBehaviour
{
    int colorId = -1;
    int id;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("entered");
        if (collider.gameObject.tag != "Player") return;

        /*Player player = collision.gameObject.GetComponent<Player>();
        if (colorId != -1 && colorId != player.id) return;*/

        EffectFire(collider.gameObject.GetComponent<PlayerStatus>());

        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
        Debug.Log(this);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("entered");
        if (collision.gameObject.tag != "Player") return;

        /*Player player = collision.gameObject.GetComponent<Player>();
        if (colorId != -1 && colorId != player.id) return;*/

        EffectFire(collision.gameObject.GetComponent<PlayerStatus>());

        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
        Debug.Log(this);
    }

    /// <summary>
    /// アイテム取ったときの効果
    /// </summary>
    /// <param name="player"></param>
    protected abstract void EffectFire(PlayerStatus playerStatus);
}