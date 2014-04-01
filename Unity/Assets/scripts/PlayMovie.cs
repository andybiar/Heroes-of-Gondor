using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		((MovieTexture)renderer.material.mainTexture).Play();
	}
}
