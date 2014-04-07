using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {
	private Animation openMe;

	// Use this for initialization
	void Start () {
		openMe = (Animation)transform.GetComponents(typeof (Animation))[0];
	}
	
	public void open() {
		Object[] trolls = GameObject.FindObjectsOfType(typeof(Troll_AI));
		foreach (Object g in trolls) {
			((Troll_AI)g).setAttackRun();
		}
		openMe.Play();
	}
}
