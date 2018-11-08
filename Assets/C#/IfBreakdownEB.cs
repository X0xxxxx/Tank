using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfBreakdownEB : MonoBehaviour {

    public Transform Sfront;
    public Transform Sbehind;
    /*Vector3 SDirection;//弹头方向
    LayerMask mask = (1 << 14);//部件层*/
    void Start()
    {

    }

    void Update()
    {
        /*SDirection = Sfront.position - Sbehind.position;
        Ray rays = new Ray(Sbehind.position, SDirection);
        RaycastHit hit;

        if (Physics.Raycast(rays, out hit, Mathf.Infinity, mask.value))//屏蔽层
        {
            Debug.Log(hit.collider.name);
        }*/
    }

    void OnCollisionEnter(Collision collision)
    {
        var name = collision.collider.name;
        Debug.Log("Name is " + name);

        /*撞到装甲的话，计算入射角*/
        if (collision.transform.GetComponent<ArmorAttribute>() != null)
        {
            Vector3 ShellDirection = this.GetComponent<GetDirection>().Direction;
            Vector3 ArmorDirection = collision.collider.GetComponent<GetDirection>().Direction;
            float fenzi, fenmu;
            fenzi = (ShellDirection.x * ArmorDirection.x) + (ShellDirection.y * ArmorDirection.y) + (ShellDirection.z * ArmorDirection.z);
            fenmu = (Mathf.Sqrt(Mathf.Pow(ShellDirection.x, 2) + Mathf.Pow(ShellDirection.y, 2) + Mathf.Pow(ShellDirection.z, 2))) *
                (Mathf.Sqrt(Mathf.Pow(ArmorDirection.x, 2) + Mathf.Pow(ArmorDirection.y, 2) + Mathf.Pow(ArmorDirection.z, 2)));
            float AOIcos = -(fenzi / fenmu);

            ArmorAttribute ArmorThickness = collision.collider.GetComponent<ArmorAttribute>();
            float RealThickness = ArmorThickness.thickness / Mathf.Abs(AOIcos);

            MesofEB chuangshenduqu = this.GetComponent<MesofEB>();

            float Realchuangshen = chuangshenduqu.chuangshen;

            if (Realchuangshen >= RealThickness)
            {
                //扣血,使用getcomponment使所撞车辆扣血
                //Debug.Log("Destroy");
                collision.collider.transform.GetComponentInParent<PartAttribute>().playerHP -= 300;
                //Debug.Log("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
            }
            /*else
                Debug.Log("Nothing happen");
            Debug.Log("等效" + RealThickness);*/
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        //Destroy(this.gameObject);
    }
}
