using UnityEngine;
using System.Collections;

public class LockOn : MonoBehaviour {
	public GameObject crosshair;
	private Time lockOnTimer;

	void Start() {
		lockOnTimer = new Time();
	}

	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position, (crosshair.transform.position - transform.position) * 2);
		RaycastHit hit = new RaycastHit();
		
		// If we see an enemy within our aggro range, engage it in combat!
		
		if (Physics.Raycast (transform.position, transform.forward, out hit)) {
			//Debug.Log (hit.collider);


			//target = hit.transform;
		}
	
	}
}
