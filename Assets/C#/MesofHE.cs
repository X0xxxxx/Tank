using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*HE信息*/
public class MesofHE : MonoBehaviour
{
    public int KOS;
    public int chuangshen;
    int speed;
    private Rigidbody rb;
    bool Vcontrol;
    // Use this for initialization
    void Start()
    {
        KOS = 2;
        chuangshen = 50;
        speed = 80;
        rb = GetComponent<Rigidbody>();
        /*Vcontrol = true;*/
    }

    // Update is called once per frame
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
