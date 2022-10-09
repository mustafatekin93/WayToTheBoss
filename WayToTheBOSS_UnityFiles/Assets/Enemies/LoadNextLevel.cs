using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("gameScene");
    }
}
