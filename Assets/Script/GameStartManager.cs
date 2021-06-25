using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    void Start()
    {
        
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Main");
    }
}
