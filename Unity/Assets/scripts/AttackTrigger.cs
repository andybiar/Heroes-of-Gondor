using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {
	public GameObject spearmanList;
	private bool popped;

	void OnTriggerEnter(Collider c) {
		Debug.Log("attack trigger");
		if (popped) return;

		for (int i = 0; i < spearmanList.transform.childCount; i++) {
			((Spearman)spearmanList.transform.GetChild(i).GetComponent(typeof(Spearman))).playAttack();
		}
		popped = true;

	}
}
