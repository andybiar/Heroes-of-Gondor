using UnityEngine;
using System.Collections;
using BoothGame;

public class Orc : Infantry, Enemy, Lockable {

	public Orc() : base() {
	}

	public void onLock() {

	}

	// When Gandalf hit an orc with a spell, it dies
	public void onFire() {
		die();
	}

	public bool isItAlive() {
		return getIsAlive();
	}

	private bool isAlly(Transform t) {
		Component[] allies = t.GetComponents(typeof(Ally));
		
		if (allies.Length > 0 && ((Ally)allies[0]).getIsAlive()) return true;
		return false;
	}
		
	protected override Transform aggroCast() {
		Debug.DrawRay(transform.position, Quaternion.Euler(0, -6, 0) * transform.forward * aggroRange);
		Debug.DrawRay(transform.position, Quaternion.Euler(0, 6, 0) * transform.forward * aggroRange);

		// Double Raycast
		RaycastHit l = new RaycastHit();
		RaycastHit r = new RaycastHit();

		bool isL = Physics.Raycast(
			transform.position, Quaternion.Euler(0,-6,0)*transform.forward, out l, aggroRange);
		bool isR = Physics.Raycast(
			transform.position, Quaternion.Euler(0,6,0)*transform.forward, out r, aggroRange);

		// Figure out what to return
		if (isL && isR) {
			if (l.distance < r.distance) {
				if (isAlly(l.transform)) return l.transform;
				else if (isAlly(r.transform)) return r.transform;
			}
			else {
				if (isAlly(r.transform)) return r.transform;
				else if (isAlly(l.transform)) return l.transform;
			}
		}
		else if (isL) {
			if (isAlly(l.transform)) return l.transform;
		}
		else if (isR) {
			if (isAlly(r.transform)) return r.transform;
		}
		return null;
	}
}