using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//挂载在炮弹预制件上，APCR是否击穿
//Angle of incidence入射角
//Mathf.Sqrt平方根
/*Mathf.Pow次方 
static function Pow(f : float, p : float): float
计算并返回 f 的 p 次方。 */
public class IfBreakdownAPCR : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        var name = collision.collider.name;
        Debug.Log("Name is " + name);
        if (collision.transform.GetComponent<ArmorAttribute>() != null) { 
            /*计算入射角*/
            Vector3 ShellDirection = this.GetComponent<GetDirection>().Direction;
        Vector3 ArmorDirection = collision.collider.GetComponent<GetDirection>().Direction;
        float fenzi, fenmu;
        fenzi = (ShellDirection.x * ArmorDirection.x) + (ShellDirection.y * ArmorDirection.y) + (ShellDirection.z * ArmorDirection.z);
        fenmu = (Mathf.Sqrt(Mathf.Pow(ShellDirection.x, 2) + Mathf.Pow(ShellDirection.y, 2) + Mathf.Pow(ShellDirection.z, 2))) *
            (Mathf.Sqrt(Mathf.Pow(ArmorDirection.x, 2) + Mathf.Pow(ArmorDirection.y, 2) + Mathf.Pow(ArmorDirection.z, 2)));
        float AOIcos = -(fenzi / fenmu);

        ArmorAttribute ArmorThickness = collision.collider.GetComponent<ArmorAttribute>();
        float RealThickness = ArmorThickness.thickness / Mathf.Abs(AOIcos);

        MesofAPCR chuangshenduqu = this.GetComponent<MesofAPCR>();

        float Realchuangshen = chuangshenduqu.chuangshen;

        if (Realchuangshen >= RealThickness)
        {
            //Debug.Log("Destroy");
            collision.collider.transform.GetComponentInParent<PartAttribute>().aiHealthy -= 250;
        }
        else
            Debug.Log("Nothing happen");
        Debug.Log("等效" + RealThickness);
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        //Destroy(this.gameObject);
    }
}
