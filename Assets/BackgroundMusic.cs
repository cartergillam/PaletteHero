using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private void Awake()
    {
        // Ensure there's only one instance of BackgroundMusic across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Register for sceneLoaded event when this object is enabled
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister from sceneLoaded event when this object is disabled
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is a game scene
        if (scene.name != "Main Menu" && scene.name != "Instructions" && scene.name != "Difficulty")
        {
            // Find the background music GameObject and stop the music
            Destroy(gameObject);
        }
    }
}
