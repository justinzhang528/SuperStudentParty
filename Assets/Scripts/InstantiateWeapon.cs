using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateWeapon : MonoBehaviour
{
    public GameObject weapon, shootAudio;
    private float timer = 0.0f;
    private float randomTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        randomTime = randomTime = Random.Range(3.0f, 6.0f);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            Instantiate(weapon, transform.position, Quaternion.identity);
            Instantiate(shootAudio, transform.position, Quaternion.identity);
            timer = 0;
            randomTime = Random.Range(3.0f, 6.0f);
        }
    }
}
