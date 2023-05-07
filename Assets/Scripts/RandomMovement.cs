using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private float time = 0.0f;
    private float randomTime = 0.0f;
    public float speed = 2.0f;
    public float xMax, xMin, zMax, zMin;

    private void Start()
    {
        randomTime = Random.Range(0.5f, 4.0f);
    }

    private void Update()
    {
        time += Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.position = new Vector3 (Mathf.Clamp(transform.position.x, xMin, xMax), 6.0f, Mathf.Clamp(transform.position.z, zMin, zMax));
        //transform.position += speed * Vector3.forward * Time.deltaTime;
        if (time > randomTime)
        {
            transform.localRotation = Quaternion.Euler(0, Random.rotation.y * 1000 , 0);           
            time = 0.0f;
            randomTime = randomTime = Random.Range(1.0f, 5.0f);
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Border")
        {
            Debug.Log("Touch Border");
            transform.localRotation = Quaternion.Euler(0, (transform.rotation.y * 100) + 180, 0);
        }
    }
}
