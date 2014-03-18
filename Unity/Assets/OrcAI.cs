using UnityEngine;
using System.Collections;

public class OrcAI : MonoBehaviour, Health, Lockable {
	private bool isAlive;

	// Use this for initialization
	void Start () {
		isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void damage(float amount) {

	}

	public void onLock() {
	}

	public void onFire() {
		isAlive = false;
	}

	public bool isDead() {
		return !isAlive;
	}
}
