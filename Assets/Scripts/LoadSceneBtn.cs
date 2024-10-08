using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneBtn : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}