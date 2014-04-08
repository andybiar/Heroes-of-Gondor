using UnityEngine;
using System.Collections;

public class TurnLeft : MonoBehaviour {

	void OnTriggerEnter(Collider c) {
		if (c.transform.parent.GetComponents(typeof(Orc)).Length > 0) {
			((Orc)c.transform.parent.GetComponents(typeof (Orc))[0]).turnLeft();
		}
	}
}
