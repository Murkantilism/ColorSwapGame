using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	int numLives = 3; // Start with 3 lives
	
	int score = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Getter + setter for number of lives
	public int GetNumLives(){
		return numLives;
	}

	// Adds the given integer to current number of lives
	public void SetNumLives(int livesChange){
		numLives += livesChange;
	}

	// Getter + setter for score
	public int GetScore(){
		return score;
	}

	public void SetScore(int scoreChange){
		score += scoreChange;
	}
}
