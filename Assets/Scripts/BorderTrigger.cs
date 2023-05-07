using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AI" || other.gameObject.tag == "Player")
        {
            Debug.Log("Touch Border");
        }
    }
}
