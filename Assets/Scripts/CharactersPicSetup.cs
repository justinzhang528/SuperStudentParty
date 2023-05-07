using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharactersPicSetup : MonoBehaviour
{
    public GameObject[] players = new GameObject[4];
    public GameObject[] player3DModelCamera = new GameObject[4];
    public GameObject[] playersToEnable = new GameObject[7];
    public GameObject nextButton;
    private string[] characterNames = new string[7] { "Cat", "Dog", "Turtle", "Owl", "Mario", "Gumba", "Bear" };
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < UIManager.playerCount; i ++)
        {
            players[i].SetActive(true);
        }
        RefreshImages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshImages()
    {
        for (int i = 0; i < UIManager.playerNames.Count; i++)
        {
            for (int j = 0; j < 7; j ++)
            {
                if (UIManager.playerNames[i] == characterNames[j])
                {
                    player3DModelCamera[i].transform.Find(UIManager.playerNames[i]).gameObject.SetActive(true);
                    playersToEnable[j].SetActive(false);
                }
            }
        }
        if (UIManager.playerNames.Count == UIManager.playerCount)
        {
            nextButton.SetActive(true);
            for (int i = 0; i < 7; i++)
            {
                playersToEnable[i].GetComponent<Button>().enabled = false;
                playersToEnable[i].GetComponent<EventTrigger>().enabled = false;
            }
        }
    }
}
