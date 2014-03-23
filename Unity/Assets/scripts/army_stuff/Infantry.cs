using UnityEngine;
using System.Collections;

namespace BoothGame{ 

public abstract class Infantry : MonoBehaviour, Health {
	// Definitions for guard state
	public enum Task { IDLE, ENGAGING, STABBING, RUNNING }
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
	public float cooldown;
	public Material deathColor;

	// Private state
	private Vector3 guardLocation;
	private float lastActionTime;
	private bool isAlive = true;
	private float distToGround;
	private Transform target;

	// Abstract stuff
	public abstract void onLock();
	public abstract void onFire();
	public abstract bool isItAlive();
	protected abstract bool aggroCast(RaycastHit hit);

	void Start() {
		distToGround = collider.bounds.extents.y;
	}

	void Update () {
		// If dead or airborne: do nothing
		if (!isGrounded() || !isAlive) {
			return;
		}

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
			switch(currentTask) {
			case Task.RUNNING:
				charge();
				break;
			case Task.ENGAGING:
				engage();
				break;
			case Task.STABBING:
				stab(target);
				break;
			}
		}
	}

	public void damage(float d) {
		health = health - d;
		if (health <= 0) die();
	}

	public bool getIsAlive() {
		return isAlive;
	}

	private bool isGrounded() {
		RaycastHit hit = new RaycastHit();
		return Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.1f);
	}

	protected void setTarget(Transform target) {
		// Set target and timer
		lastActionTime = Time.timeSinceLevelLoad;
		this.target = target;
	}

	// Stand ground, look for enemies
	private void guard() {
		RaycastHit hit = new RaycastHit();
		aggroCast(hit);
	}

	// Approach an enemy in aggroRange
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

	// Attack the target
	private void stab(Transform enemy) {
		Debug.DrawLine(transform.position, enemy.position, Color.red);

		// Deal damage, set timer
		((Health)enemy.GetComponents(typeof(Health))[0]).damage(strength);
		lastActionTime = Time.timeSinceLevelLoad;

		// TODO: play stabbing animation
	}

	// Run forward, seeking targets
	private void charge() {
		transform.position += transform.forward * speed * Time.deltaTime;
		RaycastHit hit = new RaycastHit();

		if (aggroCast(hit)) {
			setTarget(hit.transform);
			currentTask = Task.ENGAGING;
		}
	}

	// Run away!!
	private void flee() {
		Vector3 fleeDir = Vector3.Normalize(doorMarker.transform.position - transform.position);
		transform.position -= fleeDir * speed * Time.deltaTime;
		// TODO: play running animation
	}

	public void die() {
		isAlive = false;
		renderer.material = deathColor;
		// TODO: play death animation
	}
}

}
