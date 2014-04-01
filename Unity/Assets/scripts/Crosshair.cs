using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	public bool keyboardMode;
	public float arrowKeySensitivity;
	public Camera cam;
	
	public enum State { IDLE, LOCKING }
	public State currentState;
	public int maxLockCount;
	public float breakThreshold;

	// Boundaries of the game
	public float bLeft;
	public float bRight;
	public float bUp;
	public float bDown;

	// Force push controllers
	public float power;
	public float radius;

	// Private state
	private Transform target;
	private int lockCount;
	private float x, y;

	void Start() {
		currentState = State.IDLE;
	}

	void Update () {
		//processInput();
		updatePos();
		checkLockOn();
	}

	/* Collider / trigger version is not so good
	void OnTriggerEnter(Collider c) {
		currentState = State.IDLE;
		target = c.transform;
	}

	void OnTriggerStay(Collider c) {
		lockCount = lockCount + 1;
		if (lockCount >= maxLockCount) {
			Debug.Log("fire");
			fire(c.transform);
			reset();
		}
	}

	void OnTriggerExit(Collider c) {
		reset();
	}*/

	// PROCESS USER INPUT
	private void processInput() {
		// Receive Input
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += new Vector3(-1 * arrowKeySensitivity * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += new Vector3(arrowKeySensitivity * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position += new Vector3(0, arrowKeySensitivity * Time.deltaTime, 0);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position += new Vector3(0, -1 * arrowKeySensitivity * Time.deltaTime, 0);
		}
	}

	private void updatePos() {
		// TODO: auto-aim??

		float z = transform.position.z;
		transform.position = new Vector3(((640-x) / 640.0f) * (bRight - bLeft) + bLeft, 
		                                 ((480-y) / 480.0f) * (bUp - bDown) + bDown,
		                                 z);
	}

	public void setPos(float newx, float newy) {
		x = newx;
		y = newy;
	}
	
	private void reset() {
		lockCount = 0;
		currentState = State.IDLE;
		target = null;
	}

	// CHECK IF A TARGET IS LOCKABLE AND ALIVE
	private bool checkLockable(RaycastHit hit) {
		Component[] ls = hit.transform.GetComponents(typeof(Lockable));
		
		if (ls.Length > 0 && ((Lockable)ls[0]).isItAlive()) {
			return true;
		}

		return false;
	}

	// RAYCAST TO SEE IF WE ARE LOCKING ON TO ANYTHING
	private void checkLockOn() {
		// initialize RaycastHits
		RaycastHit hitClose = new RaycastHit();
		RaycastHit hitFar = new RaycastHit();

		// Vector shit
		Vector3 aim = transform.position - cam.transform.position;
		Vector3 co = Vector3.Normalize(aim);

		Debug.DrawLine(cam.transform.position, transform.position, Color.yellow);
		Debug.DrawRay(transform.position - co, aim*2, Color.blue);

		bool c = Physics.Raycast(cam.transform.position, aim, out hitClose, 
		                         Vector3.Distance(cam.transform.position, transform.position));
		bool f = Physics.Raycast (transform.position - co, aim*2, out hitFar, 100);

		// Lock on to a close target
		if (c && checkLockable(hitClose)) {
			Vector3 offset = Vector3.Normalize(aim) * .05f;
			transform.position = hitClose.point; //- offset;
			updateState (hitClose);
		}

		// Or else lock on to a far target
		else if (f && checkLockable(hitFar)) {
			Vector3 offset = Vector3.Normalize(aim) * .05f;
			transform.position = hitFar.point; //- offset;
			updateState(hitFar);
		}

		// Or else do a reset check
		else if (currentState != State.IDLE) {
			reset ();
		}
	}

	// UPDATE LOCKON STATE
	private void updateState(RaycastHit hit) {
		// If IDLE, set target and set state to LOCKING
		if (currentState == State.IDLE) {
			target = hit.transform;
			currentState = State.LOCKING;
		}
		
		// If LOCKING and SAME TARGET, increment count or FIRE!!
		if (currentState == State.LOCKING && hit.transform == target) {
			if (lockCount < maxLockCount) { 
				lockCount = lockCount + 1;
				//transform.localScale = startScale * ((maxLockCount - lockCount) / maxLockCount);
			}
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

	private void fire(Transform t) {
		// If we have a Rigidbody, apply an explosive force to it
		if (t.rigidbody) {
			t.rigidbody.AddExplosionForce(power, transform.position, radius);
		}
		
		// Call the target's onFire function
		Lockable l = (Lockable)t.GetComponents(typeof(Lockable))[0];
		l.onFire();
	}
}