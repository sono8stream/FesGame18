using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaTest : MonoBehaviour {
    public Animator anim;
    private float ground_y;
    private float vy;
    private bool ground;
    public  float g;
    public float jump_speed;

	// Use this for initialization
	void Start () {
        ground_y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        //横移動
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("run", true);
            transform.position += new Vector3(-0.2f, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("run", true);
            transform.position += new Vector3(0.2f, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else{
            anim.SetBool("run", false);
        }

        //ジャンプ
        if(Input.GetKeyDown(KeyCode.Space)){
            vy = jump_speed;
            anim.SetTrigger("jump");
        }

        vy += g;
        transform.Translate(0, vy, 0);
        if(transform.position.y<ground_y){
            transform.position = new Vector3(transform.position.x, ground_y);
            vy = 0;
            anim.SetBool("ground",true);
        }
        else{
            anim.SetBool("ground", false);
        }
	}

    public void Test(){
        
    }
}
