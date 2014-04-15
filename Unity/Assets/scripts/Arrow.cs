using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public State currentState;
	public Vector3 target;
	public enum State { LOADING, FLYING, HIT }
	public float stoppingPower;

	private Vector3 launchPos;
	private float speed;

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
	
	void OnCollisionEnter(Collision c) {
		if (c.rigidbody && c.transform.GetComponents(typeof(Lockable)).Length > 0) {
			c.rigidbody.AddForce(Vector3.Normalize(c.transform.position - transform.position) * stoppingPower);
			Lockable l = (Lockable)c.transform.GetComponents(typeof(Lockable))[0];
			l.onArrow();
			currentState = State.HIT;
			transform.parent = c.transform;
			Destroy(transform.rigidbody);
			Destroy(transform.collider);
			transform.position += transform.up * .65f;
		}
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
		transform.position += Vector3.Normalize(target - launchPos) * speed * Time.deltaTime;
	}

}