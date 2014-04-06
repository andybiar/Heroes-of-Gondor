using UnityEngine;
using System.Collections;

public class GameStateController : MonoBehaviour {
	// Set these in the inspector
	public Siege siege;
	public Gate gate;
	public Crosshair crosshair;

	private Vector3[] positions;
	private float[] fovs;
	private float[] transitionTimes;
	private bool fireEnabled = true;
	private bool moving;
	private Vector3 startPos;
	private Vector3 startRot;
	private float startFov;
	private int scene = 0;
	private Camera cam;

	void Start () {
		cam = (Camera)transform.GetComponent("Camera");

		positions = new Vector3[10];
		fovs = new float[5];
		transitionTimes = new float[4];

		// 0. MAIN MENU
		positions[0] = new Vector3(32.90144f, 19.59886f, 48.50086f);
		positions[1] = new Vector3(323.2827f, 329.244f, 5.450752f);
		fovs[0] = 40.6f;

		transitionTimes[0] = 60;

		// 1. LOOKING AT TOWERS
		positions[2] = new Vector3(19.44117f, 15.48369f, 7.56176f);
		positions[3] = new Vector3(359.5207f, 376.41245f, 0.7492f);
		fovs[1] = 24.4f;

		transitionTimes[1] = 1;

		// 2. STAFF SLAM FIGHT
		positions[4] = new Vector3(22.76301f, 3.041213f, 6.451118f);
		positions[5] = new Vector3(363.9367f, -9.572142f, 359.2582f);
		fovs[2] = 41.7f;

		transitionTimes[2] = 2;

		// 3. FIGHT AT GATE
		positions[6] = new Vector3(3.96109f, .8914597f, -2.503426f);
		positions[7] = new Vector3(353.27f, 350.3784f, 1.119353f);
		fovs[3] = 49.6f;

		// 4. SCALE TO FINAL GATE POS
		positions[8] = new Vector3(4.34126f, -.2897515f, -2.347672f);
		positions[9] = new Vector3(370.8997f, 347.814f, .1087636f);
		fovs[4] = 60.2f;

		transform.position = transform.parent.position + positions[0];
		transform.rotation = (Quaternion.Euler(positions[1]));
		cam.fieldOfView = fovs[0];
	}

	void Update () {
		if (moving) updateScene();
		// TODO: run the scenes

		if (Input.GetKeyDown (KeyCode.S)) {
			siege.send();
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			gate.open();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			scene += 1;
			moving = true;
			startPos = transform.position;
			startRot = transform.rotation.eulerAngles;
			startFov = cam.fieldOfView;
			updateScene();
		}
	}

	// Scene was just incremented
	private void updateScene() {
		if (scene == 5) scene = 0;

		Vector3 p = transform.parent.position;

		switch (scene) {
		case 1:
			transform.position += (p + positions[scene * 2] - startPos) / transitionTimes[0];
			transform.rotation = Quaternion.Euler( transform.rotation.eulerAngles +
				(positions[scene * 2 + 1] - startRot) / transitionTimes[0]);
			cam.fieldOfView += (fovs[1] - startFov) / (transitionTimes[0]);
			if (Vector3.Distance(p + positions[scene * 2], transform.position) < .04) moving = false;
			break;
		}

		/* Insta camera switch
		transform.position = transform.parent.position + positions[scene * 2];
		transform.rotation = Quaternion.Euler(positions[scene * 2 + 1]);
		cam.fieldOfView = fovs[scene];*/
	}
}
