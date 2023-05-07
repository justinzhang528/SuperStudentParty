using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public float speed = 2.0f;
    public bool down, left, right;
    private float randomDegree = 0.0f;
    public GameObject hitEffect;
    public GameObject damageAudio;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (down)
        {
            transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
        }
        if (left)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (right)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
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
                    GameManagerCollectCoin.gameManager.DecreaseCoin(0);
                    break;
                case "Player2":
                    GameManagerCollectCoin.gameManager.DecreaseCoin(1);
                    break;
                case "Player3":
                    GameManagerCollectCoin.gameManager.DecreaseCoin(2);
                    break;
                case "Player4":
                    GameManagerCollectCoin.gameManager.DecreaseCoin(3);
                    break;
                default:
                    break;
            }
        }
    }
}
