using UnityEngine;
using System.Collections;

/* THIS IS THE UGLIEST BEHEMOTH OF A CLASS */
public class GameStateController : MonoBehaviour {
	// Set these in the inspector
	public Siege siege;
	public Gate gate;
	public Crosshair crosshair;
	public AudioSource jukebox;
	public AudioClip gameMusic;
	public GameObject HoG;
	public GameObject particles;
	public ParticleEmitter dustStorm;

	private Vector3[] positions;
	private float[] fovs;
	private float[] transitionTimes;
	private bool fireEnabled = true;
	private bool moving;
	private bool onTimer;
	private bool musicFadeOut;
	private bool hogFadeOut;
	private bool playIntro = true;
	private int timerCount;
	private int timerIndex = -1;
	private Vector3 startPos;
	private Vector3 startRot;
	private float startFov;
	private int scene = 0;
	private Camera cam;
	private float timeGameStarted;
	private bool movedToWall;
	private bool openedGate;
	private bool sceneAutoPilot = true;
	private float spaceCooldown = 1.5f;
	private float lastSpaceTime;

	// TIMERS
	private int[] timers;

	void Start () {
		cam = (Camera)transform.GetComponent("Camera");

		positions = new Vector3[12];
		fovs = new float[6];
		transitionTimes = new float[5];
		timers = new int[5];

		// 0. MAIN MENU
		positions[0] = new Vector3(32.90144f, 19.59886f, 48.50086f);
		positions[1] = new Vector3(323.2827f, 329.244f, 5.450752f);
		fovs[0] = 40.6f;

		transitionTimes[0] = 200;

		// 1. FIRST TOWER
		positions[2] = new Vector3(34.81511f, 18.5997f, 42.50385f);
		positions[3] = new Vector3(350.5567f, 335.8706f, 0);
		fovs[1] = 40.6f;
		timers[0] = 0;

		transitionTimes[1] = 375;

		// 2. LOOKING AT TOWERS
		positions[4] = new Vector3(19.44117f, 15.48369f, 7.56176f);
		positions[5] = new Vector3(359.5207f, 380.41245f, 0);
		fovs[2] = 24.4f;
		timers[1] = 0;

		transitionTimes[2] = 20;

		// 3. STAFF SLAM FIGHT
		positions[6] = new Vector3(24.72953f, 2.450761f, 1.860413f);
		positions[7] = new Vector3(359.4255f, -11.9498f, 0);
		fovs[3] = 39.3f;

		transitionTimes[3] = 60;

		// 4. FIGHT AT GATE
		positions[8] = new Vector3(2.252281f, .7022796f, -3.299316f);
		positions[9] = new Vector3(352.2751f, 349.7599f, 0);
		fovs[4] = 46.3f;

		// 5. SCALE TO FINAL GATE POS
		positions[10] = new Vector3(4.582672f, -.0659391f, -3.465302f);
		positions[11] = new Vector3(372.8997f, 347.814f, 0);
		fovs[5] = 59.5f;

		transform.position = transform.parent.position + positions[0];
		transform.rotation = (Quaternion.Euler(positions[1]));
		cam.fieldOfView = fovs[0];
	}

	// UPDATE
	void FixedUpdate () {
		if (moving) updateScene();

		if (onTimer) {
			timerCount += 1;
			if (timerCount >= timers[timerIndex]) {
				onTimer = false;
				Debug.Log("Scene advanced by timer");
				nextScene();
				timerCount = 0;
			}
		}

		// BEGIN THE GAME
		if (Input.GetKeyDown (KeyCode.S)) {
			musicFadeOut = true;
			hogFadeOut = true;
			siege.send();
		}
		
		// FADE OUT THE MAIN MENU
		if (musicFadeOut) {
			jukebox.volume -= .06f;
			if (jukebox.volume <= 0) musicFadeOut = false;
		}

		if (hogFadeOut) {
			Color c = HoG.renderer.material.color;
			if (c.a <= 0) hogFadeOut = false;
			HoG.renderer.material.color = new Color(c.r, c.g, c.b, c.a - .06f);
		}

		// MOVE TO THE GATE
		if (sceneAutoPilot && !movedToWall && Time.timeSinceLevelLoad - timeGameStarted >= 34) {
			Debug.Log("Scene advanced by 'move to the wall'");
			nextScene();
			movedToWall = true;
		}

		// OPEN THE GATE
		if (sceneAutoPilot && !openedGate && Time.timeSinceLevelLoad - timeGameStarted >= 41) {
			gate.open();
			openedGate = true;
		}

		// DEBUG: ADVANCE THE SCENE
		if (Input.GetKeyDown (KeyCode.Space) && Time.timeSinceLevelLoad - lastSpaceTime > spaceCooldown) {
			lastSpaceTime = Time.timeSinceLevelLoad;

			// MAKE ALL TRANSITIONS INSTANT
			for (int i = 0; i < transitionTimes.Length; i++) {
				transitionTimes[i] = 1;
			}

			// DISABLE ALL OTHER BODIES FROM ADVANCING THE SCENE
			Object[] siegeBodies = GameObject.FindObjectsOfType(typeof(SiegeBody));
			for (int i = 0; i < siegeBodies.Length; i++) {
				((SiegeBody)siegeBodies[i]).canAdvanceScene = false;
			}

			sceneAutoPilot = false;
			Debug.Log("Scene advanced by spacebar");
			nextScene();
			jukebox.Stop();
		}
	}

	// Plays when moving
	private void updateScene() {
		if (scene == 6) Application.LoadLevel(0);

		Vector3 p = transform.parent.position;

		transform.position += (p + positions[scene * 2] - startPos) / transitionTimes[scene-1];
		transform.rotation = Quaternion.Euler( transform.rotation.eulerAngles +
			(positions[scene * 2 + 1] - startRot) / transitionTimes[scene-1]);
		cam.fieldOfView += (fovs[scene] - startFov) / (transitionTimes[scene-1]);

		// -=-=-=-=-=-ON ARRIVE-=-=-=-=-=-=-=-
		// Scenes 1 and 2 advance by timers
		if (Vector3.Distance(p + positions[scene * 2], transform.position) < .04) {
			moving = false;
			if (sceneAutoPilot && (scene == 1 || scene == 2)) {
				onTimer = true;
				timerIndex = timerIndex + 1;
			}
		}

		// Gandalf line at staff slam start
		if (scene == 3 && playIntro) {
			jukebox.PlayOneShot(Resources.Load<AudioClip>("Gandalf/enemiesApproaching"));
			playIntro = false;
		}

		// Gandalf line at gate arrival
		if (scene == 4 && playIntro) {
			jukebox.PlayOneShot(Resources.Load<AudioClip>("Gandalf/soldiersOfGondor"));
			playIntro = false;

			if (!sceneAutoPilot) gate.open();
		}

	}

	public void nextScene() {
		scene += 1;
		moving = true;
		playIntro = true;
		startPos = transform.position;
		startRot = transform.rotation.eulerAngles;
		startFov = cam.fieldOfView;

		// BEGIN THE OPENING SEQUENCE
		if (scene == 1) {
			timeGameStarted = Time.timeSinceLevelLoad;
			musicFadeOut = false;
			jukebox.clip = gameMusic;
			jukebox.volume = 1;
			jukebox.Play();
			particles.SetActive(false);
			dustStorm.minEmission = 0;
			dustStorm.maxEmission = 0;
		}

		updateScene();
	}
}
