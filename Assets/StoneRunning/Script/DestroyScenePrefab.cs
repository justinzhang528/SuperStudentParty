using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScenePrefab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent.position.x > transform.position.x)
        {
            transform.position += new Vector3(70, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(this.transform.parent.gameObject, 1f);
    }
}
