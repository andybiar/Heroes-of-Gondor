using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public Spearman soldier;

	void OnCollisionEnter(Collision c) {
		if (c.collider.transform.GetComponents(typeof(Mace)).Length > 0) {
			soldier.onMace();
		}
		else if (c.collider.transform.parent.GetComponents(typeof(Spearman)).Length > 0) {}
		else {
			int i = soldier.random.Next();
			soldier.transform.audio.PlayOneShot(Resources.Load<AudioClip>("Weapons/sword"+(i%6)));
		}
	}
}
