using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItSelf : MonoBehaviour
{
    public float time = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }
}
