using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByIndex : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Load(int index)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(index);
    }

    public void LoadScene(int index)
    {
        StartCoroutine(Load(index));
    }

    public void LoadStageMenu(float time)
    {
        Invoke("LoadStageMenuScene", time);
    }

    private void LoadStageMenuScene()
    {
        SceneManager.LoadScene(4);
    }
}
