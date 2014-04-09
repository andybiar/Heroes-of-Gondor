using UnityEngine;
using System.Collections;

public class Troll_AI : MonoBehaviour, Lockable {
	public enum Task { IDLE, RUNNING, ENGAGING, ATTACKING, DEAD }

	// Inspect me, bitch
	public float speed;
	public Transform path0;
	public Mace myMace;
	
	// Private state
	private bool isAlive = true;
	private Task currentTask = Task.IDLE;
	private Transform target;
	private AudioSource mySounds;
	private System.Random random;
	private int pointsHit;
	
	void Start () {
		mySounds = transform.GetComponent<AudioSource>();
		random = new System.Random();
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
		findTarget();
		transform.position -= Vector3.forward * speed * Time.deltaTime;
	}

	private void attack() {
		myMace.attacking = true;
		animation.CrossFade("Strike");
		int i = random.Next();
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Troll/attack"+ (i % 3)));
	}

	private void findTarget() {
		if (pointsHit == 0) {
			transform.LookAt(path0);
			if (Vector3.Distance(transform.position, path0.position) < 2.5f) {
				attack();
				pointsHit += 1;
			}
		}
	}

	public void onLock() {

	}

	public void onFire() {
		int i = random.Next();
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Troll/hit"+(i%6)));
		animation.CrossFade("Flinch");
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
