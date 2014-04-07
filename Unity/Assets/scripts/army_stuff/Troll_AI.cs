using UnityEngine;
using System.Collections;

public class Troll_AI : MonoBehaviour, Lockable {
	public enum Task { IDLE, RUNNING, ENGAGING, ATTACKING, DEAD }

	// Inspect me, bitch
	public float speed;
	
	// Private state
	private bool isAlive = true;
	private Task currentTask = Task.IDLE;
	private Transform target;
	private AudioSource mySounds;
	
	void Start () {
		mySounds = transform.GetComponent<AudioSource>();
	}

	void Update () {
		switch(currentTask) {
		case Task.RUNNING:
			charge();
			break;
		}
	
	}

	// Called when the gate opens
	public void setAttackRun() {
		currentTask = Task.RUNNING;
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Troll/attack0"));
		animation.Play("Walk");
	}

	private void charge() {
		transform.position -= Vector3.forward * speed * Time.deltaTime;
		findTarget();
	}


	private void findTarget() {
	}

	public void onLock() {

	}

	public void onFire() {
	}

	public void onArrow(){
	}

	public void onStab() {
	}

	public void onRelease() {
	}

	public bool isItAlive() {
		return isAlive;
	}
}
