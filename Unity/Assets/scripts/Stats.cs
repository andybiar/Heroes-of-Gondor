using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	public int orcsKilled;
	public int trollsAlive;
	private GameStateController gameMaster;

	void Start() {
		GameObject[] trolls = GameObject.FindGameObjectsWithTag("Troll");
		trollsAlive = trolls.Length;
		gameMaster = (GameStateController)GameObject.FindObjectOfType(typeof(GameStateController));
	}

	public void orcDied() {
		orcsKilled += 1;
	}

	public void trollDied() {
		trollsAlive -= 1;
		if (trollsAlive <= 0) {
			gameMaster.win();
		}
	}
}
