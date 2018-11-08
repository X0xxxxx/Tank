using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedrailCollisionDetection : MonoBehaviour {


    public bool onGround;

	void Start () {
        onGround = false;
	}
	
	void Update () {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Terrain")
        {
            if (onGround == false)
            {
                onGround = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Terrain")
        {
            onGround=false;
        }
    }
}
