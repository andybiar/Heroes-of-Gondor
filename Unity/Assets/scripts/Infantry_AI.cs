using UnityEngine;
using System.Collections;

public class Infantry_AI : MonoBehaviour {
	private float STABTIMEOUT = 3; // in seconds

	public enum Behaviors { IDLE, GUARD, ATTACK, STAB }
	public Behaviors currentBehavior;
	public float damage;
	public float health;
	public float aggroRange;
	public GameObject doorMarker;
	private Vector3 guardLocation;
	public float pikeRange;
	public float speed;

	private Transform target;
	private float lastStabTime;
	private Behaviors previousBehavior;

	void Start() {
		transform.LookAt(doorMarker.transform);
		guardLocation = transform.position;
	}

	// Update is called once per frame
	void Update () {
		switch(currentBehavior) {
		case Behaviors.GUARD:
			guard();
			break;
		case Behaviors.IDLE:
			break;
		case Behaviors.ATTACK:
			attack();
			break;
		case Behaviors.STAB:
			stab(target);
			break;
		}
	}

	private void guard() {
		Debug.DrawRay(transform.position, transform.forward * aggroRange);
		RaycastHit hit = new RaycastHit();

		// If we see an enemy within our aggro range, engage it in combat!

		if (Physics.Raycast (transform.position, transform.forward, out hit, aggroRange)) {
			// do a check to see if we hit an enemy?
			currentBehavior = Behaviors.STAB;
			previousBehavior = Behaviors.GUARD;
			lastStabTime = Time.timeSinceLevelLoad;
			target = hit.transform;
		}
	}

	private void attack() {
	}

	private void stab(Transform enemy) {

		// If our approach timed out, retreat to previous behavior
		
		if (Time.timeSinceLevelLoad - lastStabTime > STABTIMEOUT) {
			currentBehavior = previousBehavior;
			previousBehavior = Behaviors.STAB;
		}

		// If the enemy is out of poke range, approach it

		if (Vector3.Distance(transform.position, enemy.position) > pikeRange) {
			Debug.DrawLine(transform.position, enemy.position, Color.yellow);
			transform.LookAt(enemy);
			transform.position = Vector3.MoveTowards(transform.position, enemy.position, speed * Time.deltaTime);
		}

		// Else we attack!

		else {
			Debug.DrawLine(transform.position, enemy.position, Color.red);
			// play stabbing animation
			// do damage
		}
	}
}
