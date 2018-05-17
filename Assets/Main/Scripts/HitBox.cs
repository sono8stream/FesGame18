using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(HitBitsInfo))]
public class HitBox : MonoBehaviour {
    public HitBitsInfo HitBits;//当たる対象
    public List<GameObject> HitedObjects;//当たったリスト
    public Player owner;//親の情報   
	public Attacker attacker;

	private void Awake()
	{
		HitBits = GetComponent<HitBitsInfo>();
		attacker = GetComponent<Attacker>();
		owner = attacker.player;     
	}

	void Start()
    {		
		owner = attacker.player;
    }

    // Update is called once per frame
    void FixedUpdate () {

    }
    

	public void OnHit(Collision2D collision,SubHitBox subHitBox){
		foreach (ContactPoint2D contact in collision.contacts)
        {			
			if (CheckHitBit(contact.collider.gameObject,subHitBox))
            {
				attacker.HitReaction(contact, subHitBox);            
            }
        }
	}

    //当たる対象かチェック
	private bool CheckHitBit(GameObject obj,SubHitBox subHitBox){
        //一度当たったものには当たらない
        if(HitedObjects.Contains(obj)==false){
            string s = obj.tag;
            switch (s)
            {
                //プレイヤー
                case "Player":
                    Player opponent = obj.GetComponent<Player>();
                    if(HitBits.mySelf){
                        if(opponent.PlayerID == owner.PlayerID){
                            HitedObjects.Add(obj);
                            return true;
                        }
                    }
                    if(HitBits.myFriend){
                        if(opponent.PlayerID != owner.PlayerID && opponent.teamColor == owner.teamColor){
                            HitedObjects.Add(obj);
                            return true;
                        }
                    }
                    if (HitBits.enemy)
                    {
                        if (opponent.PlayerID != owner.PlayerID && opponent.teamColor != owner.teamColor)
                        {
                            //StartCoroutine(player.HitStop(Hitlag));                           
							opponent.Damage(subHitBox);                         
                            HitedObjects.Add(obj);                   
                            return true;
                        }
                    }
                    break;
                case "Shop":
                    if (HitBits.shop)
                    {
                        HitedObjects.Add(obj);
                        return true;
                    }
                    break;
                case "Stage":
                    if (HitBits.stage)
                    {
                        HitedObjects.Add(obj);
                        return true;
                    }
                    break;
                case "Item":
                    if (HitBits.item)
                    {
                        HitedObjects.Add(obj);
                        return true;
                    }
                    break;
                default:
                    break;
            }
        }
        return false;
    }

    //アニメーション終了時に当たったものリストをリセット
    public void ResetList(){
        HitedObjects.Clear();
    }
    
}
