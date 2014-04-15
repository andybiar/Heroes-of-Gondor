using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour {
	private bool up;	

	// Update is called once per frame
	void Update () {
		Color c = renderer.material.color;
		if (up) {
			renderer.material.color = new Color(c.r, c.g, c.b, c.a + .042f);
			if (renderer.material.color.a >= 1) up = false;
		}
		else {
			renderer.material.color = new Color(c.r, c.g, c.b, c.a - .042f);
			if (renderer.material.color.a <= 0) up = true;
		}
	
	}
}
