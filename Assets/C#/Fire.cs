using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 开火、装填
 */
public class Fire : MonoBehaviour {

    public Rigidbody AP;
    public Rigidbody APCR;
    public Rigidbody HE;
    public int KOS;
    public int nextKOS;
    int a, b, c;//记录单双击


    float Reloadtime = 7f;//重新装填时间

    public GameObject Ball;

    void Start () {
        KOS = 0;
        nextKOS = 0;//默认装填AP
        a = b = c = 0;//默认按键状态
    }
    private void FixedUpdate()
    {
        if (KOS == 0)
        {
            if (Input.GetMouseButton(0) && Reloadtime <= 0)
            {
                Instantiate(AP, this.transform.position, this.transform.rotation);
                Reloadtime = 7f;//重置装填时间
                KOS = nextKOS;
            }
            Reloadtime = Reloadtime - 0.1f;
        }

        else if (KOS == 1)
        {
            if (Input.GetMouseButton(0) && Reloadtime <= 0)
            {
                Instantiate(APCR, this.transform.position, this.transform.rotation);
                Reloadtime = 7f;
                KOS = nextKOS;
            }
            Reloadtime = Reloadtime - 0.1f;
        }

        else if (KOS == 2)
        {
            if (Input.GetMouseButton(0) && Reloadtime <= 0)
            {
                Instantiate(HE, this.transform.position, this.transform.rotation);
                Reloadtime = 7f;
                KOS = nextKOS;
            }
            Reloadtime = Reloadtime - 0.1f;
        }
    }
    void Update () {

        Ray rays = new Ray(this.transform.position, Ball.transform.position);
        Debug.DrawLine(rays.origin, Ball.transform.position, Color.yellow);

        /**********************
         * 
         * 
         * Reload
         * 
         * **********************/

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            a++;
        }
        if (a == 1)
        {
            nextKOS = 0;
            b = 0;
            c = 0;
        }
        if (a == 2)
        {
            KOS = 0;
            a = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            b++;
        }
        if (b == 1)
        {
            nextKOS = 1;
            a = 0;
            c = 0;
        }
        if (b == 2)
        {
            KOS = 1;
            b = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            c++;
        }
        if (c == 1)
        {
            nextKOS = 2;
            a = 0;
            b = 0;
        }
        if (c == 2)
        {
            KOS = 2;
            c = 0;
        }
    }

}
