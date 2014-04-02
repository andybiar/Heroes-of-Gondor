using UnityEngine;
using System.Collections;

public class movieTex : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (this.guiTexture) {
			((MovieTexture)guiTexture.texture).loop = true;
			((MovieTexture)guiTexture.texture).Play();
		}
		if (this.renderer.material.mainTexture) {
			((MovieTexture)renderer.material.mainTexture).loop = true;
			((MovieTexture)renderer.material.mainTexture).Play();
		}
	}
}
