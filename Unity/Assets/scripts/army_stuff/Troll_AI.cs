using UnityEngine;
using System.Collections;

public class Troll_AI : MonoBehaviour, Lockable {

	// Private state
	private bool isAlive;
	
	void Start () {
		isAlive = true;
	}

	void Update () {
	
	}

	public void onLock() {

	}

	public void onFire() {
	}

	public bool isItAlive() {
		return isAlive;
	}
}
