using UnityEngine;
using System.Collections;
/*
 车外摄像机跟随与转动
     */
public class CameraFollow : MonoBehaviour {

	public float distance = 15.0f;

	public float rot = 0;

	private float roll = 30f * Mathf.PI * 2 / 360;

	public GameObject target;

	public float rotSpeed = 0.2f;

	public float rollSpeed = 0.2f;
	private float maxRoll = 80f * Mathf.PI * 2 / 360;
	private float minRoll = -5f * Mathf.PI * 2 / 360;

	public float zoomSpeed = 0.2f;
	private float maxDistance = 22f;
	private float minDistance = 4f;

	void Start () {

	}
	
	void Update () {
        
        if (target == null)
			return;
		if (Camera.main == null)
			return;

		Vector3 targetPos = target.transform.position;

		Vector3 cameraPos;
		float d = distance * Mathf.Cos (roll);
		float height = distance * Mathf.Sin (roll);
		cameraPos.x = targetPos.x + d * Mathf.Cos (rot);
		cameraPos.z = targetPos.z + d * Mathf.Sin (rot);
		cameraPos.y = targetPos.y + height;

		Camera.main.transform.position = cameraPos;
		Camera.main.transform.LookAt (target.transform);
	}

	void LateUpdate(){
		if (target == null)
			return;
		if (Camera.main == null)
			return;
		
		Rotate ();
		Roll ();
		Zoom ();
	}
	void Rotate(){
		float w = Input.GetAxis ("Mouse X") * rotSpeed;
		rot -= w;
	}



	void Roll(){
		float w = Input.GetAxis ("Mouse Y") * rollSpeed ;

		roll -= w;

		if (roll > maxRoll)
			roll = maxRoll;
		if (roll < minRoll)
			roll = minRoll;
	}

	void Zoom(){
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			if (distance > minDistance) {
				distance -= zoomSpeed;
			}
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0){
			if(distance < maxDistance){
				distance += zoomSpeed;
			}
		}
	}
}
