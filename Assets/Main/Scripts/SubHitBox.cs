using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SubHitBox : MonoBehaviour
{
    public int ID;//優先順位、小さい方が優先
    public int GID;//優先順位補助、別番号で同時ヒット
    public float Damage;//ダメージ
    public Vector2 Angle;//ベクトルの角度
    public float Hitlag;//ヒットストップ、秒数
    public bool Clang;//相殺のあるなし
    public AudioClip HitSound;//当たったときの効果音
    public float RehitRate;//再ヒット間隔、秒数
    public float stopTime;//硬直時間、秒数
    public GameObject Effect;//当たった時のエフェクト
	public HitBox hitBox;//親

    void Start()
    {
		hitBox = transform.parent.GetComponent<HitBox>();      
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		hitBox.OnHit(collision, this);
    }
       
}
