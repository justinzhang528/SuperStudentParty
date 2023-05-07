using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputDelay : MonoBehaviour
{
    public float delayTime;
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = false;
        yield return new WaitForSeconds(delayTime);
        playerMovement.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
