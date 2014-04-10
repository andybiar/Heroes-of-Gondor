using UnityEngine;
using System.Collections;

namespace BoothGame{ 

	public abstract class Infantry : MonoBehaviour, Health {
		// Definitions for guard state
		public enum Task { IDLE, ENGAGING, STABBING, RUNNING, GUARDING }
		public enum Stance { GUARD, ATTACK, FLEE, DEAD, SPECIAL }

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
		public float maxAirTime;
		public bool canTalk;

		// Private state
		private Vector3 guardLocation;
		private float lastActionTime;
		private bool isAlive = true;
		protected Transform target;
		protected Health targetHealth;
		private float airTime;
		protected bool airDeath;
		protected Renderer myRenderer;
		protected float dissolveTime = 90;
		protected AudioSource mySounds;
		protected bool moving;
		public System.Random random;
		protected bool enteredBattle;

		// Abstract stuff
		protected abstract Transform aggroCast();
		protected abstract void charge();
		protected abstract void die();
		protected abstract void onCrash();
		protected abstract void onFall();
		protected abstract void specialBehavior();
		protected abstract void onGuardEngage();
		protected abstract void onMyStab();
				
		// Getters and Setters
		public bool getIsAlive() {
			return isAlive;
		}

		public void setIsAlive(bool t) {
			isAlive = t;
		}

		void Start() {
			mySounds = transform.GetComponent<AudioSource>();
			random = new System.Random();
		}

		void Update () {
			// If dead or fallen over: do nothing
			if (!isAlive) {
				dissolveMe();
			}

			// Falling
			if (!isGrounded()) {
				airTime += 1;
				if (airTime >= maxAirTime && !airDeath) {
					airDeath = true;
					onFall();
				}
			}
			else if (airDeath && isAlive) {
				onCrash();
				isAlive = false;
			}
			else if (!isUpright()) {
				Debug.Log("Orc not upright");
				animation.Stop();
				airTime += 1;
				if (airTime >= maxAirTime) {
					isAlive = false;
					die();
				}
			}
			else airTime = 0;

			if (currentTask == Task.IDLE) return;

			// GUARD STANCE
			if (currentStance == Stance.GUARD) {
				switch(currentTask) {
				case Task.GUARDING:
					guard();
					break;
				case Task.ENGAGING:
					guardEngage();
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

			else if (currentStance == Stance.SPECIAL) {
				specialBehavior();
			}
		}

		private bool isUpright() {
			return (Vector3.Dot(Vector3.up, transform.up) > .5);
		}
		
		private bool isGrounded() {
			return Mathf.Abs(rigidbody.velocity.y) < 2;
		}

		public void damage(float d) {
			health = health - d;
			if (health <= 0 && isAlive) {
				isAlive = false;
				Debug.Log("Infantry death by loss of health");
				die();
			}
		}

		protected void guardEngage() {
			onGuardEngage();
			// If we are in range and cooldown, STRIKE!
			if (Vector3.Distance(target.position, transform.position) < pikeRange &&
			    Time.timeSinceLevelLoad - lastActionTime > cooldown) {
				
				currentTask = Task.STABBING;
			}
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
			Debug.Log("Human stab");
			Debug.DrawLine(transform.position, enemy.position, Color.red);

			// If the target is dead, stop
			if (!targetHealth.getIsAlive()) {
				releaseTarget();
				currentTask = Task.IDLE;
			}
			
			// Deal damage, set timer
			((Health)enemy.GetComponents(typeof(Health))[0]).damage(strength);
			lastActionTime = Time.timeSinceLevelLoad;

			onMyStab();
		}

		// Run away!!
		private void flee() {
			// TODO: play running animation
		}

		protected void dissolveMe() {
			dissolveTime = dissolveTime - 1;
			if (dissolveTime <= 0) this.gameObject.SetActive(false);
		}

		void OnTriggerExit() {
			enteredBattle = true;
		}
	}
}
