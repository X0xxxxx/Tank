using UnityEngine;
using System.Collections;

public class ControlCamera : MonoBehaviour {

	public Camera camera1;
	public Camera camera2;
	// Use this for initialization
	void Start () {
		camera1.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			cameraSwitch();
		}
	}

	void cameraSwitch(){
		if (camera1.enabled) {
			camera1.enabled = false;
			camera2.enabled = true;
		} else {
			camera1.enabled = true;
			camera2.enabled = false;
		}
	}
}
