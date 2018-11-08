using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedShot : MonoBehaviour {
    public Transform player;
    public Transform gun;
    public Transform gunInterface;
    public Transform gunMuzzle;
    public Transform pedestal;
    public Transform bullet;
    private Vector3 aimDirection;
    private float maxDistance=200;
    private float verticalAngle = 0;
    private float verticalAngleThreshold = 15;
    private Collider playerCollider;
    private bool horizontalRotateOver = false;
    private bool verticalRotateOver = false;
    private bool takeAim = false;
    private bool canFire = true;
    private float fillTime = 7.0f;
    private float haveFillTime = 0;

    void Start () {
        playerCollider = player.Find("collider").GetComponent<Collider>();
	}

    void FixedUpdate() {
        if (canFire == false)
        {
            haveFillTime += Time.fixedDeltaTime;
            if(haveFillTime>= fillTime)
            {
                canFire = true;
                haveFillTime = 0;
            }
        }
        aimDirection = player.position - this.transform.position;
        Ray rays = new Ray(transform.position, aimDirection);
        RaycastHit hit;
        if (Physics.Raycast(rays, out hit,maxDistance))
        {
            if (hit.collider == playerCollider)
            {
                RotateGun();
            }
            else
            {
                horizontalRotateOver = false;
                verticalRotateOver = false;
            }
        }
        if (verticalRotateOver == true && horizontalRotateOver == true)
        {
            takeAim = true;
        }
        else
        {
            takeAim = false;
        }
        if (takeAim == true)
        {
            Fire();
        }
    }
    private void RotateGun()
    {
        float rotateLevelSpeed = 0.5f;
        float rotateverticalSpeed = 0.5f;
        Vector3 pedestalDirection =new Vector3(gunInterface.position.x, 0, gunInterface.position.z)- new Vector3(pedestal.position.x,0, pedestal.position.z);
        pedestalDirection = Vector3.Normalize(pedestalDirection);
        Vector3 aimLevelDirection= new Vector3(player.position.x, 0, player.position.z) - new Vector3(pedestal.position.x, 0, pedestal.position.z);
        aimLevelDirection = Vector3.Normalize(aimLevelDirection);
        float angle = Mathf.Acos(Vector3.Dot(pedestalDirection, aimLevelDirection))*Mathf.Rad2Deg;
        if (angle > 2)
        {
            if(Vector3.Cross(pedestalDirection, aimLevelDirection).y > 0)
            {
                pedestal.Rotate(pedestal.up, rotateLevelSpeed);
                gun.RotateAround(pedestal.position, pedestal.up, rotateLevelSpeed);
                gunInterface.RotateAround(pedestal.position, pedestal.up, rotateLevelSpeed);
                gunMuzzle.RotateAround(pedestal.position, pedestal.up, rotateLevelSpeed);
            }
            else if(Vector3.Cross(pedestalDirection, aimLevelDirection).y < 0)
            {
                pedestal.Rotate(-pedestal.up, rotateLevelSpeed);
                gun.RotateAround(pedestal.position, -pedestal.up, rotateLevelSpeed);
                gunInterface.RotateAround(pedestal.position, -pedestal.up, rotateLevelSpeed);
                gunMuzzle.RotateAround(pedestal.position, -pedestal.up, rotateLevelSpeed);
            }
            horizontalRotateOver = false;
        }
        else
        {
            horizontalRotateOver = true;
        }
        if(horizontalRotateOver == true)
        {

            Vector3 gunDirection =gunMuzzle.position- gunInterface.position;
            gunDirection = Vector3.Normalize(gunDirection);
            Vector3 aimDirection = player.position - gunInterface.position; ;
            aimDirection = Vector3.Normalize(aimDirection);
            float angleV = Mathf.Acos(Vector3.Dot(gunDirection, aimDirection)) * Mathf.Rad2Deg;
            if (angleV > 2)
            {
                if(gunDirection.y < aimDirection.y && verticalAngle < verticalAngleThreshold - rotateverticalSpeed&& verticalAngle>= -verticalAngleThreshold)
                {
                    gunInterface.Rotate(-gunInterface.right, rotateverticalSpeed);
                    gun.RotateAround(gunInterface.position, -gunInterface.right, rotateverticalSpeed);
                    gunMuzzle.RotateAround(gunInterface.position, -gunInterface.right, rotateverticalSpeed);
                }
                else if (gunDirection.y > aimDirection.y&&verticalAngle>-(verticalAngleThreshold-rotateverticalSpeed)&& verticalAngle <= verticalAngleThreshold)
                {
                    gunInterface.Rotate(gunInterface.right, rotateverticalSpeed);
                    gun.RotateAround(gunInterface.position, gunInterface.right, rotateverticalSpeed);
                    gunMuzzle.RotateAround(gunInterface.position, gunInterface.right, rotateverticalSpeed);
                }
                verticalRotateOver = false;
            }
            else
            {
                verticalRotateOver = true;
            }
            if(verticalRotateOver == true)
            {
                gunDirection = gunMuzzle.position - gunInterface.position;
                gunDirection = Vector3.Normalize(gunDirection);
                aimDirection = player.position - gunInterface.position; ;
                aimDirection = Vector3.Normalize(aimDirection);
                angleV = Mathf.Acos(Vector3.Dot(gunDirection, aimDirection)) * Mathf.Rad2Deg;
                if(angleV > 2)
                {
                   
                    verticalRotateOver = false;
                }
            }
        }     
    }
    private void Fire()
    {
        if (canFire == true)
        {
            Vector3 gunDirection = gunMuzzle.position - gunInterface.position;
            gunDirection = Vector3.Normalize(gunDirection);
            Transform b= Instantiate(bullet, gunMuzzle.position,Quaternion.FromToRotation(bullet.forward, gunDirection));
            //Instantiate(bullet, this.transform.Find("gun").position, this.transform.Find("gun").rotation);
            Rigidbody br = b.GetComponent<Rigidbody>();
            br.AddForce(gunDirection*2000, ForceMode.Acceleration);
            canFire = false;
        }
    }
}
