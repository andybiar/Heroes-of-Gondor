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
		public float pikeRange;
		public float speed;
		public float cooldown;
		public Material deathColor;

		// Private state
		private Vector3 guardLocation;
		private float lastActionTime;
		private bool isAlive = true;
		private float distToGround;
		protected Transform target;
		protected Health targetHealth;

		// Abstract stuff
		protected abstract Transform aggroCast();

		// Getters
		public bool getIsAlive() {
			return isAlive;
		}

		void Start() {
			distToGround = collider.bounds.extents.y;
		}

		void Update () {
			// If dead or airborne or fallen over: do nothing
			if (!isGrounded() || !isAlive || !isUpright()) {
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

		private bool isUpright() {
			return (Vector3.Dot(Vector3.up, transform.up) > .5);
		}

		private bool isGrounded() {
			RaycastHit hit = new RaycastHit();
			return Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.1f);
		}

		protected void setTarget(Transform t) {
			// Set target and timer
			lastActionTime = Time.timeSinceLevelLoad;
			target = t;
			Component[] h = t.GetComponents(typeof(Health));
			if (h.Length > 0) targetHealth = (Health)h[0];
		}

		protected void releaseTarget() {
			target = null;
			targetHealth = null;
		}

		// Stand ground, look for enemies
		private void guard() {
			Transform t = aggroCast();

			if (t) {
				setTarget(t);
				currentTask = Task.ENGAGING;
			}
		}

		// Approach an enemy in aggroRange
		private void engage() {
			// ASSERT TARGET
			if (target == null || targetHealth == null) {
				Debug.Log("Null target or health");
				currentTask = Task.IDLE;
				return;
			}


			// If the target is dead, stop
			if (!targetHealth.getIsAlive()) {
				releaseTarget();
				currentTask = Task.IDLE;
				return;
			}

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

			// If the target is dead, stop
			if (!targetHealth.getIsAlive()) {
				releaseTarget();
				currentTask = Task.IDLE;
			}
			
			// Deal damage, set timer
			((Health)enemy.GetComponents(typeof(Health))[0]).damage(strength);
			lastActionTime = Time.timeSinceLevelLoad;

			// TODO: play stabbing animation
		}

		// Run forward, seeking targets
		private void charge() {
			transform.position += transform.forward * speed * Time.deltaTime;

			Transform t = aggroCast();

			if (t) {
				setTarget(t);
				currentTask = Task.ENGAGING;
			}
		}

		// Run away!!
		private void flee() {
			// TODO: play running animation
		}

		public void die() {
			isAlive = false;
			renderer.material = deathColor;
			int neg = Random.Range(0, 2) * -1;
			transform.rigidbody.AddTorque(Vector3.right * neg);
			// TODO: play death animation
		}
	}
}
