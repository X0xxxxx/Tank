using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartAttribute : MonoBehaviour {
    /*
     原本作为模块血量后为缩减工作时间暂时直接
     转为AI整体血量
         */
    public int aiHealthy;//AI血量
    public int playerHP;

    void Start () {
        aiHealthy = 1000;
        playerHP = 1000;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(aiHealthy);
        if(aiHealthy <= 0)
        {
            Destroy(this.gameObject);
        }
        if (playerHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
