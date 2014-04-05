using UnityEngine;
using System.Collections;

public class SiegeBody : MonoBehaviour {
	public Siege driver;
	public GameObject orcList;

	void OnTriggerEnter(Collider collider) {
		driver.stop();
		foreach (Component c in orcList.transform.GetComponentsInChildren(typeof(Orc))) {
			Orc orc = (Orc)c;
			orc.setAttackRun();
		}
	}
}
