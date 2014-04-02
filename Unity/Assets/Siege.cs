using UnityEngine;
using System.Collections;

public class Siege : MonoBehaviour {
	private bool active;
	public GameObject target;
	public float speed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) active = true;
		if (active) {
			transform.LookAt(target.transform);
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}

	public void activate() {
		active = true;
	}
}
