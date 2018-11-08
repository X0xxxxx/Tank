using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    
    private AutoCruiseState autoCruiseState;

    private int comboNum;

    private KeyCode keyCode;

    private TrackSimulator m_TrackSimulator;

    void Awake()
    {
        m_TrackSimulator = GetComponent<TrackSimulator>();
        autoCruiseState = new AutoCruiseState();
    }
    private void Start()
    {
        autoCruiseState.state = AutoCruiseState.close;
        comboNum = 0;
    }

    private void Update()
    {
        CheckAutoCruiseState();
    }
    void FixedUpdate()
    {
        
        float motorInput = Input.GetAxisRaw("Vertical");

        float steerInput = Input.GetAxisRaw("Horizontal");

        //Chech the track simulator component is not null and is set correctly
        if (m_TrackSimulator != null && m_TrackSimulator.IsSetUpCorrectly == true)
        {
            //Call the update function of track simulator component
            m_TrackSimulator.UpdateMove(motorInput, steerInput,autoCruiseState);
        }

    }

    private void OnGUI()
    {
        
    }
    void CheckAutoCruiseState()
    {
        
        float motorInput = Input.GetAxisRaw("Vertical");
        float steerInput = Input.GetAxisRaw("Horizontal");
        float autoCruise = Mathf.Abs(motorInput) + Mathf.Abs(steerInput);
        if (autoCruise > 0)
        {
            autoCruiseState.state = AutoCruiseState.close;
            return;
        }
        if (autoCruiseState.state == AutoCruiseState.close)
        {
            if (Input.GetKeyDown(KeyCode.R))
                autoCruiseState.state = AutoCruiseState.moveForwardByFristGear;
            if (Input.GetKeyDown(KeyCode.F))
                autoCruiseState.state = AutoCruiseState.backUpByFristGear;
            return;

        }
        if (autoCruiseState.state == AutoCruiseState.moveForwardByFristGear)
        {
            if (Input.GetKeyDown(KeyCode.R))
                autoCruiseState.state = AutoCruiseState.moveForwardBySecondGear;
            if (Input.GetKeyDown(KeyCode.F))
                autoCruiseState.state = AutoCruiseState.backUpByFristGear;
            return;
        }
        if (autoCruiseState.state == AutoCruiseState.moveForwardBySecondGear)
        {
            if (Input.GetKeyDown(KeyCode.R))
                autoCruiseState.state = AutoCruiseState.moveForwardByThirdGear;
            if (Input.GetKeyDown(KeyCode.F))
                autoCruiseState.state = AutoCruiseState.moveForwardByFristGear;
            return;
        }
        if (autoCruiseState.state == AutoCruiseState.moveForwardByThirdGear)
        {
            if (Input.GetKeyDown(KeyCode.F))
                autoCruiseState.state = AutoCruiseState.moveForwardBySecondGear;
            return;
        }
        if (autoCruiseState.state == AutoCruiseState.backUpByFristGear)
        {
            if (Input.GetKeyDown(KeyCode.R))
                autoCruiseState.state = AutoCruiseState.moveForwardByFristGear;
            if (Input.GetKeyDown(KeyCode.F))
                autoCruiseState.state = AutoCruiseState.backUpBySecondGear;
            return;
        }
        if (autoCruiseState.state == AutoCruiseState.backUpBySecondGear)
        {
            if (Input.GetKeyDown(KeyCode.R))
                autoCruiseState.state = AutoCruiseState.backUpByFristGear;
            return;
        }
              
    }

}
