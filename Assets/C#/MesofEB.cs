using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesofEB : MonoBehaviour {
    /*敌军炮弹参数，挂在敌军炮管上*/
    public int chuangshen;
    int speed;
    private Rigidbody rb;
    bool Vcontrol;

    void Start()
    {
        chuangshen = 100;//穿深
        speed = 10;//弹速，10用于测试，1000用于实际
        rb = GetComponent<Rigidbody>();
        Vcontrol = true;//速度方向更改与否状态
    }

    void Update()
    {
        /*if (Vcontrol == true)
        {
            rb.velocity = speed * GameObject.FindWithTag("EnemyPaoguan").transform.GetComponent<GetDirection>().Direction;//生成方式不同导致速度生成方式不同
            //Debug.Log(rb.velocity);
            Vcontrol = false;
        }*/
        if (this.transform.position.x > 2000 || this.transform.position.y > 2000 || this.transform.position.z > 2000 || this.transform.position.x < -2000 || this.transform.position.y < -2000 || this.transform.position.z < -2000)
        {
            Destroy(this.gameObject);//超出地图外销毁
        }
    }
}
