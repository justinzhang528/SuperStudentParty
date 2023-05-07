using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (audioSource.time >= 20.5)
        {
            audioSource.Stop();
            audioSource.Play();
            audioSource.time = 6.1f;
        }
    }
}
