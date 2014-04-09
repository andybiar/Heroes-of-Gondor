using UnityEngine;
using System.Collections;

public class AdvanceCamera : MonoBehaviour {
	public GameStateController gameMaster;
	private bool popped;

	void OnTriggerEnter(Collider c) {
		if (!popped && c.transform.parent &&
		    c.transform.parent.transform.GetComponents(typeof(Troll_AI)).Length > 0){

			Debug.Log("Camera advanced by troll moving out of view");
			gameMaster.nextScene();
			popped = true;
		}
	}
}
