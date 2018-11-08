using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiminSide : MonoBehaviour {

    public Vector3 aim3DPosition;
    public GameObject RaytoPoint;
    Vector3 aimDirection;
    Camera cameraIn;
    LayerMask mask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12);

    public Transform turret;
    private bool isLockTurret = false;
    private float turretRotSpeed = 0.5f;
    private float turretRotTarget = 0f;

    public Transform gun;
    private float turretRollTarget = 0f;
    private float maxRoll = 10f;
    private float minRoll = -4f;
    void Start () {
        mask = ~mask;
        cameraIn = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = gun.position;
        if (Input.GetMouseButtonDown(1))
        {
            isLockTurret = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isLockTurret = false;
        }

        aimDirection = RaytoPoint.transform.position - this.transform.position;
        Ray rays = new Ray(this.transform.position, aimDirection);
        RaycastHit hit;

        if (Physics.Raycast(rays, out hit, Mathf.Infinity, mask.value))//屏蔽层
        {
            Debug.Log(hit.collider.name);
        }
        if (cameraIn.enabled)
        {
            aim3DPosition = hit.point;
            //Debug.Log("Aimin Activite");
        }
        Debug.DrawLine(rays.origin, hit.point, Color.blue);

        /************************************炮台炮管内摄像机跟随*******************************/
        Quaternion angle = Quaternion.LookRotation(gun.transform.position - aim3DPosition);

        turretRotTarget = angle.eulerAngles.y;
        turretRollTarget = angle.eulerAngles.x;
        if (cameraIn.enabled)
        {
            if (!isLockTurret)
            {
                TurretRotation();
                TurretRoll();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(aim3DPosition);
        }
    }
    public void TurretRotation()
    {
        if (this == null)//changed
            return;
        if (turret == null)
            return;

        float angle = turretRotTarget - turret.eulerAngles.y;
        if (angle < 0) angle += 360;
        if (angle > turretRotSpeed && angle < 180)
            turret.Rotate(0f, -turretRotSpeed, 0f);
        else if (angle > 180 && angle < 360 - turretRotSpeed)
            turret.Rotate(0f, turretRotSpeed, 0f);
    }

    public void TurretRoll()
    {
        if (this == null)
            return;
        if (turret == null)
            return;

        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;

        worldEuler.x = turretRollTarget;
        gun.eulerAngles = worldEuler;

        Vector3 euler = gun.localEulerAngles;
        if (euler.x > 180)
            euler.x -= 360;

        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;

        gun.localEulerAngles = new Vector3(-euler.x, localEuler.y, localEuler.z);
    }
}
