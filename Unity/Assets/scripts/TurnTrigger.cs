using UnityEngine;
using System.Collections;

public class TurnTrigger : MonoBehaviour {
	public int min;
	public int max;

	private System.Random random;

	void Start() {
		random = new System.Random();
	}

	void OnTriggerEnter(Collider c) {
		if (c.transform.parent.GetComponents(typeof(Orc)).Length > 0) {
			((Orc)c.transform.parent.GetComponents(typeof (Orc))[0]).turn(random.Next(min, max));
		}
	}
}
