using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    private string controllerNumber = "";
    private bool isEnableSelect = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disable()
    {
        SetControllerNumber();
        isEnableSelect = true;
        for (int i = 0; i < UIManager.joystickNumbers.Count; i++)
        {
            if (UIManager.joystickNumbers[i].ToString() == controllerNumber)
            {
                isEnableSelect = false;
            }
        }
        if (UIManager.playerNames.Count < UIManager.playerCount && isEnableSelect)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SetControllerNumber()
    {
        for (int action = (int)KeyCode.Backspace; action <= (int)KeyCode.Joystick8Button19; action++)
        {
            if (Input.GetKeyDown((KeyCode)action) && ((KeyCode)action).ToString().Contains("Joystick"))
            {
                controllerNumber = ((KeyCode)action).ToString().Substring(8, 1);
            }
        }
    }
}
