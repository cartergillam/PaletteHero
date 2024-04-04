using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{

    public Button button;
    public GameObject player;
    private bool triggerPlayer;
    public GameObject UI_Shop;
    public HealthBar hb;


    public void Start()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void Update()
    {
        
    }

    public void TaskOnClick()
    {
        hb.EarnHeart();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggerPlayer = true;
            UI_Shop.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            triggerPlayer = false;
            UI_Shop.SetActive(false);
        }
    }
}
