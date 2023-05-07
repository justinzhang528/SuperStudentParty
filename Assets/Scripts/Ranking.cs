using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public GameObject[] players = new GameObject[4];
    public GameObject[] playerImages = new GameObject[4];
    public GameObject pooh;
    public GameObject[] player3DModelCamera = new GameObject[4];
    public Text[] winCount = new Text[4];
    private string[] characterNames = new string[7] { "Cat", "Dog", "Turtle", "Owl", "Mario", "Gumba", "Bear" };
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < UIManager.playerNames.Count; i ++)
        {
            players[i].SetActive(true);
            winCount[i].text = UIManager.winCount[i].ToString();
            for (int j = 0; j < 7; j++)
            {
                if (UIManager.playerNames[i] == characterNames[j])
                {
                    player3DModelCamera[i].transform.Find(UIManager.playerNames[i]).gameObject.SetActive(true);
                }
            }
        }
        if (UIManager.playerNames.Count > 0)
            pooh.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
