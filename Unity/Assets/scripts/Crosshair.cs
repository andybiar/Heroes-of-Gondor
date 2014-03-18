using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	
	public float sensitivity;
	public Camera cam;
	
	public enum State { IDLE, LOCKING }
	public State currentState;
	private Transform target;
	public int lockCount;
	public int maxLockCount;
	public GameObject crosshair;

	// Force push controllers
	public float power;
	public float radius;
	
	void Start() {
		currentState = State.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
		processInput();
		checkLockOn();
	}
	
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

	private void checkLockOn() {
		// initialize RaycastHits
		RaycastHit hitBetween = new RaycastHit();
		RaycastHit hitFar = new RaycastHit();

		// Get vectors from camera to crosshair and beyond
		Vector3 aim = transform.position - cam.transform.position;
		Vector3 offset = Vector3.Normalize(aim) * .5f;
		Debug.DrawRay(cam.transform.position, aim, Color.yellow);
		Debug.DrawRay(transform.position, aim*2, Color.blue);
		
		// Lock on to anything between crosshair and camera
		if (Physics.Raycast(cam.transform.position, aim, out hitBetween, 100) && 
		    hitBetween.transform.GetComponents(typeof(Lockable)).Length > 0) {

			transform.position = hitBetween.point - offset;
			updateState (hitBetween);
		}

		// Or else lock on beyond the crosshair
		else if (Physics.Raycast (transform.position, aim*2, out hitFar, 100) &&
		         hitFar.transform.GetComponents(typeof(Lockable)).Length > 0) {

			transform.position = hitFar.point - offset;
			updateState(hitFar);
		}

		// Or else do a reset check
		else if (currentState != State.IDLE) {
			reset ();
		}
	}
	
	private void updateState(RaycastHit hit) {
		// If IDLE, set target and set state to LOCKING
		if (currentState == State.IDLE) {
			target = hit.transform;
			currentState = State.LOCKING;
		}
		
		// If LOCKING and SAME TARGET, increment count or FIRE!!
		if (currentState == State.LOCKING && hit.transform == target) {
			if (lockCount < 100) { lockCount = lockCount + 1; }
			else {
				fire(hit);
				reset();
			}
		}
		
		// If LOCKING and NEW TARGET, reset
		else if (currentState == State.LOCKING) {
			target = hit.transform;
			Debug.Log("New Target");
			reset();
		}
	}
	
	private void fire(RaycastHit hit) {
		// If we have a Rigidbody, apply an explosive force to it
		if (hit.rigidbody) {
			hit.rigidbody.AddExplosionForce(power, transform.position, radius);
		}
	}
}