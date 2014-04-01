using UnityEngine;
using System.Collections;

public class Gandalf : MonoBehaviour {
	// Set these in the inspector
	public float spellRadius;
	public float spellPower;
	public float spellCooldown;

	// Private state
	private float lastSpellTime;
	private bool hasCast;

	void Start() {
		lastSpellTime = -spellCooldown;
	}
	
	void Update () {
		processInput();
	}

	private void processInput() {
		if ((Input.GetKeyDown(KeyCode.A) ||
		    Input.GetKeyDown(KeyCode.B) ||
		    Input.GetKeyDown(KeyCode.X) ||
		    Input.GetKeyDown(KeyCode.Y)) &&
		    Time.timeSinceLevelLoad - lastSpellTime > spellCooldown &&
			!hasCast){

			castBigSpell();
		}
	}

	private void castBigSpell() {
		hasCast = true;
		Collider[] hits = Physics.OverlapSphere(transform.position, spellRadius);

		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].transform.GetComponents(typeof(Ally)).Length > 0) continue;

			if (hits[i].attachedRigidbody) {
				hits[i].attachedRigidbody.AddExplosionForce(spellPower, transform.position, spellRadius);
			}

			Component[] ls = hits[i].transform.GetComponents(typeof(Lockable));
			if (ls.Length > 0) {
				((Lockable)ls[0]).onFire();
			}
		}
	}
}
