using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	
	public float sensitivity;
	public Camera cam;
	
	public enum State { IDLE, LOCKING }
	public State currentState;
	private Transform target;
	private int lockCount;
	public int maxLockCount;

	// Force push controllers
	public float power;
	public float radius;
	
	void Start() {
		currentState = State.IDLE;
	}

	void Update () {
		processInput();
		checkLockOn();
	}

	// PROCESS USER INPUT
	private void processInput() {
		// Receive Input
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += new Vector3(-1 * sensitivity * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += new Vector3(sensitivity * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position += new Vector3(0, sensitivity * Time.deltaTime, 0);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position += new Vector3(0, -1 * sensitivity * Time.deltaTime, 0);
		}
	}
	
	private void reset() {
		lockCount = 0;
		currentState = State.IDLE;
		target = null;
	}

	// CHECK IF A TARGET IS LOCKABLE AND ALIVE
	private bool checkLockable(RaycastHit hit) {
		Component[] ls = hit.transform.GetComponents(typeof(Lockable));
		
		if (ls.Length > 0 && !(((Lockable)ls[0]).isDead())) {
			return true;
		}

		return false;
	}

	// RAYCAST TO SEE IF WE ARE LOCKING ON TO ANYTHING
	private void checkLockOn() {
		// initialize RaycastHits
		RaycastHit hitClose = new RaycastHit();
		RaycastHit hitFar = new RaycastHit();

		// Get vectors from camera to crosshair and beyond
		Vector3 aim = transform.position - cam.transform.position;
		Vector3 offset = Vector3.Normalize(aim) * .75f;
		Debug.DrawRay(cam.transform.position, aim, Color.yellow);
		Debug.DrawRay(transform.position, aim*2, Color.blue);
		
		// Raycast in front of and behind the crosshair
		bool c = Physics.Raycast(cam.transform.position, aim, out hitClose, 100);
		bool f = Physics.Raycast (transform.position, aim*2, out hitFar, 100);

		// Lock on to a close target
		if (c && checkLockable(hitClose)) {
			transform.position = hitClose.point - offset;
			updateState (hitClose);
		}

		// Or else lock on to a far target
		else if (f && checkLockable(hitFar)) {
			transform.position = hitFar.point - offset;
			updateState(hitFar);
		}

		// Or else do a reset check
		else if (currentState != State.IDLE) {
			reset ();
		}
	}

	// IF WE ARE LOCKING ON, UPDATE OUR STATE
	private void updateState(RaycastHit hit) {
		// If IDLE, set target and set state to LOCKING
		if (currentState == State.IDLE) {
			target = hit.transform;
			currentState = State.LOCKING;
		}
		
		// If LOCKING and SAME TARGET, increment count or FIRE!!
		if (currentState == State.LOCKING && hit.transform == target) {
			if (lockCount < maxLockCount) { lockCount = lockCount + 1; }
			else {
				fire(hit);
				reset();
			}
		}
		
		// If LOCKING and NEW TARGET, reset
		else if (currentState == State.LOCKING) {
			target = hit.transform;
			lockCount = 0;
		}
	}
	
	private void fire(RaycastHit hit) {
		// If we have a Rigidbody, apply an explosive force to it
		if (hit.rigidbody) {
			hit.rigidbody.AddExplosionForce(power, transform.position, radius);
		}

		// Call the target's onFire function
		Lockable l = (Lockable)hit.transform.GetComponents(typeof(Lockable))[0];
		l.onFire();
	}
}