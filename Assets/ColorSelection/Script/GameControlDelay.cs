using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlDelay : MonoBehaviour
{
    private GameControl gameControl;
    private AudioSource audioSource;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        gameControl = GetComponent<GameControl>();
        audioSource = GetComponent<AudioSource>();

        gameControl.enabled = false;
        audioSource.enabled = false;
        yield return new WaitForSeconds(0.5f);
        audioSource.enabled = true;
        yield return new WaitForSeconds(3f);
        gameControl.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
