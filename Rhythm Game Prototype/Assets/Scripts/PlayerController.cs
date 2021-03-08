using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    private RhythmGamePlayerActions controls;
    private PlayerAction pa;
    private bool charging;

    private void OnEnable()
    {
        controls = new RhythmGamePlayerActions();
        controls.Enable();
        
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        pa = GetComponent<PlayerAction>();
        controls.Player.Attack.performed += _ => Punch();
        controls.Player.HAttack.performed += _ => Charging();
        controls.Player.HRelease.performed += _ => Launch();
    }

    private void Punch()
    {
        pa.Punching();
    }

    private void Charging()
    {
        Debug.Log("charging");
        charging = true;
    }
    private void Launch()
    {
        if(charging)
            pa.HoldPunch();
        charging = false;

    }

    private void Update()
    {
        pa.CheckForDrop();
    }
}
