using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyUI : MonoBehaviour
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
}
