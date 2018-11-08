using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    //挂挡
    enum Gear
    {
        firstGear, secondGear, thirdGear
    }
    private Gear gear;
    //换挡
    enum ShiftGear
    {
        gearUp,gearDown
    }
    //各档位速度
    [System.Serializable]
    public class GearSpeed
    {
        public float 
            firstGearMaxSpeed, firstGearSwerveSpeed,
            secondGearMaxSpeed, secondGearSwerveSpeed, 
            thirdGearMaxSpeed, thirdGearSwerveSpeed ,
            reverseGearMaxSpeed, reverseGearSwerveSpeed,
            InSituSwerveSpeed;
    }
    public GearSpeed gearSpeed;

    private float maxSpeed;
    [System.Serializable]
    public enum Orientations
    {
       X, reverseX, Z, reverseZ
    }
    public Orientations orientations;

    float orientationValue;

    Vector3 tankDirection=Vector3.forward;
    //履带
    public Transform LPedrail;
    public Transform RPedrail;
    private Rigidbody LPedrail_Rigidbody;
    private Rigidbody RPedrail_Rigidbody;

    private Transform LCollider;
    private Transform RCollider;

    private float totalMass;

    // Use this for initialization
    void Start () {
        gear = Gear.firstGear;
        LPedrail_Rigidbody = LPedrail.GetComponent<Rigidbody>();
        RPedrail_Rigidbody = RPedrail.GetComponent<Rigidbody>();
        maxSpeed =Km_HToM_S( gearSpeed.firstGearMaxSpeed);


        LCollider = LPedrail;
        RCollider = RPedrail;

        if(orientations==Orientations.X|| orientations == Orientations.Z)
        {
            orientationValue = 1;
        }
        else
        {
            orientationValue = -1;
        }
        switch (orientations)
        {
            case Orientations.Z: tankDirection = Vector3.forward; break;
            case Orientations.reverseZ: tankDirection = Vector3.back; break;
            case Orientations.X: tankDirection = Vector3.left; break;
            case Orientations.reverseX: tankDirection = Vector3.right; break;
            default:/*Debug.Log(tankDirection);*/break;
                
        }

    }
	
	void Update () {
    }
    private void FixedUpdate()
    {
        
        //判断是否换挡
        if (Input.GetKeyDown(KeyCode.R))
            ShiftGears(ShiftGear.gearUp);
        else if (Input.GetKeyDown(KeyCode.F))
            ShiftGears(ShiftGear.gearDown);
        //
        if(RCollider.GetComponent<PedrailCollisionDetection>().onGround==true|| LCollider.GetComponent<PedrailCollisionDetection>().onGround)
        {

            Movement();
        }
        Tank();



    }

    void Movement()
    {
        float maxSpeed = 0;
        float swerveSpeed = 0;
        float powerFactor = Input.GetAxis("Vertical");
        float swerveFactor = Input.GetAxis("Horizontal");
        if (powerFactor < 0)
        {
            maxSpeed= Km_HToM_S(gearSpeed.reverseGearMaxSpeed);
            swerveSpeed = Km_HToM_S(gearSpeed.reverseGearSwerveSpeed);
        }
        else
        {
            if (gear == Gear.firstGear)
            {
                maxSpeed = Km_HToM_S(gearSpeed.firstGearMaxSpeed);
                swerveSpeed = Km_HToM_S(gearSpeed.firstGearSwerveSpeed);
            }
            else if (gear == Gear.secondGear)
            {
                maxSpeed = Km_HToM_S(gearSpeed.secondGearMaxSpeed);
                swerveSpeed = Km_HToM_S(gearSpeed.secondGearSwerveSpeed);

            }
            else if (gear == Gear.thirdGear)
            {
                maxSpeed = Km_HToM_S(gearSpeed.thirdGearMaxSpeed);
                swerveSpeed = Km_HToM_S(gearSpeed.thirdGearSwerveSpeed);
            }
        }
        
        LSpeedControl(powerFactor, swerveFactor, maxSpeed,swerveSpeed);
        RSpeedControl(powerFactor, swerveFactor, maxSpeed,swerveSpeed);

    }
    //坦克换挡
    void ShiftGears(ShiftGear shiftGears)
    {
        
        if (shiftGears == ShiftGear.gearUp)
        {
            if (gear == Gear.firstGear)
                gear = Gear.secondGear;
            else if (gear == Gear.secondGear)
                gear = Gear.thirdGear;
        }
        else if (shiftGears == ShiftGear.gearDown)
        {
            if (gear == Gear.secondGear)
                gear = Gear.firstGear;
            else if (gear == Gear.thirdGear)
                gear = Gear.secondGear;
        }
        
    }

    //移动和转向
    void LSpeedControl(float powerFactor, float swerveFactor, float maxSpeed,float swerveSpeed)
    {
        float limitSpeed;
        Vector3 LPedrailSpeed3 = LPedrail_Rigidbody.velocity;
        float LSpeedRate = LPedrailSpeed3.magnitude / LPedrail_Rigidbody.transform.InverseTransformVector(LPedrailSpeed3).magnitude;
        limitSpeed = SpeedControl(maxSpeed, swerveSpeed, powerFactor, swerveFactor, true); 
        if (powerFactor == 0)
        {
            if (swerveFactor != 0)
                LPedrail_Rigidbody.velocity = Vector3.Lerp(LPedrail_Rigidbody.velocity,
                    LPedrail_Rigidbody.transform.TransformVector(tankDirection).normalized * gearSpeed.InSituSwerveSpeed*Mathf.Sign(swerveFactor),
                    Time.time);
        }
        else
        {
            LPedrail_Rigidbody.velocity = Vector3.Lerp(LPedrail_Rigidbody.velocity,
                LPedrail_Rigidbody.transform.TransformVector(tankDirection).normalized * limitSpeed ,
                Time.time);

        }
    }
    void RSpeedControl(float powerFactor, float swerveFactor, float maxSpeed, float swerveSpeed)
    {
        float limitSpeed;
        Vector3 RPedrailSpeed3 = RPedrail_Rigidbody.velocity;
        float RSpeedRate = RPedrailSpeed3.magnitude / RPedrail_Rigidbody.transform.InverseTransformVector(RPedrailSpeed3).magnitude;
        limitSpeed = SpeedControl(maxSpeed, swerveSpeed, powerFactor, swerveFactor, false);
        if (powerFactor == 0)
        {
            if (swerveFactor != 0)
                RPedrail_Rigidbody.velocity = Vector3.Lerp(RPedrail_Rigidbody.velocity,
                    RPedrail_Rigidbody.transform.TransformVector(tankDirection).normalized * gearSpeed.InSituSwerveSpeed * Mathf.Sign(swerveFactor)*-1,
                    Time.time);        
        }
        else
        {
            RPedrail_Rigidbody.velocity = Vector3.Lerp(RPedrail_Rigidbody.velocity,
                RPedrail_Rigidbody.transform.TransformVector(tankDirection).normalized * limitSpeed,
                Time.time);
        }
    }

    float SpeedControl(float maxSpeed,float swerveSpeed, float powerFactor, float swerveFactor,bool left)
    {
        float outputSpeed = 0;
        float sign = Mathf.Sign(powerFactor);
        if (left)
        {
            if(powerFactor!=0)
            {
                outputSpeed = maxSpeed * sign;
                if (swerveFactor < 0)
                {
                    outputSpeed = sign *swerveSpeed ;
                }
            }
        }
        else
        {
            if (powerFactor != 0)
            {
                outputSpeed = maxSpeed * sign;
                if (swerveFactor > 0)
                {
                    outputSpeed = sign * swerveSpeed;
                }
            }

        }
        
        return outputSpeed;
    }
    void Tank()
    {
        
        float powerFactor = Input.GetAxis("Vertical");
        float swerveFactor = Input.GetAxis("Horizontal");
        float Lspeed = LPedrail_Rigidbody.velocity.magnitude;
        float Rspeed = RPedrail_Rigidbody.velocity.magnitude;/*Debug.Log(M_SToKm_H(Lspeed));*/
        if(powerFactor>0&&swerveFactor==0&&Mathf.Abs( Lspeed- maxSpeed) <=2 && Mathf.Abs(Rspeed - maxSpeed) <= 2)
        {
            
            if (gear == Gear.firstGear)
                gear = Gear.secondGear;
            else if (gear == Gear.secondGear)
                gear = Gear.thirdGear;
        }
    }

    

    //单位换算
    float M_SToKm_H(float speed)
    {
        return speed * 3.6f;
    }
    float Km_HToM_S(float speed)
    {
        return speed / 3.6f;
    }
    
}
