using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputController : MonoBehaviour
{
    public bool isLeftArrowPressed { get; private set; }
    public bool isRightArrowPressed { get; private set; }
    public bool isDownArrowPressed { get; private set; }
    public bool isUpArrowPressed { get; private set; }
    public bool isWeaponUpPressd { get; private set; }
    public bool isWeaponDownPressd { get; private set; }
    
    void Update()
    {

        isLeftArrowPressed = Input.GetKey(KeyCode.A);
        isRightArrowPressed = Input.GetKey(KeyCode.D);
        isDownArrowPressed = Input.GetKey(KeyCode.S);
        isUpArrowPressed = Input.GetKey(KeyCode.W);

        isWeaponUpPressd = Input.GetKeyDown(KeyCode.E);
        isWeaponDownPressd = Input.GetKeyDown(KeyCode.Q);
    }


}
