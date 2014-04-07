using UnityEngine;
using System.Collections;
using BoothGame;

public class Spearman : Infantry, Ally {

	public Spearman() : base() {
	}

	protected override Transform aggroCast() {
		Debug.DrawRay(transform.position, transform.forward * aggroRange);

		// If we see an enemy within our aggro range, engage it in combat!
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (transform.position, transform.forward, out hit, aggroRange)) {
			Component[] enemies = hit.transform.GetComponents(typeof(Enemy));
			
			if (enemies.Length > 0 && ((Enemy)enemies[0]).getIsAlive()) return hit.transform;
		}

		return null;
	}

	protected override void charge() {
	}

	protected override void die() {
	}

	protected override void onFall() {
	}

	protected override void onCrash() {
	}

}
