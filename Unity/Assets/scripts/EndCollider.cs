using UnityEngine;
using System.Collections;

public class EndCollider : MonoBehaviour {

	void OnTriggerEnter(Collider c) {
		Debug.Log("collided");
	}

	void OnCollisionEnter(Collision c) {
		Debug.Log ("collision");
	}
}
