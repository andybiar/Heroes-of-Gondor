using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	public int orcsKilled;
	public GUIText orcsKilledText;

	void Start() {
		orcsKilledText.text = "Orcs Killed: 0";
	}

	public void orcDied() {
		orcsKilled += 1;
		orcsKilledText.text = "Orcs Killed: " + orcsKilled;
	}

}
