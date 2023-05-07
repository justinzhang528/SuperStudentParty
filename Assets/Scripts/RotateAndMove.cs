using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndMove : MonoBehaviour
{
    private float rotateSpeed = 15.0f;
    private float moveSpeed;
    public GameObject hitEffect;
    public GameObject damageAudio;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(-rotateSpeed, 0, 0));
        this.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            Instantiate(hitEffect, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z), Quaternion.identity);
            Instantiate(damageAudio, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z), Quaternion.identity);
            other.gameObject.GetComponent<Animator>().SetTrigger("damage");
            switch (other.gameObject.tag)
            {
                case "Player1":
                    GameManagerJumpingSurvive.gameManager.ReduceLife(0);
                    break;
                case "Player2":
                    GameManagerJumpingSurvive.gameManager.ReduceLife(1);
                    break;
                case "Player3":
                    GameManagerJumpingSurvive.gameManager.ReduceLife(2);
                    break;
                case "Player4":
                    GameManagerJumpingSurvive.gameManager.ReduceLife(3);
                    break;
                default:
                    break;
            }
        }
    }
}
