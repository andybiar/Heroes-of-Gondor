using UnityEngine;
using System.Collections;

public class TurnTrigger : MonoBehaviour {
	public int min;
	public int max;
	public bool excludeAllies;
	public bool excludeEnemies;

	private System.Random random;

	void Start() {
		random = new System.Random();
	}

	void OnTriggerEnter(Collider c) {
		if (!excludeEnemies) {
			if (c.transform.parent && c.transform.parent.GetComponents(typeof(Troll_AI)).Length > 0) {
				((Troll_AI)c.transform.parent.GetComponents(typeof(Troll_AI))[0]).turn(random.Next(min, max));
			}
			else if (c.transform.parent && c.transform.parent.GetComponents(typeof(Orc)).Length > 0) {
				((Orc)c.transform.parent.GetComponents(typeof (Orc))[0]).turn(random.Next(min, max));
			}
		}
		if (!excludeAllies) {
			if (c.transform.parent && c.transform.parent.GetComponents(typeof(Spearman)).Length > 0) {
				((Spearman)c.transform.parent.GetComponents(typeof(Spearman))[0]).turn(random.Next(min,max));
			}
		}
	}
}
