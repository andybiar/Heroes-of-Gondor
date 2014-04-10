using UnityEngine;
using System.Collections;

public class Mace : MonoBehaviour {
	public bool attacking;

	void OnCollisionEnter(Collision c) {
		if (c.rigidbody && attacking) {
			Vector3 q = c.contacts[0].point;
			c.rigidbody.AddExplosionForce(1000, new Vector3(q.x, q.y - 1, q.z), 6); // power, center, r
			Component[] cs = c.transform.GetComponents(typeof(Spearman));
			if (cs.Length > 0) {
				((Spearman)cs[0]).onMace();
			}
		}
	}
}
