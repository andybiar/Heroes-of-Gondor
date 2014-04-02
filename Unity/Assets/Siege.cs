using UnityEngine;
using System.Collections;

public class Siege : MonoBehaviour {
	private bool active;
	public GameObject target;
	public float speed;

	// Use this for initialization
	void Start () {
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			transform.LookAt(target.transform);
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}

	public void activate() {
		active = true;
	}
}
