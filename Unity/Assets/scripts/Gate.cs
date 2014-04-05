using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {
	private Animation openMe;

	// Use this for initialization
	void Start () {
		openMe = (Animation)transform.GetComponents(typeof (Animation))[0];
	}
	
	public void open() {
		openMe.Play();
	}
}
