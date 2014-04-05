using UnityEngine;
using System.Collections;

public class Siege : MonoBehaviour {
	private bool moving;
	public GameObject target;
	public float speed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			transform.LookAt(target.transform);
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}

	public void send() {
		moving = true;
	}

	public void stop() {
		moving = false;
	}
}
