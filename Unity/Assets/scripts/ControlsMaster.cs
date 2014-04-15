using UnityEngine;
using System.Collections.Generic;

public class ControlsMaster : MonoBehaviour {
	public enum MOVES { LEFT, RIGHT, IN, NONE }
	public GameObject movesDisplay;
	private GameStateController gameMaster;

	public Material right, left, into;

	private MOVES nextCorrectMove;

	private List<MOVES> currentSeq;
	private float moveTime = 5;
	private float moveTimerStart;
	private int sequenceLength;
	private int sCount; // the number of correct presses in the sequence so far, always <= sequenceLength
	private bool fired;
	private bool gameStarted;
	private bool firstAfterMenu = true;
	private bool lastScene;

	public void bumpOver() {
		lastScene = true;
	}

	void Start () {
		gameMaster = (GameStateController)GameObject.FindObjectOfType<GameStateController>();
	}

	void Update () {
		processInput();

		// Run out of time
		if (nextCorrectMove != MOVES.NONE && Time.timeSinceLevelLoad - moveTimerStart >= moveTime) {
			Debug.Log("Ran out of time to do the move");
		}

		// Update your cursor
		updateCursorImage();
	}

	private void updateCursorImage() {
		if (!gameStarted) return;
		else if (firstAfterMenu) {
			movesDisplay.transform.localScale *= .6f;
			firstAfterMenu = false;
		}

		if (!lastScene)
		movesDisplay.transform.position = gameMaster.transform.position + new Vector3(.3f, .5f, 2.1f);
		else {
			movesDisplay.transform.position = gameMaster.transform.position + new Vector3(-.85f, .5f, 2.1f);
		}
		if (nextCorrectMove == MOVES.RIGHT || nextCorrectMove == MOVES.LEFT) {

			movesDisplay.transform.rotation = Quaternion.Euler(gameMaster.transform.rotation.eulerAngles + 
				new Vector3(-.5f, 260.2f, 93.3f));
		}
		else {
			movesDisplay.renderer.material = into;
			movesDisplay.transform.rotation = Quaternion.Euler (
				new Vector3(53.97f, 163.97f, 348.58f));
		}


		if (nextCorrectMove == MOVES.RIGHT) {
			movesDisplay.renderer.material = right;
			movesDisplay.renderer.enabled = true;
		}
		else if (nextCorrectMove == MOVES.LEFT) {
			movesDisplay.renderer.material = left;
			movesDisplay.renderer.enabled = true;
		}
		else if (nextCorrectMove == MOVES.IN) {
			movesDisplay.renderer.material = into;
			movesDisplay.renderer.enabled = true;
		}
		else {
			movesDisplay.renderer.enabled = false;
		}
	}

	public void hide() {
		movesDisplay.renderer.enabled = false;
	}

	public void onFire() {
		fired = true;
	}

	private void checkCompletion() {
		if (sCount == sequenceLength){
			if (!gameStarted) gameStarted = true;
			gameMaster.castNextSpell();
			sCount = 0;
		}
		else {
			nextCorrectMove = currentSeq[sCount];
		}
	}

	private void processInput() {
		if (Input.GetKeyUp(KeyCode.X)) {
			leftPressed();
		}
		else if (Input.GetKeyDown(KeyCode.A)) {
			rightPressed();
		}
		else if (fired || Input.GetKeyDown(KeyCode.J)) {
			firePressed();
		}
	}

	private void leftPressed() {
		if (nextCorrectMove == MOVES.LEFT) {
			sCount += 1;
			gameMaster.playSuccess();
			checkCompletion();
		}

	}

	private void rightPressed() {
		if (nextCorrectMove == MOVES.RIGHT) {
			sCount += 1;
			gameMaster.playSuccess();
			checkCompletion();
		}

	}

	private void firePressed() {
		if (nextCorrectMove == MOVES.IN) {
			sCount += 1;
			gameMaster.playSuccess();
			checkCompletion();
		}
		fired = false;

	}

	public void generateSequence(List<MOVES> moveList, float interval) {
		if (moveList.Count == 0) {
			Debug.Log("Cannot generate sequence from empty list");
			return;
		}

		currentSeq= moveList;
		moveTime = interval;
		nextCorrectMove = moveList[0];
		moveTimerStart = Time.timeSinceLevelLoad;
		sequenceLength = moveList.Count;
		sCount = 0;
	}
	                          
}
