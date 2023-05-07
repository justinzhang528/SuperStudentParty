using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButton()
    {
        GetComponent<Animator>().SetTrigger("Press");
    }

    public void OnSelect()
    {
        GetComponent<Animator>().SetBool("Selected", true);
    }

    public void DeSelect()
    {
        GetComponent<Animator>().SetBool("Selected", false);
    }
}
