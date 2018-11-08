using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*履带模型转动辅助*/
public class DrivingWheel : MonoBehaviour
{
    public GameObject TankBody;
    public Rigidbody FrontLeft;
    public Rigidbody RearLeft;
    public Rigidbody FrontRight;
    public Rigidbody RearRight;

    public Rigidbody fuzhulunL;
    public Rigidbody fuzhulunR;

    private int wheelTorque;

    public Transform LwatchPoint;
    public Transform RwatchPoint;
    private Vector3[] L_Pointlocation;
    private Vector3[] R_Pointlocation;

    private Vector3 rotateAxis;
    public Rigidbody rig;
    /*public GameObject middleUp;
    public GameObject middleDown;
    public GameObject leftUp;
    public GameObject leftDown;*/
    private float rotateSpeed;
    //private int iInL;
    //private int iInR;

    void Start()
    {
        L_Pointlocation = new Vector3[2];
        R_Pointlocation = new Vector3[2];
        L_Pointlocation[0] = L_Pointlocation[1] = new Vector3(0, 0, 0);
        R_Pointlocation[0] = R_Pointlocation[1] = new Vector3(0, 0, 0);
        //iInL = iInR = 0;
    }

    void Update()
    {
        rotateSpeed = GetSpeed();

        if(Input.GetKey(KeyCode.V))
        {
            Debug.Log(RearLeft.transform.position);
        }
        /*rotateAxis =
        middleUp.transform.position + GetSpeed() / 1.3f * (leftUp.transform.position - middleUp.transform.position) -
        middleDown.transform.position + GetSpeed() / 1.3f * (leftDown.transform.position - middleDown.transform.position);*/
    }

    void FixedUpdate()
    {
        L_speedBuilder();
        R_speedBuilder();
        Debug.Log(L_speedWatcher());

        if (Input.GetKey(KeyCode.W))//前进
        {
            wheelTorque = -900000;//扭矩
            FrontLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            if (GetSpeed() > 0.6)//履带上方辅助轮驱动条件
            {
                fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
                fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            }
            FrontLeft.transform.Rotate(0, -rotateSpeed*8, 0);
            RearLeft.transform.Rotate(0, -rotateSpeed*12, 0);
            FrontRight.transform.Rotate(0, -rotateSpeed*8, 0);
            RearRight.transform.Rotate(0, -rotateSpeed*12, 0);
            //fuzhulun.transform.Rotate(0, rotateSpeed * 14, 0);
            //			LeftRoll.AddRelativeTorque(Vector3.right*wheelTorque,ForceMode.Acceleration);
            //			RightRoll.AddRelativeTorque(Vector3.right*wheelTorque,ForceMode.Acceleration);
        }

        else if (Input.GetKey(KeyCode.S))//后退
        {
            wheelTorque = 900000;
            FrontLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            if (GetSpeed() > 0.6)
            {
                fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
                fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            }
            FrontLeft.transform.Rotate(0, rotateSpeed * 12, 0);
            RearLeft.transform.Rotate(0, rotateSpeed * 8, 0);
            FrontRight.transform.Rotate(0, rotateSpeed * 12, 0);
            RearRight.transform.Rotate(0, rotateSpeed * 8, 0);
            //fuzhulun.transform.Rotate(0, -rotateSpeed * 14, 0);
            //LeftRoll.AddRelativeTorque(Vector3.right*wheelTorque,ForceMode.Acceleration);
            //RightRoll.AddRelativeTorque(Vector3.right*wheelTorque,ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.A))//前进左转
        {
            wheelTorque = -900000;//扭矩
            FrontLeft.AddRelativeTorque(Vector3.up * wheelTorque/2, ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * wheelTorque/2, ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            /*if (GetSpeed() > 0.6)
            {
                fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
                fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            }*/
            fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
            fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            FrontLeft.transform.Rotate(0, -rotateSpeed * 8, 0);
            RearLeft.transform.Rotate(0, -rotateSpeed * 12, 0);
            FrontRight.transform.Rotate(0, -rotateSpeed * 12, 0);
            RearRight.transform.Rotate(0, -rotateSpeed * 16, 0);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))//前进右转
        {
            wheelTorque = -900000;//扭矩
            FrontLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * wheelTorque/2, ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * wheelTorque/2, ForceMode.Acceleration);
            /*if (GetSpeed() > 0.6)
            {
                fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
                fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            }*/
            fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
            fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            FrontLeft.transform.Rotate(0, -rotateSpeed * 12, 0);
            RearLeft.transform.Rotate(0, -rotateSpeed * 16, 0);
            FrontRight.transform.Rotate(0, -rotateSpeed * 8, 0);
            RearRight.transform.Rotate(0, -rotateSpeed * 12, 0);
        }
        else if (Input.GetKey(KeyCode.A))//原地左转
        {
            wheelTorque = -900000;//扭矩
            FrontLeft.AddRelativeTorque(Vector3.up * (-wheelTorque), ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * (-wheelTorque), ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            /**************************************************************************/
            FrontLeft.transform.Rotate(0, rotateSpeed * 8, 0);
            RearLeft.transform.Rotate(0, rotateSpeed * 12, 0);
            FrontRight.transform.Rotate(0, -rotateSpeed * 8, 0);
            RearRight.transform.Rotate(0, -rotateSpeed * 12, 0);
        }
        else if (Input.GetKey(KeyCode.D))//原地右转
        {
            wheelTorque = -900000;//扭矩
            FrontLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * (-wheelTorque), ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * (-wheelTorque), ForceMode.Acceleration);
            /**************************************************************************/
            FrontLeft.transform.Rotate(0, -rotateSpeed * 8, 0);
            RearLeft.transform.Rotate(0, -rotateSpeed * 12, 0);
            FrontRight.transform.Rotate(0, rotateSpeed * 8, 0);
            RearRight.transform.Rotate(0, rotateSpeed * 12, 0);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))//后退左转
        {
            wheelTorque = 900000;//扭矩
            FrontLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * wheelTorque / 2, ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * wheelTorque / 2, ForceMode.Acceleration);
            /*if (GetSpeed() > 0.6)
            {
                fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
                fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            }*/
            fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
            fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            FrontLeft.transform.Rotate(0, rotateSpeed * 16, 0);
            RearLeft.transform.Rotate(0, rotateSpeed * 12, 0);
            FrontRight.transform.Rotate(0, rotateSpeed * 12, 0);
            RearRight.transform.Rotate(0, rotateSpeed * 8, 0);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))//后退右转
        {
            wheelTorque = 900000;//扭矩
            FrontLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            RearLeft.AddRelativeTorque(Vector3.up * wheelTorque, ForceMode.Acceleration);
            FrontRight.AddRelativeTorque(Vector3.up * wheelTorque / 2, ForceMode.Acceleration);
            RearRight.AddRelativeTorque(Vector3.up * wheelTorque / 2, ForceMode.Acceleration);
            /*if (GetSpeed() > 0.6)
            {
                fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
                fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            }*/
            fuzhulunL.AddRelativeTorque(Vector3.up * (-wheelTorque) * L_speedWatcher(), ForceMode.Acceleration);
            fuzhulunR.AddRelativeTorque(Vector3.up * (-wheelTorque) * R_speedWatcher(), ForceMode.Acceleration);
            FrontLeft.transform.Rotate(0, rotateSpeed * 16, 0);
            RearLeft.transform.Rotate(0, rotateSpeed * 12, 0);
            FrontRight.transform.Rotate(0, rotateSpeed * 12, 0);
            RearRight.transform.Rotate(0, rotateSpeed * 8, 0);
        }

    }

    public float GetSpeed()
    {
        float speed = rig.velocity.magnitude;
        //Debug.Log(speed);
        return speed;
    }
    /*****************************************************************************/
    private void L_speedBuilder()//生成【左边】履带速度素材
    {

        /*if(iInL == 0)
        {
            L_Pointlocation[0] = LwatchPoint.position;
            iInL = 1;
        }
        else if(iInL == 1)
        {
            L_Pointlocation[1] = LwatchPoint.position;
            iInL = 0;
        }*/
        L_Pointlocation[1] = L_Pointlocation[0];
        L_Pointlocation[0] = LwatchPoint.position;


    }

    private Vector3 L_Speedchecker()//返回左边履带速度方向
    {
        Vector3 LV = L_Pointlocation[0] - L_Pointlocation[1];
        return LV;
    }
    private int L_speedWatcher()//【左边】通过比较履带速度方向和履带模型前后判断履带前进后退
    {
        Vector3 L_modelDirection = FrontLeft.transform.position - RearLeft.transform.position;
        Vector3 LV = L_Speedchecker();

        float angle = Vector3.Angle(L_modelDirection, LV);
        if (angle < 90)
        {
            return 1;
        }
        else
            return -1;
    }
    private void R_speedBuilder()//生成【左边】履带速度素材
    {

        /*if (iInR == 0)
        {
            R_Pointlocation[0] = RwatchPoint.position;
            iInR = 1;
        }
        else if (iInR == 1)
        {
            R_Pointlocation[1] = RwatchPoint.position;
            iInR = 0;
        }*/
        R_Pointlocation[1] = R_Pointlocation[0];
        R_Pointlocation[0] = RwatchPoint.position;

    }

    private Vector3 R_Speedchecker()//返回右边履带速度方向
    {
        Vector3 RV = R_Pointlocation[0] - R_Pointlocation[1];
        return RV;
    }
    private int R_speedWatcher()//【右边】通过比较履带速度方向和履带模型前后判断履带前进后退
    {
        Vector3 R_modelDirection = FrontRight.transform.position - RearRight.transform.position;
        Vector3 RV = R_Speedchecker();

        float angle = Vector3.Angle(R_modelDirection, RV);
        if (angle < 90)
        {
            return 1;
        }
        else
            return -1;
    }

}
