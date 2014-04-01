using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	public int targetFrameRate;
	public float timeScale;
	
	void Start () {
		Application.targetFrameRate = targetFrameRate;
		Time.timeScale = timeScale;
	}
}
