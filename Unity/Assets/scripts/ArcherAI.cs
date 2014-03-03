using UnityEngine;
using System.Collections;

public class ArcherAI : MonoBehaviour {
	public GameObject arrow;
	public float arrowSpeed;
	public GameObject target;

	void Update() {
		transform.LookAt(target.transform.position);
		if (Input.GetKeyDown(KeyCode.Space)) {
			fire(target.transform.position);
		}
	}

	public void fire(Vector3 target) {
		GameObject newArrow = (GameObject) Instantiate(arrow, transform.position, Quaternion.identity);
		newArrow.transform.rotation *= Quaternion.Euler(90, 0, 0);
		newArrow.GetComponent<Arrow>().fire(target, arrowSpeed);
	}
}
