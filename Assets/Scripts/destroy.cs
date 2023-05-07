using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    public GameObject explosure, destroyAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Foot")
        {
            Instantiate(explosure, gameObject.transform.position, Quaternion.identity);
            Instantiate(destroyAudio, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Physics.gravity = new Vector3(0, -30, 0);
            other.gameObject.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * 300);
        }
    }
}
