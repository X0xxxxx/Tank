using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoFindEnemy : MonoBehaviour {

    public Transform player;
    private NavMeshAgent agent;
    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 aimPos = player.position;
        agent.SetDestination(aimPos);
	}
}
