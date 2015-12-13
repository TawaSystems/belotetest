using UnityEngine;
using System.Collections;

public class SplashScreenScript : MonoBehaviour {

	public Texture2D tex;
	private Rect texRect;

	// Use this for initialization
	void Start () {
		texRect = new Rect(0, 0, Screen.width, Screen.height);

		Debug.Log (tex);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		GUI.DrawTexture (texRect, tex);
	}
}
