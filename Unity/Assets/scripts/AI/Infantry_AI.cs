using UnityEngine;
using System.Collections;

public class Infantry_AI : MonoBehaviour, Health {
	// Definitions for guard state
	public enum Task { IDLE, ENGAGING, STABBING }
	public enum Stance { GUARD, ATTACK, FLEE }

	// Things to set in the inspector
	public float ENGAGE_TIMEOUT;
	public Task currentTask;
	public Stance currentStance;
	public float strength;
	public float health;
	public float aggroRange;
	public GameObject doorMarker;
	public float pikeRange;
	public float speed;
	public Infantry_AI partner;
	public float cooldown;

	// Private state
	private Vector3 guardLocation;
	private Transform target;
	private float lastActionTime;

	void Start() {
		transform.LookAt(doorMarker.transform);
		guardLocation = transform.position;
	}

	void Update () {
		// GUARD STANCE
		if (currentStance == Stance.GUARD) {
			switch(currentTask) {
			case Task.IDLE:
				guard();
				break;
			case Task.ENGAGING:
				engage();
				break;
			case Task.STABBING:
				stab(target);
				break;
			}
		}

		// FLEE STANCE
		else if (currentStance == Stance.FLEE) {
			flee();
		}

		// ATTACK STANCE
		else if (currentStance == Stance.ATTACK) {
		}
	}

	private void setTarget(Transform target) {
		// Set target, timer, and task
		currentTask = Task.ENGAGING;
		lastActionTime = Time.timeSinceLevelLoad;
		this.target = target;
	}

	// Stand ground, look for enemies
	private void guard() {
		Debug.DrawRay(transform.position, transform.forward * aggroRange);
		RaycastHit hit = new RaycastHit();

		// If we see an enemy within our aggro range, engage it in combat!
		if (Physics.Raycast (transform.position, transform.forward, out hit, aggroRange) &&
		    hit.transform.GetComponents(typeof(Enemy)).Length > 0) {
	
			setTarget(hit.transform);
			partner.engage(hit.transform);
		}
	}

	// Charge at an enemy in aggroRange
	private void engage() {
		if (target == null) return;

		// Approach the enemy
		Debug.DrawRay(transform.position, transform.forward * pikeRange, Color.yellow);
		transform.LookAt(target);
		transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

		// If our approach timed out, retreat to previous behavior
		if (Time.timeSinceLevelLoad - lastActionTime > ENGAGE_TIMEOUT) {
			currentTask = Task.IDLE;
		}

		// If we are in range and cooldown, STRIKE!
		if (Vector3.Distance(target.position, transform.position) < pikeRange &&
		    Time.timeSinceLevelLoad - lastActionTime > cooldown) {

			currentTask = Task.STABBING;
		}
	}

	// The partner's order to engage
	public void engage(Transform target) {
		if (currentTask == Task.IDLE) {
			setTarget(target);
		}
	}

	// Attack the target
	private void stab(Transform enemy) {
		Debug.DrawLine(transform.position, enemy.position, Color.red);

		// Deal damage, set timer
		((Health)enemy.GetComponents(typeof(Health))[0]).damage(strength);
		lastActionTime = Time.timeSinceLevelLoad;

		// TODO: play stabbing animation
	}

	// Receive damage
	public void damage(float d) {

	}

	// Run away!!
	private void flee() {
		Vector3 fleeDir = Vector3.Normalize(doorMarker.transform.position - transform.position);
		transform.position -= fleeDir * speed * Time.deltaTime;
		// TODO: play running animation
	}
}
