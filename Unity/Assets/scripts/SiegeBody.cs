using UnityEngine;
using System.Collections;

public class SiegeBody : MonoBehaviour {
	public Siege driver;
	public GameObject orcList;
	public GameStateController gameMaster;
	bool advancedScene;

	public bool canAdvanceScene;

	void OnTriggerEnter(Collider collider) {
		if (!advancedScene) {
			gameMaster.nextScene();
			advancedScene = true;
		}
		else {
			driver.stop();
			foreach (Component c in orcList.transform.GetComponentsInChildren(typeof(Orc))) {
				Orc orc = (Orc)c;
				orc.setAttackRun();
			}
		}
	}
}
