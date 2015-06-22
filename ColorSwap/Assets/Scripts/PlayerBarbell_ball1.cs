using UnityEngine;
using System.Collections;

public class PlayerBarbell_ball1 : MonoBehaviour {

	public ScoreKeeper sk;
	public PlayerBarbell playerBarbell;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// When this ball collides with something check what it is.
	void OnTriggerEnter2D(Collider2D col){
		// When a block collides with this block, check if colors are the same
		if(col.gameObject.name == "Block"){
			if(col.gameObject.GetComponent<Renderer>().material.color == playerBarbell.ball1_color){
				Debug.Log(gameObject.name + " and Block are same color");
				sk.SetNumLives(-1);
			}else{
				Debug.Log(gameObject.name + " and Block are different colors");
				sk.SetScore(1);
			}
			//Debug.Log("Block hit" + col.gameObject.renderer.material.color);
		}
	}
}
