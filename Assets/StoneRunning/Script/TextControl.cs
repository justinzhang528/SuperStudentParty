using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    TextMesh textMesh;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        textMesh = GetComponent<TextMesh>();
        for(int i = 5; i >= 1; i--)
        {
            textMesh.text = i.ToString();
            yield return new WaitForSeconds(1.2f);
        }
        textMesh.text = "Run!";
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
