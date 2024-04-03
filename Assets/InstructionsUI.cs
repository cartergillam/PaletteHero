using Unity.VisualScripting;
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
        LeftArrow.SetActive(false);
    }

    public void ChangeSlide(string direction)
    {
        char arrow = direction[0];
        if (slide == 1)
        {
            anim.SetBool("two", true);
            anim.SetBool("one", false);
            anim.SetBool("three", false);
            slide = 2;
            LeftArrow.SetActive(true);
            return;
        }
        else if (slide == 2)
        {
            LeftArrow.SetActive(true);
            RightArrow.SetActive(true);
            if (arrow == 'l')
            {
                anim.SetBool("one", true);
                anim.SetBool("two", false);
                anim.SetBool("three", false);
                LeftArrow.SetActive(false);
                slide = 1;
                return;
            }
            else
            {
                anim.SetBool("three", true);
                anim.SetBool("one", false);
                anim.SetBool("two", false);
                RightArrow.SetActive(false);
                slide = 3;
                return;
            }
        }
        else
        {
            RightArrow.SetActive(true);
            if (arrow == 'l')
            {
                anim.SetBool("two", true);
                anim.SetBool("one", false);
                anim.SetBool("three", false);
                slide = 2;
                return;
            }
        }   
    }

    public void MainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
