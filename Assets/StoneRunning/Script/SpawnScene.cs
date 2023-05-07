using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScene : MonoBehaviour
{
    public GameObject[] scene;
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
        if (other.gameObject.tag.Contains("Player1"))
        {
            int index = Random.Range(0, scene.Length);
            int rotation = Random.Range(0, 2);
            Vector3 nextSceneLocation = new Vector3(transform.parent.position.x + 40, transform.position.y, transform.position.z);
            Quaternion nextSceneRotation = Quaternion.Euler(0, 180 * rotation, 0);
            Instantiate(scene[index], nextSceneLocation, nextSceneRotation);
            Destroy(this.gameObject);
        }
    }
}
