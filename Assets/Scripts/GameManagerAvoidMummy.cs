using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerAvoidMummy : MonoBehaviour
{
    public enum State { Gaming, End};
    public State state = State.Gaming;
    public int playerCount = 2;
    public int life = 3;
    private int playerLeftCount;
    public static GameManagerAvoidMummy gameManager;
    public GameObject victoryEffect;
    private GameObject[] players = new GameObject[4];
    private int[] playerLife = new int[4];
    public Text[] lifeText = new Text[4];
    public Text timeText;
    public Text endingText;
    public int intervalTime = 4;
    public AudioSource backGroundMusic;
    public AudioSource winMusic;
    public AudioSource drawMusic;
    public AudioSource fireworkAudio;
    public AudioSource readyGo;
    private Vector3 rightPos = new Vector3(-14, 1.3f, -4);
    private Vector3 leftPos = new Vector3(6, 1.3f, -4);
    private Vector3 forwardPos = new Vector3(-4, 1.3f, -14);
    private Vector3 backPos = new Vector3(-4, 1.3f, 6);
    public GameObject rightInstantiatePoint;
    public GameObject leftInstantiatePoint;
    public GameObject forwardInstantiatePoint;
    public GameObject backInstantiatePoint;
    private float timer = 0.0f;
    private int previousTime = 0;
    public GameObject canvas;
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
        for (int i = 0; i < playerCount; i ++)
        {
            foreach (Transform child in parent)
            {
                if (child.name == UIManager.playerNames[i])
                {
                    child.gameObject.SetActive(true);
                    child.gameObject.tag = "Player" + (i+1);
                    players[i] = child.gameObject;
                }
            }
            players[i].transform.position = new Vector3(-7 + (i * 2), 0.5f, -5);
        }
        playerLeftCount = playerCount;
        for (int i = 0; i < playerCount; i ++)
        {
            GameObject child = GameObject.Find("Canvas").transform.GetChild(i + 2).gameObject;
            child.SetActive(true);
            playerLife[i] = life;
        }
        SetLifeText();
        GetComponent<GameManagerAvoidMummy>().enabled = false;
        for (int i = 4; i >= 1; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countDownText.text = "Ready Go!";
        yield return new WaitForSeconds(2f);
        countDownText.enabled = false;
        GetComponent<GameManagerAvoidMummy>().enabled = true;
        backGroundMusic.Play();
        for (int i = 0; i < playerCount; i++)
            players[i].GetComponent<PlayerMovement>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        int interval = intervalTime;
        if (timer > 30)
        {
            interval = intervalTime / 2;
        }
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

        for (int i = 0; i < playerCount; i++)
        {
            if (playerLife[i] <= 0 && playerLife[i] >= -10)
            {
                playerLife[i] = -99; 
                playerLeftCount--;
                players[i].GetComponent<PlayerMovement>().Die();    
            }
        }
        int randomNumber = Random.Range(1,5);
        timer += Time.deltaTime;
        if ((int)timer % interval == 0 && (int)timer != previousTime)
        {
            previousTime = (int)timer;
            switch (randomNumber)
            {
                case 1:
                    Instantiate(rightInstantiatePoint, rightPos, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(leftInstantiatePoint, leftPos, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(forwardInstantiatePoint, forwardPos, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(backInstantiatePoint, backPos, Quaternion.identity);
                    break;
            }
        }
        SetLifeText();
        timeText.text = ((int)timer).ToString();
        Debug.Log("left:" + playerLeftCount);
    }

    IEnumerator EndGame()
    {
        state = State.End;
        for (int i = 0; i < GameObject.Find("Players").transform.childCount; i++)
        {
            GameObject.Find("Players").transform.GetChild(i).GetComponent<Animator>().SetBool("walking", false);
            GameObject.Find("Players").transform.GetChild(i).GetComponent<PlayerMovement>().enabled = false;
        }
        yield return new WaitForSeconds(3);
        GameObject camera = GameObject.Find("Main Camera");
        for (int i = 1; i < 6; i++)
        {
            GameObject child = GameObject.Find("Canvas").transform.GetChild(i).gameObject;
            child.SetActive(false);
        }
        GameObject child0 = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        child0.SetActive(true);
        if (playerLeftCount >= 1)
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
                    camera.transform.position = new Vector3(players[i].transform.position.x, players[i].transform.position.y + 2.0f, players[i].transform.position.z - 5f);
                    camera.transform.rotation = Quaternion.Euler(10, 0, 0);
                    Instantiate(victoryEffect, new Vector3(players[i].transform.position.x, players[i].transform.position.y + 4, players[i].transform.position.z + 2), Quaternion.Euler(90, 0, 0));
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

    public float GetTimer()
    {
        return timer;
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
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gameObject in objs)
        {
            gameObject.GetComponent<MummyMovement>().enabled = false;
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPause = false;
        backGroundMusic.Play();
        for (int i = 0; i < playerCount; i++)
        {
            players[i].GetComponent<PlayerMovement>().enabled = true;
        }
        Invoke("SetCanvasActiveFalse", 0.6f);
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            gameObject.GetComponent<MummyMovement>().enabled = true;
            gameObject.GetComponent<AudioSource>().enabled = true;
        }
    }

    public void OnClick()
    {
        isOnClick = true;
    }

    void SetCanvasActiveFalse()
    {
        canvas.SetActive(false);
    }

    void SetLifeText()
    {
        for (int i = 0; i < playerCount; i++)
        {
            lifeText[i].text = playerLife[i].ToString();
            if (playerLife[i] < 0)
                lifeText[i].text = "0";
        }
    }
}
