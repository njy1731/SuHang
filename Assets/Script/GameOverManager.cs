using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    void Start()
    {
        
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Main");
    }
}
