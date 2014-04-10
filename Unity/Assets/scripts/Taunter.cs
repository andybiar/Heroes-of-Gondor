using UnityEngine;
using System.Collections;

public class Taunter : Orc {
	private bool started;
	private bool inPosition;
	public Transform targetPosition;
	public GameObject orcList;
	public GameStateController gameMaster;

	public void send() {
		currentTask = Task.ENGAGING;
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
			mySounds.clip = (Resources.Load<AudioClip>("Orc/trembleBeforeOrcs"));
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
	}

	void OnTriggerEnter() {
		if (!inPosition) {
			animation.Play("RunToStand");
			inPosition = true;
		}
		// TODO: play taunt animation
		
	}


}
