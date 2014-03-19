using UnityEngine;
using System.Collections;

public class Troll_AI : MonoBehaviour, Lockable {
	public bool isAlive;

	// Use this for initialization
	void Start () {
		isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onLock() {

	}

	public void onFire() {
	}

	public bool isDead() {
		return !isAlive;
	}
}
