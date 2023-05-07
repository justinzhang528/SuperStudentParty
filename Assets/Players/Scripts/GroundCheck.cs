using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag.Contains("Color"))
        {
            transform.parent.GetComponent<Animator>().SetBool("isGrounded", true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag.Contains("Color"))
        {
            transform.parent.GetComponent<Animator>().SetBool("isGrounded", false);
            transform.parent.GetComponent<Animator>().SetTrigger("jump");
        }
    }
}
