using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 供瞄准标识球移动
 */
public class AimBallTest : MonoBehaviour {

    public Camera Aimout;
    public Camera Aimin;
	void Start () {
		
	}
	
	void Update () {
        if (Aimout.GetComponent<Camera>().enabled)
        {
            this.transform.position = Aimout.GetComponent<AimoutSide>().aim3DPosition;
        }
        else
            this.transform.position = Aimin.GetComponent<AiminSide>().aim3DPosition;
    }
}
