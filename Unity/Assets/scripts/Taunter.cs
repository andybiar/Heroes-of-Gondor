using UnityEngine;
using System.Collections;

public class Taunter : Orc {
	private bool started;
	private bool inPosition;
	public Transform targetPosition;
	public GameObject orcList;

	public void send() {
		currentTask = Task.ENGAGING;
		Debug.Log("sent");
	}

	protected override void specialBehavior() {
		if (!started) {
			animation.Play("Run");
			started = true;
		}
		else if (!inPosition) {
			Debug.Log("Taunter running to his spot");
			transform.LookAt(targetPosition);
			transform.position += transform.forward * speed * Time.deltaTime;
		}
		else if (!mySounds.isPlaying) {
			mySounds.clip = (Resources.Load<AudioClip>("Orc/trembleBeforeOrcs"));
			mySounds.Play();
			currentTask = Task.IDLE;
		}
	}

	void OnTriggerEnter() {
		inPosition = true;
		animation.Play("RunToStand");
		// TODO: play taunt animation
		foreach (Component c in orcList.GetComponentsInChildren(typeof (Orc))) {
			((Orc)c).setAttackRun();
		}
		rigidbody.freezeRotation = true;
		rigidbody.isKinematic = true;
		
	}


}
