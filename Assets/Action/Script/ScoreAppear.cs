using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAppear : MonoBehaviour {
    List<TextMesh> meshes = new List<TextMesh>();
    Transform playerTransform;
    float deltaY;
    float offsetY = 2;

    private void Awake()
    {
        meshes.Add(GetComponent<TextMesh>());
        meshes.AddRange(GetComponentsInChildren<TextMesh>());
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        deltaY += 0.05f;
        transform.position = playerTransform.position + Vector3.up * (offsetY + deltaY);
        for (int i = 1; i < meshes.Count;i++){
            meshes[i].color -= new Color(0, 0, 0, 0.03f);
        }
        meshes[0].color -= new Color(0, 0, 0, 0.005f);
    }

    public void SetText(string text,Color color,Transform transform){
        playerTransform = transform;
        foreach(TextMesh mesh in meshes){
            mesh.text = text;
        }
        meshes[0].color = color;
    }
}
