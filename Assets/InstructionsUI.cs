using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsUI : MonoBehaviour
{
    private Animator anim;
    private GameObject LeftArrow;
    private GameObject Instructions;
    private GameObject RightArrow;
    private int slide;

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
            if (slide < 8)
                slide++;
        }

        UpdateSlideAnimation();
        UpdateArrowVisibility();
    }

    private void UpdateSlideAnimation()
    {
        for (int i = 1; i <= 8; i++)
        {
            anim.SetBool("slide" + i, i == slide);
        }
    }

    private void UpdateArrowVisibility()
    {
        LeftArrow.SetActive(slide > 1);
        RightArrow.SetActive(slide < 8);
    }

    public void MainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
