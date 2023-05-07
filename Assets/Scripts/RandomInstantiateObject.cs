using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstantiateObject : MonoBehaviour
{
    public GameObject instantiateObject;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0,2) == 1)
        {
            Debug.Log("Mummy is Comming");
            Instantiate(instantiateObject, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
