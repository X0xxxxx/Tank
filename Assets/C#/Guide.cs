using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour {
    public Transform[] path;
    [HideInInspector]
    public float[] angles;
    private int reachPoint;
    private Rigidbody guide;
    private void Awake()
    {
        guide = GetComponent<Rigidbody>();
        
    }
    // Use this for initialization
    void Start () {
        if (path == null|| guide==null) return;
        reachPoint = 0;
        angles = new float[path.Length];

    }
    public int GetReachPoint()
    {
        return reachPoint;
    }
    // Update is called once per frame
    void Update () {
		
	}
    private void FixedUpdate()
    {
        
        if (reachPoint < path.Length)
        {
            if(Vector3.Distance(transform.position, path[reachPoint].position) > 0.5)
            {
                guide.velocity = Vector3.Normalize(path[reachPoint].position - transform.position)*20;
                transform.LookAt(path[reachPoint].position);
            }
            //transform.position = Vector3.Lerp(transform.position, path[reachPoint].position, 0.02f);
            if (Vector3.Distance(transform.position, path[reachPoint].position) <= 0.2f)
            {
                float angle = Mathf.Acos(Vector3.Dot(Vector3.back, transform.forward.normalized)) * Mathf.Rad2Deg;
                Vector3 cross = Vector3.Cross(Vector3.back, transform.forward.normalized);
                if (cross.y < 0)
                {
                    angle =  - angle;
                }
                angles[reachPoint] = angle;
                
                reachPoint++;
            }
                
            if(reachPoint == path.Length)
            {
                guide.velocity=Vector3.zero;

            }
        }
    }
}
