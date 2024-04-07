using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyUI : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public void GoToScene(string sceneName)
    {
        audioSource.PlayOneShot(audioClip);
        // Delay the scene change
        StartCoroutine(DelayedSceneChange(sceneName));
    }
    IEnumerator DelayedSceneChange(string sceneName)
    {
        // Wait for the length of the audio clip
        yield return new WaitForSeconds(audioClip.length);
        // Change scene
        SceneManager.LoadScene(sceneName);
    }
}
