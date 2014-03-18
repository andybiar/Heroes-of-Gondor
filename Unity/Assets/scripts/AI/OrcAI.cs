using UnityEngine;
using System.Collections;

public class OrcAI : MonoBehaviour, Health, Lockable, Enemy {
	// Set these in the inspector
	public float health;
	public Material deathColor;

	// Private state
	private bool isAlive;
	
	void Start () {
		isAlive = true;
	}

	void Update () {
	
	}

	private void die() {
		isAlive = false;
		renderer.material = deathColor;
		// TODO: play death animation
	}

	public void damage(float amount) {
		health = health - amount;
		if (health <= 0) { die(); }
	}

	public void onLock() {
	}

	public void onFire() {
		isAlive = false;
	}

	public bool isDead() {
		return !isAlive;
	}
}
