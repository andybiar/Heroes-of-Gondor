using UnityEngine;
using System.Collections;

public class Gandalf : MonoBehaviour {
	// Set these in the inspector
	public float spellRadius;
	public float spellPower;
	public float spellCooldown;
	public GameStateController gameMaster;

	// Private state
	private float lastSpellTime;

	void Start() {
		lastSpellTime = -spellCooldown;
	}
	
	void Update () {
	}

	public void castBigSpell() {
		Collider[] hits = Physics.OverlapSphere(transform.position, spellRadius);

		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].transform.GetComponents(typeof(Ally)).Length > 0 ||
			    (hits[i].transform.parent &&
			    hits[i].transform.parent.GetComponents(typeof(Ally)).Length > 0)) continue;

			if (hits[i].attachedRigidbody) {
				hits[i].attachedRigidbody.freezeRotation = false;
				hits[i].attachedRigidbody.AddExplosionForce(spellPower, transform.position, spellRadius);
				gameMaster.onSlam();
			}

			Component[] ls = hits[i].transform.GetComponents(typeof(Lockable));
			if (ls.Length > 0) {
				((Lockable)ls[0]).onFire();
			}
		}
	}
}
