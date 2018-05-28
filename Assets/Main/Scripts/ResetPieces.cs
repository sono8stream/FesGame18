using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPieces : MonoBehaviour {
	[SerializeField]
	List<Transform> transformList;
	[SerializeField]
	List<Vector3> childDefualtPos;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform)
		{
			transformList.Add(child);
			childDefualtPos.Add(child.position);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			for (int i = 0; i < transformList.Count;i++){
				transformList[i].position = childDefualtPos[i];
				transformList[i].rotation = Quaternion.identity;
				transformList[i].SetParent(this.transform);
				transformList[i].gameObject.SetActive(false);
			}
			GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
		}
		if(Input.GetMouseButtonDown(0)){		
			GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
			foreach(Transform child in transformList){
				child.SetParent(null);
				child.gameObject.SetActive(true);
			}
		}
	}
}
