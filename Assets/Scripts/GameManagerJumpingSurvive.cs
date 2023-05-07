using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerJumpingSurvive : MonoBehaviour
{
    public enum State { Gaming, End};
    public State state = State.Gaming;
    public int playerCount = 2;
    public int life = 3;
    private int playerLeftCount;
    public static GameManagerJumpingSurvive gameManager;
    public GameObject victoryEffect;
    public GameObject wheel;
    private GameObject[] players = new GameObject[4];
    private int[] playerLife = new int[4];
    public Text[] lifeText = new Text[4];
    public Text timeText;
    public Text endingText;
    public int intervalTime;
    public AudioSource backGroundMusic;
    public AudioSource winMusic;
    public AudioSource drawMusic;
    public AudioSource beachMusic;
    public AudioSource fireworkAudio;
    public AudioSource readyGo;
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
        playerLeftCount = playerCount = UIManager.playerCount;
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
            players[i].transform.position = new Vector3(-4.5f + (i * 1.5f), 0.5f, players[i].transform.position.z);
        }
        for (int i = 0; i < playerCount; i ++)
        {
            GameObject child = GameObject.Find("Canvas").transform.GetChild(i + 2).gameObject;
            child.SetActive(true);
            playerLife[i] = life;
        }
        intervalTime = 2;
        SetLifeText();
        GetComponent<GameManagerJumpingSurvive>().enabled = false;
        for (int i = 4; i >= 1; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countDownText.text = "Ready Go!";
        yield return new WaitForSeconds(2f);
        countDownText.enabled = false;
        GetComponent<GameManagerJumpingSurvive>().enabled = true;
        backGroundMusic.Play();
        for (int i = 0; i < playerCount; i++)
            players[i].GetComponent<PlayerMovement>().enabled = true;
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

        for (int i = 0; i < playerCount; i++)
        {
            if (playerLife[i] <= 0 && playerLife[i] >= -10)
            {
                playerLife[i] = -99; 
                playerLeftCount--;
                players[i].GetComponent<PlayerMovement>().Die();
            }
        }
        int interval = intervalTime;
        if (timer > 40)
        {
            interval = intervalTime / 2;
        }
        timer += Time.deltaTime;
        if ((int)timer % interval == 0 && (int)timer != previousTime)
        {
            previousTime = (int)timer;
            intervalTime = Random.Range(2, 6);
            GameObject gameObject = Instantiate(wheel, new Vector3(-16, 0.8f, -16.5f), Quaternion.identity);
            gameObject.transform.Rotate(new Vector3(0, -90, 0));
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
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(gameObject);
        yield return new WaitForSeconds(3);
        GameObject camera = GameObject.Find("Main Camera");
        for (int i = 1; i < 6; i++)
        {
            GameObject child = GameObject.Find("Canvas").transform.GetChild(i).gameObject;
            child.SetActive(false);
        }
        GameObject child0 = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        child0.SetActive(true);
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
                    camera.transform.position = new Vector3(players[i].transform.position.x, 2.3f, players[i].transform.position.z - 5f);
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
        beachMusic.Pause();
        for (int i = 0; i < playerCount; i++)
        {
            players[i].GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPause = false;
        backGroundMusic.Play();
        beachMusic.Play();
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
