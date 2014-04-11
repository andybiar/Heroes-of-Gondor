using UnityEngine;
using System.Collections;

public class Mace : MonoBehaviour {
	public bool attacking;

	void OnCollisionEnter(Collision c) {
		if (c.rigidbody && attacking) {
			Vector3 q = c.contacts[0].point;
			Component[] cs = c.transform.GetComponents(typeof(Spearman));
			if (cs.Length > 0) {
				((Spearman)cs[0]).onMace(q);
			}
		}
	}
}
