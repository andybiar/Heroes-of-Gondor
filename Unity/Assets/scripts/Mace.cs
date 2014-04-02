using UnityEngine;
using System.Collections;

public class Mace : MonoBehaviour {

	void OnCollisionEnter(Collision c) {
		if (c.rigidbody) {
			Vector3 q = c.contacts[0].point;
			c.rigidbody.AddExplosionForce(1000, new Vector3(q.x, q.y - 1, q.z), 6); // power, center, r
			/*
			float max = 0;
			Vector3 knockback = new Vector3(0, 0, 0);
			for (int i = 0; i < c.contacts.Length; i++) {
				if (c.contacts[i].normal.y > max) {
					max = c.contacts[i].normal.y;
					knockback = c.contacts[i].normal;
				}
			}
			knockback = knockback * 99999;
			c.rigidbody.AddForce(Vector3.ClampMagnitude(knockback, 15000));
			*/
		}
	}
}
