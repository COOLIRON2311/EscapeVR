using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_DoorScript : MonoBehaviour
{
    [Tooltip("If it is false door can't be used")]
    public bool Locked = false;
    [Tooltip("It is true for remote control only")]
    public bool Remote = false;
    [Space]
    [Tooltip("Door can be opened")]
    public bool CanOpen = true;
    [Tooltip("Door can be closed")]
    public bool CanClose = true;
    [Space]
    [Tooltip("Door locked by red key (use key script to declarate any object as key)")]
    public bool RedLocked = false;
    public bool BlueLocked = false;
    [Tooltip("It is used for key script working")]
    [Space]
    public bool isOpened = false;
    [Range(0f, 4f)]
    [Tooltip("Speed for door opening, degrees per sec")]
    public float OpenSpeed = 3f;

    // Hinge
    [HideInInspector]
    public Rigidbody rbDoor;
    HingeJoint hinge;
    JointLimits hingeLim;
    float currentLim;

    void Start()
    {
        rbDoor = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();
    }

    public void TryAction()
    {
        if (!Remote)
            Action();
    }

    public void Action()
    {
        if (Locked)
            return;

        if (isOpened && CanClose)
        {
            isOpened = false;
        }
        else if (!isOpened && CanOpen)
        {
            isOpened = true;
            rbDoor.AddRelativeTorque(new(0, 0, 20f));
        }
    }

    public void Lock() => Locked = true;

    public void Unlock() => Locked = false;

    private void FixedUpdate()
    {
        if (isOpened)
        {
            currentLim = 85f;
        }
        else
        {
            if (currentLim > 1f)
                currentLim -= 0.5f * OpenSpeed;
        }

        hingeLim.max = currentLim;
        hingeLim.min = -currentLim;
        hinge.limits = hingeLim;
    }
}
