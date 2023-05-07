using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerStoneRunning : MonoBehaviour
{
    public enum State { Gaming, End };
    public State state = State.Gaming;
    public static GameManagerStoneRunning gameManager;
    public int playerCount = 2;
    private int[] playerLife = new int[4] { 1, 1, 1, 1 };
    private int playerLeftCount;
    public GameObject victoryEffect;    
    private GameObject[] players = new GameObject[4];
    public AudioSource backGroundMusic;
    public AudioSource winMusic;
    public AudioSource drawMusic;
    public AudioSource fireworkAudio;
    private bool isPause, isOnClick = false;
    public GameObject canvas;
    public Text endingText;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        playerLeftCount = playerCount = UIManager.playerCount;
        Transform parent = GameObject.Find("Players").transform;
        for (int i = 0; i < playerCount; i++)
        {
            foreach (Transform child in parent)
            {
                if (child.name == UIManager.playerNames[i])
                {
                    child.gameObject.SetActive(true);
                    child.gameObject.tag = "Player" + (i + 1);
                    players[i] = child.gameObject;
                }
            }
            players[i].transform.position = new Vector3(0, players[i].transform.position.y, -5 + (i * 2));
        }
        GetComponent<GameManagerStoneRunning>().enabled = false;
        yield return new WaitForSeconds(6.5f);
        GetComponent<GameManagerStoneRunning>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (state != State.Gaming)
        {
            ReloadScene();
            backGroundMusic.Stop();
            return;
        }

        if (Input.GetButtonDown("Submit") && !isPause && !isOnClick)
            PauseGame();
        isOnClick = false;

        if (playerLeftCount <= 1)
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        state = State.End;
        for (int i = 0; i < GameObject.Find("Players").transform.childCount; i++)
        {
            GameObject.Find("Players").transform.GetChild(i).GetComponent<Animator>().SetBool("walking",false);
            GameObject.Find("Players").transform.GetChild(i).GetComponent<PlayerMovement>().enabled = false;
        }
        GameObject.Find("Ball").GetComponent<Roll>().enabled = false;
        yield return new WaitForSeconds(3);
        GameObject camera = GameObject.Find("Main Camera");
        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
        Destroy(GameObject.Find("Ball").gameObject);
        if (playerLeftCount == 1)
        {
            for (int i = 0; i < playerCount; i++)
            {
                if (playerLife[i] > 0)
                {
                    winMusic.Play();
                    fireworkAudio.Play();
                    players[i].GetComponent<PlayerMovement>().Win();
                    endingText.text = "Winner : Player" + (i + 1);
                    UIManager.winCount[i]++;
                    camera.GetComponent<CameraMove>().enabled = false;
                    players[i].transform.position = new Vector3(players[i].transform.position.x, players[i].transform.position.y + 1, 9);
                    camera.transform.position = new Vector3(players[i].transform.position.x + 5, 3, players[i].transform.position.z);
                    camera.transform.rotation = Quaternion.Euler(10, -90, 0);
                    Instantiate(victoryEffect, new Vector3(players[i].transform.position.x - 2, 5, players[i].transform.position.z), Quaternion.Euler(90, 0, 0));
                }
            }
        }
        else
        {
            drawMusic.Play();
            endingText.text = "It's a draw!";
        }
    }

    public void ReduceLife(int playerNumber)
    {
        playerLife[playerNumber]--;
    }

    private void Reload()
    {
        SceneManager.LoadScene(4);
    }

    public void ReloadScene()
    {
        Invoke("Reload", 9f);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        canvas.SetActive(true);
        isPause = true;
        isOnClick = false;
        backGroundMusic.Pause();
        for (int i = 0; i < playerCount; i++)
        {
            players[i].GetComponent<PlayerMovement>().enabled = false;
        }
        GameObject.Find("Ball").GetComponent<Roll>().enabled = false;
    }

    public void ResumeGame()
    {
        Invoke("SetCanvasActiveFalse", 0.6f);
        Time.timeScale = 1;
        isPause = false;
        backGroundMusic.Play();
        for (int i = 0; i < playerCount; i++)
        {
            players[i].GetComponent<PlayerMovement>().enabled = true;
        }
        GameObject.Find("Ball").GetComponent<Roll>().enabled = true;
    }

    public void OnClick()
    {
        isOnClick = true;
    }

    void SetCanvasActiveFalse()
    {
        canvas.SetActive(false);
    }

    public void ReducePlayerLeftCount()
    {
        playerLeftCount--;
    }
}
