using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public int health = 5;
    public int maxHealth = 5;
    private List<HealthHeart> hearts = new List<HealthHeart>();
    
    public GameObject gameOverMenu;

    void Start()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            var heart = Instantiate(heartPrefab, transform).GetComponent<HealthHeart>();
            hearts.Add(heart);
        }
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        UpdateHearts();

        if (health == 0)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].UpdateHeart(i < health ? HeartStatus.Full : HeartStatus.Empty);
        }
    }
}
