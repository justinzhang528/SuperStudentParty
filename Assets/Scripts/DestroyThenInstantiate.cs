using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThenInstantiate : MonoBehaviour
{
    public GameObject weaponLeft, weaponRight;
    private float rotation;
    // Start is called before the first frame update
    void Start()
    {
        rotation = Random.rotation.y * 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0.5f)
        {
            Instantiate(weaponLeft, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.Euler(0, rotation, 0));
            Instantiate(weaponRight, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.Euler(0, rotation, 0));
            Destroy(gameObject);
        }
    }
}
