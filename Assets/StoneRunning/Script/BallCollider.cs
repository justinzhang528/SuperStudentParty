using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            switch (other.gameObject.tag)
            {
                case "Player1":
                    GameManagerStoneRunning.gameManager.ReduceLife(0);
                    break;
                case "Player2":
                    GameManagerStoneRunning.gameManager.ReduceLife(1);
                    break;
                case "Player3":
                    GameManagerStoneRunning.gameManager.ReduceLife(2);
                    break;
                case "Player4":
                    GameManagerStoneRunning.gameManager.ReduceLife(3);
                    break;
                default:
                    break;
            }
            other.gameObject.GetComponent<PlayerMovement>().Die();
            GameManagerStoneRunning.gameManager.ReducePlayerLeftCount();
        }
    }
}
