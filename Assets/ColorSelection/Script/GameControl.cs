using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public Material[] color;
    public MeshRenderer[] blocks;
    bool isCoroutineExecuting = false;
    int index = -1;
    float waitTime = 3f;
    int playerAlive = 2; //這裏抓Player數量
    private int playerCount;
    List<GameObject[]> gameObjects = new List<GameObject[]>();
    public AudioSource dingSoundEffect;
    public GameObject victoryEffect;
    private GameObject[] players = new GameObject[4];
    public AudioSource backGroundMusic;
    public AudioSource winMusic;
    public AudioSource drawMusic;
    public AudioSource fireworkAudio;
    private bool isPause, isOnClick = false;
    public GameObject canvas;
    public Text endingText;

    private enum State
    {
        Start,
        RoundEnd,
        Check,
        End
    }
    private State state;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        state = State.Start;
        for (int i = 0; i < color.Length; i++)
        {
            gameObjects.Add(GameObject.FindGameObjectsWithTag("Color" + (i + 1)));
        }
        playerCount = playerAlive = UIManager.playerCount;
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
            players[i].transform.position = new Vector3(-1 + i, 0.5f + 0);
        }
        yield return new WaitForSeconds(3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCoroutineExecuting)
        {
            switch (state)
            {
                case State.Start:
                    StartCoroutine(ChangeColor());
                    break;
                case State.RoundEnd:
                    StartCoroutine(DestroyBlock());
                    break;
                case State.Check: //player win here
                    AliveNumber();
                    break;
                case State.End:
                    backGroundMusic.Stop();
                    ReloadScene();
                    return;
            }
        }
        if (Input.GetButtonDown("Submit") && !isPause && !isOnClick)
            PauseGame();
        isOnClick = false;
    }

    private IEnumerator ChangeColor()
    {
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(waitTime);
        index = Random.Range(0, 7);
        foreach (var block in blocks)
        {
            block.material = color[index];
        }
        dingSoundEffect.Play();
        yield return new WaitForSeconds(waitTime);
        if(dingSoundEffect.time >= waitTime)
        {
            dingSoundEffect.Stop();
        }
        state = State.RoundEnd;
        isCoroutineExecuting = false;
    }

    private IEnumerator DestroyBlock()
    {
        isCoroutineExecuting = true;
        for (int i = 0; i < color.Length; i++)
        {
            foreach (var target in gameObjects[i])
            {
                if (!target.CompareTag("Color" + (index + 1)))
                    target.SetActive(false);
            }
        }

        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < color.Length; i++)
        {
            foreach (var target in gameObjects[i])
            {
                if (!target.CompareTag("Color" + (index + 1)))
                    target.SetActive(true);
            }
        }
        yield return new WaitForSeconds(1f);
        ResetColor();
    }

    private void ResetColor()
    {
        for (int i = 0; i < color.Length; i++)
        {
            blocks[i].material = color[i];
        }
        isCoroutineExecuting = false;
        state = State.Check;
    }

    private void AliveNumber()
    {
        if(playerAlive > 1)
        {
            state = State.Start;
            if(waitTime >= 1)
            {
                waitTime -= 0.2f;
            }
        }
        else
        {
            GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
            if (playerAlive == 1)
            {
                GameObject parent = GameObject.Find("Players");
                GameObject winner = new GameObject();
                int childCount = GameObject.Find("Players").transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    if (parent.transform.GetChild(i).gameObject.active)
                    {
                        winner = parent.transform.GetChild(i).gameObject;
                        break;
                    }
                }
                string winnerNumber = winner.tag.ToString().Substring(6, 1);
                GameObject camera = GameObject.Find("Main Camera");
                winMusic.Play();
                fireworkAudio.Play();
                winner.GetComponent<PlayerMovement>().Win();
                endingText.text = "Winner : Player" + winnerNumber;
                UIManager.winCount[int.Parse(winnerNumber) - 1]++;
                camera.transform.position = new Vector3(winner.transform.position.x, 3, winner.transform.position.z - 3);
                camera.transform.rotation = Quaternion.Euler(20, 0, 0);
                Instantiate(victoryEffect, new Vector3(winner.transform.position.x, winner.transform.position.y + 4, winner.transform.position.z + 2), Quaternion.Euler(90, 0, 0));
            }
            else
            {
                drawMusic.Play();
                endingText.text = "It's a draw!";
            }
            state = State.End;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            playerAlive -= 1;
            Destroy(other.gameObject);
        }
        Debug.Log("playerAlive: " + playerAlive);
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
    }

    public void OnClick()
    {
        isOnClick = true;
    }

    void SetCanvasActiveFalse()
    {
        canvas.SetActive(false);
    }
}
