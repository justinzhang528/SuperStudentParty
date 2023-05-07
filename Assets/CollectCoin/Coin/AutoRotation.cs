using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    public float speed = 100.0f;
    public GameObject coinAudio;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -50, 0);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 700);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2" || collision.gameObject.tag == "Player3" || collision.gameObject.tag == "Player4")
        {
            switch(collision.gameObject.tag)
            {
                case "Player1":
                    GameManagerCollectCoin.gameManager.IncreaseCoin(0);
                    break;
                case "Player2":
                    GameManagerCollectCoin.gameManager.IncreaseCoin(1);
                    break;
                case "Player3":
                    GameManagerCollectCoin.gameManager.IncreaseCoin(2);
                    break;
                case "Player4":
                    GameManagerCollectCoin.gameManager.IncreaseCoin(3);
                    break;
                default:
                    break;
            }
            Instantiate(coinAudio, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
