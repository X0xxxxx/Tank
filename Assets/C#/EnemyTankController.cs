using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : MonoBehaviour {

    public Transform[] pathPoints;
    private Vector3[] path;
    private EnemyPatrolSimulator enemy_TrackSimulator;
    private float rushDistanceThreshold=10.0f;
    private float constantSpeedDistanceThreshold = 6.8f;
    private float slowSpeedDistanceThreshold = 3.5f;
    private Vector3 guideOffset;
    
    private int reachPoint;
    void Awake()
    {
        enemy_TrackSimulator = GetComponent<EnemyPatrolSimulator>();

        if (enemy_TrackSimulator == null )
            return;
    }
    private void Start()
    {
        path = new Vector3[pathPoints.Length * 2];
        for(int i = 0; i < path.Length; i++)
        {
            if (i < pathPoints.Length)
            {
                path[i] = pathPoints[i].position;
                path[i].y = 0;
            }
            else
            {
                path[i] = pathPoints[path.Length - i - 1].position;
                path[i].y = 0;
            }
        }
        reachPoint = 0;
    }

    private void Update()
    {
    }
    void FixedUpdate()
    {
            NormalStateAction();
    }
    private void NormalStateAction()
    {

        if (path != null&&path.Length>reachPoint)
        {
            Vector3 p =path[reachPoint];
            float distanceThreshold = 10f;
            Vector3 selfPosition = transform.position;
            selfPosition.y = 0;
            Vector3 aimPosition = p;
            Vector3 selfDirection = transform.forward;
            Vector3 aimDirection = aimPosition - selfPosition;
            float angle = Mathf.Acos(Vector3.Dot(selfDirection.normalized, aimDirection.normalized)) * Mathf.Rad2Deg;
            angle = angle < 90 ? angle : 180 - angle;
            if (Vector3.Distance(selfPosition, aimPosition) > distanceThreshold)
            {         
                    if (TurnToAim(p))
                        MoveToAim(p);
            }

            else
            {
                enemy_TrackSimulator.UpdateMove(0, 0);
                reachPoint++;
            }
        }
        else if(path != null)
        {
            reachPoint = 0;
        }
    }

private void OnGUI()
    {
        
    }
    
    private void MoveToAim(Vector3 position,float motorInputValue=1.0f)
    {
        float limitDistance = 40.0f;
        float motorInput;
        float steerInput;
        Vector3 selfPosition = transform.position;
        selfPosition.y = 0;
        Vector3 selfDirection = transform.forward;
        selfDirection.y = 0;
        Vector3 aimPosition = position;
        aimPosition.y = 0;
        Vector3 aimDirection = aimPosition - selfPosition;
        float aimDistance = Vector3.Distance(aimPosition, selfPosition);
        if (Vector3.Dot(selfDirection.normalized, aimDirection.normalized) >= 0|| Vector3.Dot(selfDirection.normalized, aimDirection.normalized) < 0&& aimDistance> limitDistance)
        {
            motorInput = motorInputValue;
        }
        else
        {
            motorInput = -motorInputValue;
        }
        if(Vector3.Cross(selfDirection.normalized, aimDirection.normalized).y > 0)
        {
            steerInput = 1f;
        }
        else if (Vector3.Cross(selfDirection.normalized, aimDirection.normalized).y < 0)
        {
            steerInput = -1f;
        }
        else
        {
            steerInput = 0;
        }
        enemy_TrackSimulator.UpdateMove(motorInput,steerInput);
    }
    private bool TurnToAim(Vector3 position)
    {
        float limitDistance = 40;
        float motorInput=0;
        float steerInput=0;
        Vector3 selfPosition = transform.position;
        selfPosition.y = 0;
        Vector3 selfDirection = transform.forward;
        selfDirection.y = 0;
        Vector3 aimPosition = position;
        aimPosition.y = 0;
        Vector3 aimDirection = aimPosition - selfPosition;
        float angle = Mathf.Acos(Vector3.Dot(selfDirection.normalized, aimDirection.normalized)) * Mathf.Rad2Deg;
        float angleThreshold = 15.0f;
        
        if(angle> 135&&angle<180&& Vector3.Distance(selfPosition, aimPosition)< limitDistance)
        {
            if (Vector3.Cross(selfDirection, aimDirection).y >0)
            {
                steerInput = -1f;
            }
            else if(Vector3.Cross(selfDirection, aimDirection).y < 0)
            {
                steerInput = 1f;
            }
        }
        else if (angle > angleThreshold)
        {
            if (Vector3.Cross(selfDirection, aimDirection).y > 0)
            {
                steerInput = 1f;
            }
            else if (Vector3.Cross(selfDirection, aimDirection).y < 0)
            {
                steerInput = -1f;
            }
        }
        else
        {
            steerInput = 0;
            return true;
        }
        enemy_TrackSimulator.UpdateMove(motorInput, steerInput);
        return false;
    }

}
