using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangingScene : MonoBehaviour
{
    public string sceneName;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
