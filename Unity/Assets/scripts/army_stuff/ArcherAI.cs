using UnityEngine;
using System.Collections;

public class ArcherAI : MonoBehaviour {
	public GameObject arrow;
	public float arrowSpeed;
	public float cooldown;

	private float lastShotTime;

	void Start() {
		lastShotTime = -cooldown;
	}

	void Update() {
		if (Time.timeSinceLevelLoad - lastShotTime < cooldown) {
			return;
		}
		else {
			lookForTargets();
		}
	}

	private void lookForTargets() {
		Debug.DrawRay(transform.position, transform.forward * 200);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (transform.position, transform.forward, out hit, 200)) {
			Component[] enemies = hit.transform.GetComponents(typeof(Enemy));
			
			if (enemies.Length > 0 && ((Enemy)enemies[0]).getIsAlive()) {
				Component[] cs = hit.transform.GetComponentsInChildren<Collider>();
				fire(hit.transform.position + new Vector3(0, ((Collider)cs[0]).bounds.extents.y, 0));
			}
		}
	}

	private void fire(Vector3 target) {
		lastShotTime = Time.timeSinceLevelLoad;
		GameObject newArrow = (GameObject) Instantiate(arrow, transform.position, Quaternion.identity);
		newArrow.transform.rotation *= Quaternion.Euler(90, 0, 0);
		newArrow.GetComponent<Arrow>().fire(target, arrowSpeed);
	}

	public void damage(float d) {

	}
}
