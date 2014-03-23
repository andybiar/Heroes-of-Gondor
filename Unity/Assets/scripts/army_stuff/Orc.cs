using UnityEngine;
using System.Collections;
using BoothGame;

public class Orc : Infantry, Enemy {

	void Start() {

	}

	public override void onLock() {

	}

	// When Gandalf hit an orc with a spell, it dies
	public override void onFire() {
		die();
	}

	public override bool isItAlive() {
		return getIsAlive();
	}

	protected override bool aggroCast(RaycastHit hit) {
		// If we see an enemy within our aggro range, engage it in combat!
		if (Physics.Raycast (transform.position, transform.forward, out hit, aggroRange)) {
			Component[] allies = hit.transform.GetComponents(typeof(Ally));
			
			return allies.Length > 0 && ((Ally)allies[0]).getIsAlive();
		}
		return false;
	}
}