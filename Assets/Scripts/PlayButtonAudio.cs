using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonAudio : MonoBehaviour
{
    public AudioSource submit;
    public AudioSource move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySubmitAudio()
    {
        submit.Play();
    }

    public void PlayMoveAudio()
    {
        move.Play();
    }
}
