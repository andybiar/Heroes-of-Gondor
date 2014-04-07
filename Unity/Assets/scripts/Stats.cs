using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	public int orcsKilled;

	void Start() {
	}

	public void orcDied() {
		orcsKilled += 1;
	}

}
