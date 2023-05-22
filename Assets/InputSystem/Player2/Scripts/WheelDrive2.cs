using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DriveType2 { RearWheelDrive, FrontWheelDrive, AllWheelDrive }
public class WheelDrive2 : MonoBehaviour
{
    [SerializeField] float maxAngle2 = 30f;
    [SerializeField] float maxTorque2 = 300f;
    [SerializeField] float brakeTorque2 = 60000f;
    [SerializeField] GameObject wheelShape2;

    [SerializeField] float criticalSpeed2 = 7f;
    [SerializeField] int stepBelow2 = 5;
    [SerializeField] int stepAbove2 = 2;

    [SerializeField] DriveType2 driveType2;
    WheelCollider[] m_Wheels2;
    float handBrake2, torque2;
    public float angle2;

    public InputActionAsset inputActions2;
    InputActionMap gameplayActionMap2;
    InputAction handBrakeInputAction2;
    InputAction steeringInputAction2;
    InputAction accelerationInputAction2;

    void Awake()
    {
        gameplayActionMap2 = inputActions2.FindActionMap("Gameplay2");

        handBrakeInputAction2 = gameplayActionMap2.FindAction("HandBrake2");
        steeringInputAction2 = gameplayActionMap2.FindAction("SteeringAngle2");
        accelerationInputAction2 = gameplayActionMap2.FindAction("Acceleration2");

        handBrakeInputAction2.performed += GetHandBrakeInput2;
        handBrakeInputAction2.canceled += GetHandBrakeInput2;

        steeringInputAction2.performed += GetAngleInput2;
        steeringInputAction2.canceled += GetAngleInput2;

        accelerationInputAction2.performed += GetTorqueInput2;
        accelerationInputAction2.canceled += GetTorqueInput2;
    }

    void GetHandBrakeInput2(InputAction.CallbackContext context)
    {
        handBrake2 = context.ReadValue<float>() * brakeTorque2;
    }

    void GetAngleInput2(InputAction.CallbackContext context)
    {
        angle2 = context.ReadValue<float>() * maxAngle2;
    }
    void GetTorqueInput2(InputAction.CallbackContext context)
    {
        torque2 = context.ReadValue<float>() * maxTorque2;
    }

    private void OnEnable()
    {
        handBrakeInputAction2.Enable();
        steeringInputAction2.Enable();
        accelerationInputAction2.Enable();
    }

    private void OnDisable()
    {
        handBrakeInputAction2.Disable();
        steeringInputAction2.Disable();
        accelerationInputAction2.Disable();
    }

    void Start()
    {
        m_Wheels2 = GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < m_Wheels2.Length; i++)
        {
            var wheel = m_Wheels2[i];

            if (wheelShape2 != null)
            {
                var ws = Instantiate(wheelShape2);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    void Update()
    {
        m_Wheels2[0].ConfigureVehicleSubsteps(criticalSpeed2, stepBelow2, stepAbove2);

        foreach (WheelCollider wheel in m_Wheels2)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = angle2;
            }
            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake2;
            }
            if (wheel.transform.localPosition.z < 0 && driveType2 != DriveType2.FrontWheelDrive)
            {
                wheel.motorTorque = torque2;
            }
            if (wheel.transform.localPosition.z > 0 && driveType2 != DriveType2.RearWheelDrive)
            {
                wheel.motorTorque = torque2;
            }
            if (wheelShape2)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                Transform shapeTransform = wheel.transform.GetChild(0);

                if (wheel.name == "b0l" || wheel.name == "b1l")
                {
                    shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                    shapeTransform.position = p;
                }
                else
                {
                    shapeTransform.rotation = q;
                    shapeTransform.position = p;
                }
            }
        }
    }
}
