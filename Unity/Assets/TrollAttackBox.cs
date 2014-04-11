using UnityEngine;
using System.Collections;

public class TrollAttackBox : MonoBehaviour {
	private Troll_AI troll;

	void Start() {
		troll = (Troll_AI)transform.parent.GetComponent(typeof(Troll_AI));
	}

	void OnTriggerEnter(Collider c) {
		if (c.transform.GetComponent(typeof(Ally)) ||
		    (c.transform.GetComponent(typeof(Shield)))) {
			troll.attack();
		}
	}
}
