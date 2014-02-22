using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public State currentState;
	public float speed;
	public Vector3 target;
	public enum State { LOADING, FLYING, HIT }

	private Vector3 launchPos;

	/*void Start() {
		Debug.Log("Started");
		currentState = State.LOADING;
	}*/

	void Update () {
		switch (currentState) {
		case State.FLYING:
			fly();
			break;
		case State.HIT:
			break;
		case State.LOADING:
			break;
		}
	}

	// Doesn't work!!
	void OnCollisionEnter() {
		Debug.Log("Collided");
	}

	public void fire(Vector3 target, float speed) {
		// Arrows can only be fired from a LOADING state
		if (this.currentState != State.LOADING) {
			Debug.Log("Arrow firing from incorrect state: " + currentState);
			return;
		}

		// Set the flight variables and FLY!
		this.target = target;
		this.speed = speed;
		this.currentState = State.FLYING;
		this.launchPos = transform.position;
	}


	void fly() {
		transform.position += (target - launchPos) / 24;
	}

}