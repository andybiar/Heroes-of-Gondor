using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public float sensitivity;
	public Camera cam;

	public enum State { IDLE, LOCKING }
	public State currentState;
	private Transform target;
	public int lockCount;

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
		Debug.Log ("Reset");
		lockCount = 0;
		currentState = State.IDLE;
		target = null;
	}

	private void checkLockOn() {
		Vector3 aim = transform.position - cam.transform.position;
		RaycastHit hit = new RaycastHit();
		Debug.DrawRay(transform.position, aim*2, Color.blue);

		// If we are aiming at a Lockable, start locking on
		if (Physics.Raycast (transform.position, aim*2, out hit, 100)) {
			if (hit.transform.GetComponents(typeof(Lockable)).Length > 0) {
				// We are locking on!

				// If IDLE, set target and set state to LOCKING
				if (currentState == State.IDLE) {
					target = hit.transform;
					currentState = State.LOCKING;
				}

				// If LOCKING and SAME TARGET, increment count or FIRE!!
				if (currentState == State.LOCKING && hit.transform == target) {
					if (lockCount < 100) { lockCount += 1; }
					else {
						fire();
						reset();
					}
				}

				// If LOCKING and NEW TARGET, reset
				else if (currentState == State.LOCKING) {
					Debug.Log("New Target");
					reset();
				}
			}
			else {
				reset();
			}
		}
	}

	private void fire() {
	}
}
