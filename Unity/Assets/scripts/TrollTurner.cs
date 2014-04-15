using UnityEngine;
using System.Collections;

public class TrollTurner : MonoBehaviour {

	public Troll_AI troll;

	void OnTriggerEnter(Collider c) {
		troll.turn (-15);
	}
}
