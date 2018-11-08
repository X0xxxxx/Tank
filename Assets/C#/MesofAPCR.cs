using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*APCR信息*/
public class MesofAPCR : MonoBehaviour
{
    public int KOS;
    public int chuangshen;
    int speed;
    private Rigidbody rb;
    bool Vcontrol;

    void Start()
    {
        KOS = 1;
        chuangshen = 150;
        speed = 100;
        rb = GetComponent<Rigidbody>();
        Vcontrol = true;
    }

    void Update()
    {
        if (Vcontrol == true)
        {
            rb.velocity = speed * GameObject.FindWithTag("PlayerPaoguan").transform.GetComponent<GetDirection>().Direction;
            Vcontrol = false;
        }
        if (this.transform.position.x > 2000 || this.transform.position.y > 2000 || this.transform.position.z > 2000 || this.transform.position.x < -2000 || this.transform.position.y < -2000 || this.transform.position.z < -2000)
        {
            Destroy(this.gameObject);//超出地图外销毁
        }
    }
}
