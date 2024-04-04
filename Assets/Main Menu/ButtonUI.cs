using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonUI : MonoBehaviour
{
    public GameObject Player;
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (sceneName == "Sample Scene")
        {
            Health healthComponent = Player.GetComponent<Health>();
            healthComponent.Awake();
        }
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has Quit.");
    }
}
