using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionsUI : MonoBehaviour
{
    private Animator anim;
    private GameObject LeftArrow;
    private GameObject Instructions;
    private GameObject RightArrow;
    private int slide;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        LeftArrow = GameObject.Find("LeftArrow");
        RightArrow = GameObject.Find("RightArrow");
        Instructions = GameObject.Find("Instructions");
        anim = Instructions.GetComponent<Animator>();
        slide = 1;
        UpdateArrowVisibility();
    }

    public void ChangeSlide(string direction)
    {
        char arrow = direction[0];
        if (arrow == 'l')
        {
            if (slide > 1)
                slide--;
        }
        else
        {
            if (slide < 9)
                slide++;
        }

        UpdateSlideAnimation();
        UpdateArrowVisibility();
    }

    private void UpdateSlideAnimation()
    {
        for (int i = 1; i <= 9; i++)
        {
            anim.SetBool("slide" + i, i == slide);
        }
    }

    private void UpdateArrowVisibility()
    {
        LeftArrow.SetActive(slide > 1);
        RightArrow.SetActive(slide < 9);
    }

    public void MainMenu(string sceneName)
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
