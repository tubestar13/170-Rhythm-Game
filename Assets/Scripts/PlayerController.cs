using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerAction pa;

    // Start is called before the first frame update
    void Start()
    {
        pa = GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return; //no keyboard

        PlayerAction.combo = pa.CheckForDrop(PlayerAction.combo);

        //Combo Combat
        if (keyboard.zKey.wasPressedThisFrame)
        {
            PlayerAction.combo = pa.Punching(PlayerAction.combo);
        }

        if (keyboard.zKey.wasReleasedThisFrame)
        {
            PlayerAction.combo = pa.HoldPunch(PlayerAction.combo);
        }
    }
}
