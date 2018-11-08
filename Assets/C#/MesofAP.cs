using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*AP信息*/
public class MesofAP : MonoBehaviour
{
    public int KOS;
    public int chuangshen;
    int speed;
    private Rigidbody rb;
    bool Vcontrol;


    void Start()
    {
        KOS = 0;//弹种编号
        chuangshen = 100;//穿深
        speed = 100;//弹速，100用于测试，1000用于实际
        rb = GetComponent<Rigidbody>();
        Vcontrol = true;//速度方向更改与否状态
    }

    void Update()
    {
        if(Vcontrol == true)
        {
            rb.velocity = speed * GameObject.FindWithTag("PlayerPaoguan").transform.GetComponent<GetDirection>().Direction;
            Vcontrol = false;
        }
        if(this.transform.position.x>2000|| this.transform.position.y > 2000|| this.transform.position.z > 2000|| this.transform.position.x < -2000|| this.transform.position.y < -2000|| this.transform.position.z < -2000)
        {
            Destroy(this.gameObject);//超出地图外销毁
        }
    }
}
