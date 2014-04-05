using UnityEngine;
using System.Collections;

public class GameStateController : MonoBehaviour {
	// Set these in the inspector
	public Siege siege;
	public Gate gate;

	// Define the GameStateController
	public enum State {MENU, GROUND, GATE};

	// Private state
	private State currentState;
	
	void Start () {
		currentState = State.MENU;
	}

	void Update () {
		if (currentState == State.MENU && Input.GetKeyDown(KeyCode.S)) {
			siege.send();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			gate.open();
		}
	}	
}
