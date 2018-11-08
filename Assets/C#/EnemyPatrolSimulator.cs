using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EnemyPatrolSimulator : MonoBehaviour {

    //坦克上方车轮
    [System.Serializable]
    public class UpperWheelData
    {
        public Transform wheelTransform;
    }

    //坦克下方车轮
    [System.Serializable]
    public class SuspendedWheelData
    {
        //车轮模型
        public Transform wheelTransform;

       //车轮骨架
        public Transform wheelBoneTransform;

        //车轮碰撞器
        public WheelCollider wheelCollider;
    }
    //履带数据，包含上方车轮数组，下方车轮数组，履带 SkinnedMeshRenderer
    [System.Serializable]
    public class BaseTrackData
    {
        //上方车轮数组
        public UpperWheelData[] upperWheelDataArray;

        //下方车轮数组 
        public SuspendedWheelData[] suspendedWheelDataArray;

        //履带 SkinnedMeshRenderer
        public SkinnedMeshRenderer trackSkinnedMeshRenderer;
    }

    //左右履带继承于 BaseTrackData
    [System.Serializable]
    public class LeftTrackData : BaseTrackData { }

    [System.Serializable]
    public class RightTrackData : BaseTrackData { }

    //左履带实例
    public LeftTrackData leftTrackData;

    //有履带实例
    public RightTrackData rightTrackData;

    //车轮力矩，扭矩动画曲线
    [System.Serializable]
    public class WheelFeaturesAnimationCurves
    {
        //车轮力矩动画曲线
        public AnimationCurve singleWheelMotorAnimationCurve = AnimationCurve.Linear(0.0f, 10000.0f, 20f, 0.0f);

        //车轮阻力矩动画曲线
        public AnimationCurve singleWheelBrakeAnimationCurve = AnimationCurve.Linear(0.0f, 10000.0f, 20f, 12000.0f);

        //The animationCurve which recorded the relationship between wheel's rotation speed and the torque that applied to the vehicle's Y axis 
        public AnimationCurve singleWheelSteerTorqueYValueAnimationCurve = AnimationCurve.Linear(0.0f, 0.5f, 20f, 0.2f);
    }


    //动画曲线实例
    public WheelFeaturesAnimationCurves wheelFeaturesAnimationCurves=new WheelFeaturesAnimationCurves();

    //车轮和骨架位移补偿方向枚举
    public enum WheelAndBoneTransformOffsetDirection
    {
        X,
        Y,
        Z
    }

    //车轮旋转轴枚举
    public enum WheelRotateAxis
    {
        X,
        Y,
        Z
    }
    //履带贴图偏移方向枚举
    public enum TrackTextureOffsetType
    {
        X,
        Y
    }

    //调整属性
    [System.Serializable]
    public class AdjustParameters
    {
        //车轮和骨架偏移方向设置为Y
        public WheelAndBoneTransformOffsetDirection wheelAndBoneTransformOffsetDirection = WheelAndBoneTransformOffsetDirection.Y;
        //车轮和骨架方向偏移设置为false
        public bool inverseWheelAndBoneTransformOffsetDirection = false;
        //车轮和骨架偏移值
        public float singleWheelAndBoneTransfromVerticalOffset = 0.1f;

        //车轮旋转轴
        public WheelRotateAxis wheelRotateAxis = WheelRotateAxis.X;
        //反向旋转设置为false
        public bool inverseWheelRotateDirection = false;

        //履带贴图偏移方向设置为X 
        public TrackTextureOffsetType trackTextureOffsetType = TrackTextureOffsetType.X;
        //贴图反向偏移设置为false;
        public bool inverseTrackTextureOffsetDirection = false;
        //履带贴图偏移因数
        public float singleTrackTextureOffsetSpeedMultiplier = 1.111111f;

    }
    // 调整属性实例
    public AdjustParameters adjustParameters;

    // 中心
    public Transform centerOfMass;

    // 最大角速度
    public float rigidbodyMaxAngularVelocity = 5.0f;

    // 是否在屏幕上显示debug
    public bool showDebugInformation = false;

    // 判断参数是否设置正确 
    public bool IsSetUpCorrectly
    {
        set;
        get;
    }

    //坦克刚体
    private Rigidbody m_Rigidbody;

    //判断坦克履带数据是否设置正确
    public bool CheckIfTrackDataSettingIsCorrect(BaseTrackData trackData)
    {
        string trackDataName = string.Empty;

        //判断trackData是LeftTrackData还是RightTrackData
        if (trackData is LeftTrackData)
        {
            trackDataName = "LeftTrackData";
        }
        else
        {
            trackDataName = "RightTrackData";
        }

        //创建用于存储内部数据的对象
        UpperWheelData[] upperWheelArray = trackData.upperWheelDataArray;

        SuspendedWheelData[] suspendedWheelDataArray = trackData.suspendedWheelDataArray;

        SkinnedMeshRenderer trackRender = trackData.trackSkinnedMeshRenderer;

        //遍历数组，如果为完全设置正确，返回false
        for (int i = 0; i < upperWheelArray.Length; i++)
        {
            UpperWheelData upperWheelData = upperWheelArray[i];

            Transform wheelTransform = upperWheelData.wheelTransform;

            if (wheelTransform == null)
            {
                string message = string.Format("You are not set {0}-> UpperWheelData0{1} -> Wheel Model !", trackDataName, i);

                EditorUtility.DisplayDialog("Track Simulator", message, "OK");

                return false;
            }
        }

        for (int i = 0; i < suspendedWheelDataArray.Length; i++)
        {
            SuspendedWheelData suspendedWheelData = suspendedWheelDataArray[i];

            Transform wheelTransform = suspendedWheelData.wheelTransform;

            Transform wheelBoneTransform = suspendedWheelData.wheelBoneTransform;

            WheelCollider wheelCollider = suspendedWheelData.wheelCollider;

            if (wheelTransform == null)
            {
                string message = string.Format("You are not set {0} -> SuspendedWheelData0{1} -> Wheel Model !", trackDataName, i);

                EditorUtility.DisplayDialog("Track Simulator", message, "OK");

                return false;
            }

            if (wheelBoneTransform == null)
            {
                string message = string.Format("You are not set {0} -> SuspendedWheelData0{1} -> Wheel Bone !", trackDataName, i);

                EditorUtility.DisplayDialog("Track Simulator", message, "OK");

                return false;
            }

            if (wheelCollider == null)
            {
                string message = string.Format("You are not set {0} -> suspendedWheelData0{1} -> Wheel Collider !", trackDataName, i);

                EditorUtility.DisplayDialog("Track Simulator", message, "OK");

                return false;
            }

        }

        if (trackRender == null)
        {
            string message = string.Format("You are not set {0} -> Track SkinnedMeshRenderer !", trackDataName);

            EditorUtility.DisplayDialog("Track Simulator", message, "OK");

            return false;
        }

        return true;

    }
    //判断MassCenter是否设置正确
    public bool CheckIfMassCenterSettingIsCorrect(Transform massCenter)
    {
        if (massCenter == null)
        {
            EditorUtility.DisplayDialog("Track Simulator", "massCenter is null,please set it correctly", "OK");

            return false;
        }

        return true;
    }

    //判断碰撞器是否设置正确
    public bool CheckIfBodyColliderIsSetUp()
    {
        Collider[] colliderArray = transform.GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliderArray.Length; i++)
        {
            Collider collider = colliderArray[i];

            if (collider is WheelCollider)
            {
                continue;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
    //使用上面定义的方法判断各个组件是否设置正确
    public void Awake()
    {
        if (CheckIfTrackDataSettingIsCorrect(leftTrackData) == false)
        {
            IsSetUpCorrectly = false;

            EditorApplication.isPlaying = false;

            return;
        }
        if (CheckIfTrackDataSettingIsCorrect(rightTrackData) == false)
        {
            IsSetUpCorrectly = false;

            EditorApplication.isPlaying = false;

            return;
        }

        if (CheckIfMassCenterSettingIsCorrect(centerOfMass) == false)
        {
            IsSetUpCorrectly = false;

            EditorApplication.isPlaying = false;

            return;
        }

        if (CheckIfBodyColliderIsSetUp() == false)
        {
            EditorUtility.DisplayDialog("Track Simulator", "Please add a box or mesh collider to its main body", "OK");

            IsSetUpCorrectly = false;

            EditorApplication.isPlaying = false;

            return;
        }

        IsSetUpCorrectly = true;

        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.maxAngularVelocity = rigidbodyMaxAngularVelocity;

        m_Rigidbody.centerOfMass = centerOfMass.localPosition;

        IsSetUpCorrectly = true;


    }


    public void UpdateMove(float motorInput, float steerInput)
    {
        float leftTrackSteerTorqueYValue = UpdateTrackController(motorInput, steerInput, leftTrackData);

        float rightTrackSteerTorqueYValue = UpdateTrackController(motorInput, steerInput, rightTrackData);

        float totalSteerTorqueYValue = leftTrackSteerTorqueYValue + rightTrackSteerTorqueYValue;

        //车辆旋转
        if (Mathf.Abs(m_Rigidbody.angularVelocity.y) < 1.0f)
        {
            if (CheckIfMoveBackward(motorInput) == false)
            {
                m_Rigidbody.AddRelativeTorque(0.0f, totalSteerTorqueYValue, 0.0f, ForceMode.Acceleration);
            }
            else
            {
                m_Rigidbody.AddRelativeTorque(0.0f, -totalSteerTorqueYValue, 0.0f, ForceMode.Acceleration);
            }
        }

    }
    //更新履带控制，
    private float UpdateTrackController(float motorInput, float steerInput, BaseTrackData trackData)
    {
        //总转向扭矩在Y轴上的值
        float totalWheelSteerTorqueYValue = 0.0f;

        //准备工作，用于定义各种实例
        #region Prepare

        UpperWheelData[] upperWheelArray = trackData.upperWheelDataArray;

        SuspendedWheelData[] suspendedWheelDataArray = trackData.suspendedWheelDataArray;

        SkinnedMeshRenderer trackRender = trackData.trackSkinnedMeshRenderer;


        WheelAndBoneTransformOffsetDirection wheelAndBoneTransformOffsetDirection = adjustParameters.wheelAndBoneTransformOffsetDirection;

        bool inverseWheelAndBoneTransformOffsetDirection = adjustParameters.inverseWheelAndBoneTransformOffsetDirection;

        float singleWheelAndBoneTransfromVerticalOffset = adjustParameters.singleWheelAndBoneTransfromVerticalOffset;


        WheelRotateAxis wheelRotateAxis = adjustParameters.wheelRotateAxis;

        bool inverseWheelRotateDirection = adjustParameters.inverseWheelRotateDirection;


        TrackTextureOffsetType trackTextureOffsetType = adjustParameters.trackTextureOffsetType;

        bool inverseTrackTextureOffsetDirection = adjustParameters.inverseTrackTextureOffsetDirection;

        float singleTrackTextureOffsetSpeedMultiplier = adjustParameters.singleTrackTextureOffsetSpeedMultiplier;


        float inverseWheelAndBoneTransformOffsetDirectionFlag = 0.0f;
        //根据inverseWheelAndBoneTransformOffsetDirection，指定inverseWheelAndBoneTransformOffsetDirectionFlag的值（作为车轮模型和bone位移的乘数）
        if (inverseWheelAndBoneTransformOffsetDirection == true)
        {
            inverseWheelAndBoneTransformOffsetDirectionFlag = -1.0f;
        }
        else
        {
            inverseWheelAndBoneTransformOffsetDirectionFlag = 1.0f;
        }

        //车轮模型旋转方向乘数
        float inverseWheelRotateDirectionFlag = 0.0f;

        if (inverseWheelRotateDirection == true)
        {
            inverseWheelRotateDirectionFlag = -1.0f;
        }
        else
        {
            inverseWheelRotateDirectionFlag = 1.0f;
        }

        //履带贴图偏移方向乘数
        float inverseTrackTextureOffsetDirectionFlag = 0.0f;

        if (inverseTrackTextureOffsetDirection == true)
        {
            inverseTrackTextureOffsetDirectionFlag = -1.0f;
        }
        else
        {
            inverseTrackTextureOffsetDirectionFlag = 1.0f;
        }

        //车轮平均每分钟旋转次数
        float wheelAverageSpeedRpm = CalculateWheelsAverageRPM(suspendedWheelDataArray);
        //车轮平均每分钟旋转度数（Time.fixedDeltaTime作为乘数）
        float wheelAverageSpeedDegreePerSecond = Time.fixedDeltaTime * wheelAverageSpeedRpm * 360.0f / 60.0f;

        #endregion
        //上车轮模型数组处理
        #region Handle UpperWheelArray

        for (int i = 0; i < upperWheelArray.Length; i++)
        {
            UpperWheelData upperWheelData = upperWheelArray[i];

            Transform wheelTransform = upperWheelData.wheelTransform;

            if (wheelRotateAxis == WheelRotateAxis.X)
            {
                wheelTransform.rotation = wheelTransform.rotation * Quaternion.Euler(inverseWheelRotateDirectionFlag * wheelAverageSpeedDegreePerSecond, 0.0f, 0.0f);
            }
            else if (wheelRotateAxis == WheelRotateAxis.Y)
            {
                wheelTransform.rotation = wheelTransform.rotation * Quaternion.Euler(0.0f, inverseWheelRotateDirectionFlag * wheelAverageSpeedDegreePerSecond, 0.0f);
            }
            else  //(wheelRotateAxis == WheelRotateAxis.Z)
            {
                wheelTransform.rotation = wheelTransform.rotation * Quaternion.Euler(0.0f, 0.0f, inverseWheelRotateDirectionFlag * wheelAverageSpeedDegreePerSecond);
            }

        }

        #endregion
        //下车轮数据数组处理
        #region Handle SuspendedWheelDataArray

        for (int i = 0; i < suspendedWheelDataArray.Length; i++)
        {
            SuspendedWheelData suspendedWheelData = suspendedWheelDataArray[i];


            WheelCollider wheelCollider = suspendedWheelData.wheelCollider;

            wheelCollider.wheelDampingRate = Mathf.Lerp(100.0f, 0.0f, Mathf.Abs(motorInput));

            //移动目标位置
            Vector3 wheelTranformTargetPosition = Vector3.zero;

            Quaternion wheelTransfromTargetRotation = Quaternion.identity;
            //获取车轮碰撞器的世界坐标位置与旋转
            wheelCollider.GetWorldPose(out wheelTranformTargetPosition, out wheelTransfromTargetRotation);

            //车轮碰撞器竖值方向
            Vector3 wheelColliderVerticalDirection = Vector3.zero;

            if (wheelAndBoneTransformOffsetDirection == WheelAndBoneTransformOffsetDirection.X)
            {
                wheelColliderVerticalDirection = wheelCollider.transform.right * inverseWheelAndBoneTransformOffsetDirectionFlag;
            }
            else if (wheelAndBoneTransformOffsetDirection == WheelAndBoneTransformOffsetDirection.Y)
            {
                wheelColliderVerticalDirection = wheelCollider.transform.up * inverseWheelAndBoneTransformOffsetDirectionFlag;
            }
            else   //wheelAndBoneTransformOffsetDirection == WheelAndBoneTransformOffsetDirection.Z
            {
                wheelColliderVerticalDirection = wheelCollider.transform.forward * inverseWheelAndBoneTransformOffsetDirectionFlag;
            }

            //由于履带存在厚度，骨架与车轮模型需在竖直方向上偏移
            Transform wheelBoneTransform = suspendedWheelData.wheelBoneTransform;

            Vector3 wheelBoneTransformVerticalOffset = wheelColliderVerticalDirection * singleWheelAndBoneTransfromVerticalOffset;

            wheelBoneTransform.position = wheelTranformTargetPosition + wheelBoneTransformVerticalOffset;



            Transform wheelTransform = suspendedWheelData.wheelTransform;

            Vector3 wheelTranformVerticalOffset = wheelColliderVerticalDirection * singleWheelAndBoneTransfromVerticalOffset;

            wheelTransform.position = wheelTranformTargetPosition + wheelTranformVerticalOffset;
            //根据旋转轴和每分钟旋转度数控制车轮模型旋转
            if (wheelRotateAxis == WheelRotateAxis.X)
            {
                wheelTransform.rotation = wheelTransform.rotation * Quaternion.Euler(inverseWheelRotateDirectionFlag * wheelAverageSpeedDegreePerSecond, 0.0f, 0.0f);
            }
            else if (wheelRotateAxis == WheelRotateAxis.Y)
            {
                wheelTransform.rotation = wheelTransform.rotation * Quaternion.Euler(0.0f, inverseWheelRotateDirectionFlag * wheelAverageSpeedDegreePerSecond, 0.0f);
            }
            else   //wheelRotateAxis == WheelRotateAxis.Z
            {
                wheelTransform.rotation = wheelTransform.rotation * Quaternion.Euler(0.0f, 0.0f, inverseWheelRotateDirectionFlag * wheelAverageSpeedDegreePerSecond);
            }

            //根据车轮碰撞器的半径和转速计算出车轮的速度
            float wheelSpeedKMPH = Mathf.Abs(ConvertSingleWheelRPMToKMPH(wheelCollider.radius, wheelCollider.rpm));

            
            CalculateSingleWheelMotorTorque(motorInput, steerInput, trackData, wheelCollider, wheelSpeedKMPH);

            CalculateSingleWheelBrakeTorque(motorInput, steerInput, wheelCollider, wheelSpeedKMPH);
            
            
            float singleWheelSteerTorqueYValue = CalculateSingleWheelSteerTorqueYValue(motorInput, steerInput, wheelCollider, wheelSpeedKMPH);

            totalWheelSteerTorqueYValue = totalWheelSteerTorqueYValue + singleWheelSteerTorqueYValue;

        }


        #endregion
        //履带贴图偏移处理
        #region Handle TrackSkinnedMeshRenderer

        Vector2 trackMainTextureOffset = trackRender.material.GetTextureOffset("_MainTex");

        Vector2 trackBumpMapOffset = trackRender.material.GetTextureOffset("_BumpMap");

        if (trackTextureOffsetType == TrackTextureOffsetType.X)
        {
            trackMainTextureOffset.x = Mathf.Repeat(trackMainTextureOffset.x + (inverseTrackTextureOffsetDirectionFlag * wheelAverageSpeedDegreePerSecond * singleTrackTextureOffsetSpeedMultiplier) / 360.0f, 1.0f);

            trackBumpMapOffset.x = Mathf.Repeat(trackBumpMapOffset.x + (inverseTrackTextureOffsetDirectionFlag * wheelAverageSpeedDegreePerSecond * singleTrackTextureOffsetSpeedMultiplier) / 360.0f, 1.0f);
        }
        else
        {
            trackMainTextureOffset.y = Mathf.Repeat(trackMainTextureOffset.y + (inverseTrackTextureOffsetDirectionFlag * wheelAverageSpeedDegreePerSecond * singleTrackTextureOffsetSpeedMultiplier) / 360.0f, 1.0f);

            trackBumpMapOffset.y = Mathf.Repeat(trackBumpMapOffset.y + (inverseTrackTextureOffsetDirectionFlag * wheelAverageSpeedDegreePerSecond * singleTrackTextureOffsetSpeedMultiplier) / 360.0f, 1.0f);
        }

        trackRender.material.SetTextureOffset("_MainTex", trackMainTextureOffset);

        trackRender.material.SetTextureOffset("_BumpMap", trackBumpMapOffset);

        #endregion


        return totalWheelSteerTorqueYValue;
    }

    //根据motorInput和steerInput为车轮碰撞器添加力矩
    private void CalculateSingleWheelMotorTorque(float motorInput, float steerInput, BaseTrackData trackData, WheelCollider wheelCollider, float wheelSpeedKMPH)
    {
        AnimationCurve singleWheelMotorAnimationCurve = wheelFeaturesAnimationCurves.singleWheelMotorAnimationCurve;

        float direction = 0.0f;

        if (trackData is LeftTrackData)
        {
            direction = 1.0f;
        }
        else
        {
            direction = -1.0f;
        }

        if (motorInput == 0.0f && steerInput == 0.0f)
        {
            wheelCollider.motorTorque = 0.0f;

        }
        else if (motorInput == 0.0f && steerInput != 0.0f)
        {
            wheelCollider.motorTorque = steerInput * direction * singleWheelMotorAnimationCurve.Evaluate(wheelSpeedKMPH); ;
        }
        else if (motorInput != 0.0f && steerInput == 0.0f)
        {
            wheelCollider.motorTorque = motorInput * singleWheelMotorAnimationCurve.Evaluate(wheelSpeedKMPH);
        }
        else //if (motorInput != 0.0f && steerInput != 0.0f)
        {
            wheelCollider.motorTorque = motorInput * singleWheelMotorAnimationCurve.Evaluate(wheelSpeedKMPH);
        }
    }

    //根据motorInput和steerInput为车轮碰撞器添加阻力矩
    private void CalculateSingleWheelBrakeTorque(float motorInput, float steerInput, WheelCollider wheelCollider, float wheelSpeedKMPH)
    {
        AnimationCurve singleWheelBrakeAnimationCurve = wheelFeaturesAnimationCurves.singleWheelBrakeAnimationCurve;

        if (motorInput == 0.0f && steerInput == 0.0f)
        {
            wheelCollider.brakeTorque = singleWheelBrakeAnimationCurve.Evaluate(wheelSpeedKMPH);
        }
        else if (motorInput == 0.0f && steerInput != 0.0f)
        {
            wheelCollider.brakeTorque = 0.0f;
        }
        else if (motorInput != 0.0f && steerInput == 0.0f)
        {
            if (wheelCollider.rpm > 0.0f && motorInput < 0.0f)
            {
                wheelCollider.brakeTorque = singleWheelBrakeAnimationCurve.Evaluate(wheelSpeedKMPH);
            }
            else if (wheelCollider.rpm < 0.0f && motorInput > 0.0f)
            {
                wheelCollider.brakeTorque = singleWheelBrakeAnimationCurve.Evaluate(wheelSpeedKMPH);
            }
            else
            {
                wheelCollider.brakeTorque = 0.0f;
            }

        }
        else //if (motorInput != 0.0f && steerInput != 0.0f)
        {
            if (wheelCollider.rpm > 0.0f && motorInput < 0.0f)
            {
                wheelCollider.brakeTorque = singleWheelBrakeAnimationCurve.Evaluate(wheelSpeedKMPH);
            }
            else if (wheelCollider.rpm < 0.0f && motorInput > 0.0f)
            {
                wheelCollider.brakeTorque = singleWheelBrakeAnimationCurve.Evaluate(wheelSpeedKMPH);
            }
            else
            {
                wheelCollider.brakeTorque = 0.0f;
            }
        }
    }

    //计算车辆转向扭矩
    private float CalculateSingleWheelSteerTorqueYValue(float motorInput, float steerInput, WheelCollider wheelCollider, float wheelKMPHSpeed)
    {
        AnimationCurve singleWheelSteerTorqueYValueAnimationCurve = wheelFeaturesAnimationCurves.singleWheelSteerTorqueYValueAnimationCurve;

        float singleWheelSteerTorqueYValue = 0.0f;

        if (motorInput == 0.0f && steerInput == 0.0f)
        {
            singleWheelSteerTorqueYValue = 0.0f;
        }
        else if (motorInput == 0.0f && steerInput != 0.0f)
        {
            if (wheelCollider.isGrounded == true)
            {
                singleWheelSteerTorqueYValue = (steerInput * singleWheelSteerTorqueYValueAnimationCurve.Evaluate(wheelKMPHSpeed));

            }
            else
            {
                singleWheelSteerTorqueYValue = 0.0f;
            }

        }
        else if (motorInput != 0.0f && steerInput == 0.0f)
        {
            singleWheelSteerTorqueYValue = 0.0f;
        }
        else //if (motorInput != 0.0f && steerInput != 0.0f)
        {
            if (wheelCollider.isGrounded == true)
            {
                if (wheelCollider.rpm > 0.0f && motorInput < 0.0f)
                {
                    singleWheelSteerTorqueYValue = 0.0f;
                }
                else if (wheelCollider.rpm < 0.0f && motorInput > 0.0f)
                {
                    singleWheelSteerTorqueYValue = 0.0f;
                }
                else
                {
                    singleWheelSteerTorqueYValue = (steerInput * singleWheelSteerTorqueYValueAnimationCurve.Evaluate(wheelKMPHSpeed));
                }
            }
            else
            {
                singleWheelSteerTorqueYValue = 0.0f;
            }
        }

        return singleWheelSteerTorqueYValue;
    }
    
    private float CalculateWheelsAverageRPM(SuspendedWheelData[] suspendedWheelDataArray)
    {
        float AverageRpmOfWheels = 0.0f;

        List<int> groundedWheelIndexList = new List<int>();

        for (int i = 0; i < suspendedWheelDataArray.Length; i++)
        {
            if (suspendedWheelDataArray[i].wheelCollider.isGrounded == true)
            {
                groundedWheelIndexList.Add(i);
            }
        }

        if (groundedWheelIndexList.Count == 0)
        {
            float totalRpmOfWheelArray = 0.0f;

            for (int i = 0; i < suspendedWheelDataArray.Length; i++)
            {
                SuspendedWheelData suspendedWheelData = suspendedWheelDataArray[i];

                totalRpmOfWheelArray = totalRpmOfWheelArray + suspendedWheelData.wheelCollider.rpm;
            }

            AverageRpmOfWheels = totalRpmOfWheelArray / suspendedWheelDataArray.Length;
        }
        else
        {
            float totalRpmOfGroundedWheelList = 0.0f;

            for (int i = 0; i < groundedWheelIndexList.Count; i++)
            {
                int groundedWheelIndex = groundedWheelIndexList[i];

                totalRpmOfGroundedWheelList = totalRpmOfGroundedWheelList + suspendedWheelDataArray[groundedWheelIndex].wheelCollider.rpm;
            }

            AverageRpmOfWheels = totalRpmOfGroundedWheelList / groundedWheelIndexList.Count;
        }

        return AverageRpmOfWheels;
    }

    //检查车辆是否后退
    private bool CheckIfMoveBackward(float motorInput)
    {
        bool isMoveBackward = false;

        if (motorInput < 0.0f && transform.InverseTransformDirection(m_Rigidbody.velocity).z < 1.0f)
        {
            isMoveBackward = true;
        }
        else
        {
            isMoveBackward = false;
        }

        return isMoveBackward;
    }


    private float ConvertSingleWheelRPMToKMPH(float wheelRadius, float revolutionsPerMinute)
    {
        float wheelPerimeter = 2.0f * Mathf.PI * wheelRadius;

        float meterPerMinute = revolutionsPerMinute * wheelPerimeter;

        float meterPerHour = meterPerMinute * 60.0f;

        float kilometerPerHour = meterPerHour / 1000.0f;

        return kilometerPerHour;

    }
}
