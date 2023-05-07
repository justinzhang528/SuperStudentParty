using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerCollectCoin : MonoBehaviour
{
    public enum State { Gaming, End };
    public State state = State.Gaming;
    public static GameManagerCollectCoin gameManager;
    public int playerCount = 2;
    public int gamingTime = 60;
    public GameObject coinModel;
    public GameObject victoryEffect;
    public GameObject enemy;
    private GameObject[] enemies = new GameObject[2];
    private GameObject[] players = new GameObject[4];
    public Text[] coinsText = new Text[4];
    private int[] playerCoins = new int[4] { 0, 0, 0, 0 };
    public Text timeText;
    public Text endingText;
    public AudioSource backgroundMusic;
    public AudioSource winMusic;
    public AudioSource fireworkAudio;
    public AudioSource readyGo;
    private float timer = 0.0f;
    private float timing = 0.0f;
    private float randomTime = 0.0f;
    private int randomQuantity = 0;
    public GameObject canvas, timeUpCanvas;
    public Text countDownText;
    private bool isPause, isOnClick = false;

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
        readyGo.Play();
        playerCount = UIManager.playerCount;
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
            players[i].transform.position = new Vector3(-4 + (i * 3), 0.5f, 0);
        }
        randomTime = Random.Range(4.0f, 8.0f);
        randomQuantity = Random.Range(3, 10);
        for (int i = 0; i < playerCount; i++)
        {
            GameObject child = GameObject.Find("Canvas").transform.GetChild(i + 2).gameObject;
            child.SetActive(true);
        }
        GetComponent<GameManagerCollectCoin>().enabled = false;
        for (int i = 4; i >= 1; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countDownText.text = "Ready Go!";
        yield return new WaitForSeconds(2f);
        countDownText.enabled = false;
        GetComponent<GameManagerCollectCoin>().enabled = true;
        backgroundMusic.Play();
        for (int i = 0; i < playerCount; i++)
            players[i].GetComponent<PlayerMovement>().enabled = true;
        enemies[0] = Instantiate(enemy, new Vector3(-3, 5, -1), Quaternion.identity);
        enemies[1] = Instantiate(enemy, new Vector3(3, 5, -1), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Gaming)
        {
            ReloadScene();
            backgroundMusic.Stop();
            return;
        }

        if (Input.GetButtonDown("Submit") && !isPause && !isOnClick)
            PauseGame();
        isOnClick = false;

        if (timing >= gamingTime)
        {
            StartCoroutine(EndGame());
        }

        timer += Time.deltaTime;
        timing += Time.deltaTime;
        if (timer > randomTime)
        {
            for (int i = 0; i < randomQuantity; i++)
            {
                Instantiate(coinModel, new Vector3(Random.Range(-6.0f, 6.2f), 0.8f, Random.Range(-4.0f, 4.2f)), Quaternion.identity);
            }
            timer = 0;
            randomTime = Random.Range(4.0f, 8.0f);
            randomQuantity = Random.Range(3, 10);
        }
        SetCoinText();
        timeText.text = ((int)timing).ToString();
    }

    IEnumerator EndGame()
    {
        state = State.End;
        timeUpCanvas.SetActive(true);
        for (int i = 0; i < GameObject.Find("Players").transform.childCount; i++)
        {
            GameObject.Find("Players").transform.GetChild(i).GetComponent<Animator>().SetBool("walking", false);
            GameObject.Find("Players").transform.GetChild(i).GetComponent<PlayerMovement>().enabled = false;
        }
        for (int i = 0; i < 2; i++)
            Destroy(enemies[i]);
        yield return new WaitForSeconds(3);        
        timeUpCanvas.SetActive(false);
        GameObject camera = GameObject.Find("Main Camera");
        for (int i = 1; i < 6; i++)
        {
            GameObject child = GameObject.Find("Canvas").transform.GetChild(i).gameObject;
            child.SetActive(false);
        }
        GameObject child0 = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        child0.SetActive(true);
        int winningNumber = 0;
        int maxCoins = playerCoins[0];
        for (int i = 0; i < playerCount - 1; i++)
        {
            if (playerCoins[i] < playerCoins[i + 1])
            {
                maxCoins = playerCoins[i + 1];
                winningNumber = i + 1;
            }
            players[i].GetComponent<PlayerMovement>().End();
        }
        for (int i = 0; i < playerCount; i++)
        {
            if (playerCoins[i] == maxCoins)
            {
                players[i].GetComponent<PlayerMovement>().Win();
                endingText.text += (i + 1) + ".";
                UIManager.winCount[i]++;
            }
            else
            {
                Destroy(players[i]);
            }
        }
        winMusic.Play();
        fireworkAudio.Play();
        camera.transform.position = new Vector3(players[winningNumber].transform.position.x, players[winningNumber].transform.position.y + 2.0f, players[winningNumber].transform.position.z - 5f);
        camera.transform.rotation = Quaternion.Euler(10, 0, 0);
        Instantiate(victoryEffect, new Vector3(players[winningNumber].transform.position.x, players[winningNumber].transform.position.y + 4, players[winningNumber].transform.position.z + 2), Quaternion.Euler(90, 0, 0));
        for (int i = 0; i < 2; i++)
            Destroy(enemies[i]);
    }

    public void DecreaseCoin(int playerNumber)
    {
        if (playerCoins[playerNumber] >= 2)
        {
            playerCoins[playerNumber] -= 2;
        }
        else if (playerCoins[playerNumber] == 1)
        {
            playerCoins[playerNumber] --;
        }
    }

    public void IncreaseCoin(int playerNumber)
    {
        playerCoins[playerNumber]++;
    }

    private void Reload()
    {
        SceneManager.LoadScene(4);
    }

    public void ReloadScene()
    {
        Invoke("Reload", 9f);
    }

    public float GetTimer()
    {
        return timer;
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
            Debug.Log("Detected key code: " + e.keyCode);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        canvas.SetActive(true);
        isPause = true;
        isOnClick = false;
        backgroundMusic.Pause();
        for (int i = 0; i < playerCount; i++)
        {
            players[i].GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPause = false;
        backgroundMusic.Play();
        for (int i = 0; i < playerCount; i++)
        {
            players[i].GetComponent<PlayerMovement>().enabled = true;
        }
        Invoke("SetCanvasActiveFalse", 0.6f);
    }

    public void OnClick()
    {
        isOnClick = true;
    }

    void SetCanvasActiveFalse()
    {
        canvas.SetActive(false);
    }

    void SetCoinText()
    {
        for (int i = 0; i < playerCount; i++)
        {
            coinsText[i].text = playerCoins[i].ToString();
        }
    }
}
