using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject ball;
    private CameraMove cameraMove;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        cameraMove = GetComponent<CameraMove>();
        cameraMove.enabled = false;
        yield return new WaitForSeconds(6.0f);
        cameraMove.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(ball.transform.position.x + 10, this.transform.position.y, this.transform.position.z);
    }
}
