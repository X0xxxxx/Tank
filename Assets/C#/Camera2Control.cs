using UnityEngine;
using System.Collections;

public class Camera2Control : MonoBehaviour {

	Camera camera;
	
	public float rotSpeed = 20f;
	
	public float rollSpeed = 20f;
	private float maxRoll = 70f * Mathf.PI * 2 / 360;
	private float minRoll = -10f * Mathf.PI * 2 / 360;

	public float zoomSpeed = 10f;
	private float maxDistance = 70f;
	private float minDistance = 20f;
    float xRotateAngle;
    float yRotateAngle;
    // Use this for initialization
    void Start () {
		camera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
        float y = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        float x = Input.GetAxis("Mouse Y") * rollSpeed * Time.deltaTime;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (camera.fieldOfView > minDistance)
            {
                camera.fieldOfView -= zoomSpeed;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(camera.fieldOfView < maxDistance)
            {
                camera.fieldOfView += zoomSpeed;
            }

        }
      
        yRotateAngle += y;
        xRotateAngle -= x;
        transform.rotation = Quaternion.Euler(new Vector3(xRotateAngle, yRotateAngle, 0));

    }

}
