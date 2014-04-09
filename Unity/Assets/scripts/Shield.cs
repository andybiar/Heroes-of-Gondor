using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public Rigidbody soldier;

	void OnCollisionEnter(Collision c) {
		Debug.Log("Shield hit: " + c.transform.name);
		if (c.collider.transform.GetComponents(typeof(Mace)).Length > 0) {
			Debug.Log("Mace hit shield");
		}
	}
}
