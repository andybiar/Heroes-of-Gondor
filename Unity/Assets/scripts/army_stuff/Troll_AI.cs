using UnityEngine;
using System.Collections;

public class Troll_AI : MonoBehaviour, Lockable {

	// Private state
	private bool isAlive;
	
	void Start () {
		isAlive = true;
		animation.wrapMode = WrapMode.Loop;
		animation.Play("troll_attack");
	}

	void Update () {
	
	}

	public void onLock() {

	}

	public void onFire() {
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
}
