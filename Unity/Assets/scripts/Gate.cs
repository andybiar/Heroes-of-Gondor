using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {
	private Animation openMe;
	private AudioSource mySounds;

	// Use this for initialization
	void Start () {
		openMe = (Animation)transform.GetComponents(typeof (Animation))[0];
		mySounds = transform.GetComponent<AudioSource>();
	}
	
	public void open() {
		Object[] trolls = GameObject.FindObjectsOfType(typeof(Troll_AI));
		foreach (Object g in trolls) {
			((Troll_AI)g).setAttackRun();
		}
		openMe.Play();
	}

	public void batteringRam() {
		mySounds.PlayOneShot(Resources.Load<AudioClip>("batteringRam2"));
	}
}
