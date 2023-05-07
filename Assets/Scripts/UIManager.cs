using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static int playerCount = 2;
    public static int[] winCount = new int[4] { 0, 0, 0, 0 };
    public static List<string> playerNames = new List<string>();
    public static List<string> joystickNumbers = new List<string>();
    private string[] CharacterNames = new string[7] { "Cat", "Dog", "Turtle", "Owl", "Mario", "Gumba", "Bear" };
    private string pressedKey = "";
    private string controllerNumber = "";
    //public Text controllerText;
    private float timer;
    private static int clickCharactersCount = 1;
    bool isDisplay = false;
    bool isEnableSelect = true;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        KeyPressHandler();
        SetControllerNumber();
        timer += Time.deltaTime;
    }

    private void KeyPressHandler()
    {
        for (int action = (int)KeyCode.Backspace; action <= (int)KeyCode.Joystick8Button19; action++)
        {
            if (Input.GetKeyDown((KeyCode)action) && ((KeyCode)action).ToString().Contains("Joystick"))
                pressedKey = "joystick";
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            pressedKey = "keyboard";
            Debug.Log("pressedKey: " + pressedKey);
        }
    }

    private void SetControllerNumber()
    {
        for (int action = (int)KeyCode.Backspace; action <= (int)KeyCode.Joystick8Button19; action++)
        {
            if (Input.GetKeyDown((KeyCode)action) && ((KeyCode)action).ToString().Contains("Joystick"))
            {
                controllerNumber = ((KeyCode)action).ToString().Substring(8, 1);
                timer = 0;
                isDisplay = true;
            }
        }
        /*if (timer < 3 && isDisplay)
        {
            controllerText.text = "JoyStick " + controllerNumber + " is pressed!";
        }
        else
        {
            controllerText.text = "";
        }*/
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetPlayerCount(int count)
    {
        joystickNumbers.Clear();
        playerNames.Clear();
        clickCharactersCount = 1;
        playerCount = count;
    }

    public void AddPlayersToList(string name)
    {
        if (playerNames.Count >= playerCount)
            return;
        controllerNumber = clickCharactersCount.ToString();
        SetControllerNumber();
        isEnableSelect = true;
        for (int i = 0; i < joystickNumbers.Count; i++)
        {
            if (joystickNumbers[i].ToString() == controllerNumber)
            {
                isEnableSelect = false;
            }
        }
        if (isEnableSelect)
        {
            playerNames.Add(name);
            joystickNumbers.Add(controllerNumber);
            Debug.Log("joystick:" + controllerNumber + "is added");
            clickCharactersCount++;
        }
        else
        {
            Debug.Log("joystick:" + controllerNumber + "is already exist");
        }
    }

    public void InitializedPlayers()
    {
        if (playerCount > joystickNumbers.Count)
        {
            int difference = playerCount - joystickNumbers.Count;
            for (int i = 0; i < difference; i ++)
            {
                for (int j = 1; j < 5; j ++)
                {
                    bool isExist = false;
                    isExist = IsJoystickNumberExist(j);
                    if (!isExist)
                    {
                        joystickNumbers.Add(j.ToString());
                        break;
                    }
                }
                for (int j = 0; j < 7; j++)
                {
                    bool isExist = false;
                    isExist = IsPlayerNameExist(CharacterNames[j]);
                    if (!isExist)
                    {
                        playerNames.Add(CharacterNames[j]);
                        break;
                    }
                }
            }
        }
    }

    private bool IsJoystickNumberExist(int number)
    {
        for (int i = 0; i < joystickNumbers.Count; i++)
        {
            if (number.ToString() == joystickNumbers[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPlayerNameExist(string name)
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            if (name == playerNames[i])
            {
                return true;
            }
        }
        return false;
    }

    public void ShowPlayers()
    {
        for (int i=0;i<playerCount;i++)
        {
            Debug.Log(joystickNumbers[i]+" . "+playerNames[i]);
        }
    }

    public void ResetWinningCount()
    {
        for (int i = 0; i < 4; i++)
            winCount[i] = 0;
    }
}
