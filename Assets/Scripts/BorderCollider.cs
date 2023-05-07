using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AI" || collision.gameObject.tag == "Player")
        {
            Debug.Log("Touch Border");
        }
    }
}
