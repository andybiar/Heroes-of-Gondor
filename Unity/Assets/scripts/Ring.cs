using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour, Lockable {
	public GameStateController gameMaster;
	public GameObject innerRing;
	public GameObject outerRing;

	void Start() {
		Color c = outerRing.renderer.material.color;
		outerRing.renderer.material.color = new Color(c.r, c.g, c.b, 0);
	}

	public void onLock() {
		Color c = outerRing.renderer.material.color;
		outerRing.renderer.material.color = new Color(c.r, c.g, c.b, 1);
	}

	public void onRelease() {
	}

	public void onFire() {
		gameMaster.beginGame();
		gameObject.SetActive(false);
		innerRing.SetActive(false);
	}

	public void onStab() {
	}

	public void onArrow() {
	}

	public bool isItAlive() {
		return true;
	}
}
