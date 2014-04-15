using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour, Lockable {
	public GameStateController gameMaster;
	public GameObject pressText;

	void Start() {
	}

	public void onLock() {
	}

	public void onRelease() {
	}

	public void onFire() {
		gameMaster.beginGame();
		gameObject.SetActive(false);
		pressText.SetActive(false);

	}

	public void onStab() {
	}

	public void onArrow() {
	}

	public bool isItAlive() {
		return true;
	}
}
