using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    private Rigidbody rockRb;
    [SerializeField]
    private float rollSpeed;
    private Roll roll;

    private int a = 0;

    IEnumerator Start()
    {
        roll = GetComponent<Roll>();
        roll.enabled = false;
        yield return new WaitForSeconds(6.5f);
        roll.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RockRoll();
    }

    private void RockRoll()
    {
        transform.position += new Vector3(rollSpeed * Time.deltaTime, 0, 0);
        Quaternion target = Quaternion.Euler(0, 0, -a);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f);

        if (Time.time % 5 == 0)
        {
            rollSpeed += 0.3f;
        }
        a += 5;
        if (a >= 360)
        {
            a = 0;
        }
    }
}
