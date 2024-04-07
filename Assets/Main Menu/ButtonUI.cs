using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public void GoToScene(string sceneName)
    {
        audioSource.PlayOneShot(audioClip);
        // Delay the scene change
        StartCoroutine(DelayedSceneChange(sceneName));
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has Quit.");
    }

    IEnumerator DelayedSceneChange(string sceneName)
    {
        // Wait for the length of the audio clip
        yield return new WaitForSeconds(audioClip.length);

        // Change scene
        SceneManager.LoadScene(sceneName);
    }
}
