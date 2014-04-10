using UnityEngine;
using System.Collections;

public class Troll_AI : MonoBehaviour, Lockable, Enemy, Health {
	public enum Task { IDLE, RUNNING, ENGAGING, ATTACKING, DEAD }

	// Inspect me, bitch
	public float speed;
	public Transform path0;
	public Transform path1;
	public Mace myMace;
	
	// Private state
	private bool isAlive = true;
	private Task currentTask = Task.IDLE;
	private Transform target;
	private AudioSource mySounds;
	private System.Random random;
	private int pointsHit;
	private int health = 5;
	private Stats stats;
	private float wallCheckDist = 2;
	private GameStateController gameMaster;
	private bool enteredBattle;
	private bool turning;
	private float startRotation;
	private float turnDegrees;
	
	void Start () {
		mySounds = transform.GetComponent<AudioSource>();
		random = new System.Random();
		stats = (Stats)GameObject.FindObjectOfType<Stats>();
		gameMaster = (GameStateController)GameObject.FindObjectOfType(typeof(GameStateController));
	}

	void Update () {
		if (enteredBattle && checkBoundaries()) {}
		else {
			switch(currentTask) {
			case Task.RUNNING:
				charge();
				break;
			case Task.ATTACKING:
				waitForAttackAnimation();
				break;
			}
		}
	}

	private bool checkBoundaries() {
		Debug.DrawRay(transform.position, transform.forward * wallCheckDist);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDist)) {
			if (Vector3.Distance(transform.position + transform.right, gameMaster.transform.position) <
			    Vector3.Distance(transform.position - transform.right, gameMaster.transform.position)) {
				transform.Rotate(new Vector3(0, 30, 0));
				return true;
			}
		}
		return false;
	}

	public void damage(float amount) {

	}

	private void waitForAttackAnimation() {
		if (!animation.isPlaying) currentTask = Task.RUNNING;
	}

	public bool getIsAlive() {
		return isAlive;
	}

	// Called when the gate opens
	public void setAttackRun() {
		currentTask = Task.RUNNING;
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Troll/attack0"));
		animation.Play("Walk");
	}

	private void charge() {
		findTarget();
		transform.position += transform.forward * speed * Time.deltaTime;

		if (turning == true) {
			transform.Rotate(new Vector3(0, turnDegrees/130.0f, 0));
			if (Mathf.Abs(transform.rotation.eulerAngles.y - (startRotation + turnDegrees)) < 5) {
				turning = false;
			}
		}
	}

	public void attack() {
		currentTask = Task.ATTACKING;
		myMace.attacking = true;
		animation.CrossFade("Strike");
		int i = random.Next();
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Troll/attack"+ (i % 3)));
	}

	private void findTarget() {/*
		if (pointsHit == 0) {
			transform.LookAt(path0);
			if (Vector3.Distance(transform.position, path0.position) < 2.5f) {
				attack();
				pointsHit += 1;
			}
		}
		else if (pointsHit == 1) {
			transform.LookAt(path1);
			if (Vector3.Distance(transform.position, path1.position) < 2.5f) {
				attack();
				pointsHit += 1;
			}
		}*/
	}

	public void onLock() {

	}

	public void onFire() {
		Debug.Log("TROLL hit by Gandalf");
		int i = random.Next();
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Troll/hit"+(i%6)));
		animation.CrossFade("Flinch");
		myMace.attacking = false;
		health -= 1;
		if (health <= 0) {
			die();
		}
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

	void OnTriggerExit(Collider c) {
		enteredBattle = true;
	}

	void OnTriggerEnter(Collider c) {
		Debug.Log("FUCKING TRIGGER");
	}

	private void die() {
		animation.CrossFade("Die");
		int i = random.Next();
		mySounds.PlayOneShot(Resources.Load<AudioClip>("Troll/die"+(i%2)));
		stats.trollDied();
	}

	public void turn(int degrees) {
		Debug.Log("FUCKING TURN");
		turning = true;
		startRotation = transform.rotation.eulerAngles.y;
		turnDegrees = degrees;
	}

}
