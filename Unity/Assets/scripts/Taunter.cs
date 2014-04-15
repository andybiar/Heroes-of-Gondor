using UnityEngine;
using System.Collections;

public class Taunter : Orc {
	private bool started;
	private bool inPosition;
	public Transform targetPosition;
	public GameObject orcList;
	public GameStateController gameMaster;
	private int terrainHits;

	public void send() {
		currentTask = Task.ENGAGING;
	}

	public override void onLock() {
	}

	protected override void specialBehavior() {
		if (!started) {
			animation.Play("Run");
			started = true;
		}
		else if (!inPosition) {
			transform.LookAt(targetPosition);
			transform.position += transform.forward * speed * Time.deltaTime;
		}
		else if (!mySounds.isPlaying) {
			mySounds.clip = (Resources.Load<AudioClip>("Orc/taunt1"));
			mySounds.Play();
			currentTask = Task.IDLE;
			animation.Play ("Taunt");
		}
	}

	public override void onFire() {
		base.onFire();
		foreach (Component c in orcList.GetComponentsInChildren(typeof (Orc))) {
			((Orc)c).setAttackRun();
		}
		gameMaster.slamEnabled = true;
		die();
	}

	void OnTriggerEnter() {
		if (!inPosition) {
			animation.Play("RunToStand");
			inPosition = true;
		}
		// TODO: play taunt animation
		
	}

	void OnCollisionEnter(Collision c) {
		if (currentStance == Stance.DEAD && c.transform.name.Equals("Terrain")) {
			if(terrainHits == 0) terrainHits += 1;
			else gameObject.SetActive(false);
		}
	}


}
