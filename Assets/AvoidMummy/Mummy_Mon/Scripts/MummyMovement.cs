using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyMovement : MonoBehaviour
{
    public float speed = 0.0f;
    private float rotateDegree = 0.0f;
    public bool forward, back, right, left;
    private Vector3 direction;
    private float timer = 0.0f;
    public GameObject hitEffect, damageAudio;
    // Start is called before the first frame update
    void Start()
    {
        if (forward)
        {
            direction = Vector3.forward;
            rotateDegree = 0.0f;
        }
        if (back)
        {
            direction = Vector3.back;
            rotateDegree = 180.0f;
        }
        if (right)
        {
            direction = Vector3.right;
            rotateDegree = 90.0f;
        }
        if (left)
        {
            direction = Vector3.left;
            rotateDegree = -90.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, rotateDegree, 0);
        transform.Translate(direction * ((speed * Time.deltaTime) + (GameManagerAvoidMummy.gameManager.GetTimer() * 0.002f)), Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            Instantiate(hitEffect, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z), Quaternion.identity);
            Instantiate(damageAudio, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z), Quaternion.identity);
            other.gameObject.GetComponent<Animator>().SetTrigger("damage");
            switch (other.gameObject.tag)
            {
                case "Player1":
                    GameManagerAvoidMummy.gameManager.ReduceLife(0);
                    break;
                case "Player2":
                    GameManagerAvoidMummy.gameManager.ReduceLife(1);
                    break;
                case "Player3":
                    GameManagerAvoidMummy.gameManager.ReduceLife(2);
                    break;
                case "Player4":
                    GameManagerAvoidMummy.gameManager.ReduceLife(3);
                    break;
                default:
                    break;
            }
        }
    }
}
