using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class AN_Button : MonoBehaviour
{
    public bool isGripped = false;
    [Tooltip("True for rotation like valve (used for ramp/elevator only)")]
    public bool isValve = false;
    [Tooltip("SelfRotation speed of valve")]
    public float ValveSpeed = 10f;
    [Tooltip("If it isn't valve, it can be lever or button (animated)")]
    public bool isLever = false;
    [Tooltip("If it is false door can't be used")]
    public bool Locked = false;
    [Tooltip("The door for remote control")]
    public AN_DoorScript DoorObject;
    [Space]
    [Tooltip("Any object for ramp/elevator baheviour")]
    public Transform RampObject;
    [Tooltip("Door can be opened")]
    public bool CanOpen = true;
    [Tooltip("Door can be closed")]
    public bool CanClose = true;
    [Tooltip("Current status of the door")]
    public bool isOpened = false;
    [Space]
    [Tooltip("True for rotation by X local rotation by valve")]
    public bool xRotation = true;
    [Tooltip("True for vertical movenment by valve (if xRotation is false)")]
    public bool yPosition = false;
    public float max = 90f, min = 0f, speed = 5f;
    bool valveBool = true;
    float current, startYPosition;
    Quaternion rampQuat;
    XRKnob knob;

    void Start()
    {
        if (isValve)
        {
            knob = GetComponent<XRKnob>();
        }
        if (RampObject == null)
            return;

        startYPosition = RampObject.position.y;
        rampQuat = RampObject.rotation;
    }

    void Update()
    {
        if (isValve && !isGripped)
        {
            if (!isOpened && knob.value > 0)
            {
                knob.value -= ValveSpeed * Time.deltaTime;
            }
            if (isOpened && knob.value < 1)
            {
                knob.value += ValveSpeed * Time.deltaTime;
            }
        }
    }

    public void TryAction()
    {
        if (Locked || isValve || DoorObject == null || DoorObject.Remote)
            return;
        DoorObject.Action();
    }

    public void UpdateRamp(float value)
    {
        if (Locked || !isValve || RampObject == null || knob == null)
            return;

        if (knob.value >= 1)
        {
            isOpened = true;
        }
        else if (knob.value < 0)
        { 
            isOpened = false; 
        }

        if (xRotation)
        {
            RampObject.rotation = rampQuat * Quaternion.Euler(knob.value * max, 0f, 0f);
        }
        else if (yPosition)
        {
            RampObject.position = new Vector3(RampObject.position.x, startYPosition + knob.value * max, RampObject.position.z);
        }
    }
}
