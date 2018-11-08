using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*返回朝向
     挂在炮弹，装甲板
     */
public class GetDirection : MonoBehaviour {

    public Transform Front;
    public Transform Behind;
    public Transform parentTra;//中心点或者父级
    Vector3 FrontlocalPos; Vector3 BehindlocalPos;
    Vector3 FrontworldPos; Vector3 BehindworldPos;
    Vector3 DirectionQ;float MoLength;
    public Vector3 Direction;

    void Start()
    {

    }

    void Update()
    {
        FrontlocalPos = Front.localPosition;
        BehindlocalPos = Behind.localPosition;
        FrontworldPos = parentTra.TransformPoint(FrontlocalPos);
        BehindworldPos = parentTra.TransformPoint(BehindlocalPos);
        DirectionQ = FrontworldPos - BehindworldPos;
        MoLength = Mathf.Sqrt(Mathf.Pow(DirectionQ.x,2) + Mathf.Pow(DirectionQ.y,2) + Mathf.Pow(DirectionQ.z,2));
        Direction = new Vector3(DirectionQ.x/MoLength,DirectionQ.y/MoLength,DirectionQ.z/MoLength);
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(Direction);
        }
    }
}
