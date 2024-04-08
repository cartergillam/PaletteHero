using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText;
    public TMP_Text scoreText1;
    int score = 10000;
    private float updateInteval = 1f;
    private float timer = 0f;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scoreText.text = score.ToString();
        scoreText1.text = score.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInteval)
        {
            score -= 10;
            scoreText.text = score.ToString();
            scoreText1.text = score.ToString();
            timer = 0f;
        }
    }

    public void AddPoints(int points)
    {
        scoreText.text = score.ToString();
        scoreText1.text = score.ToString();
        score += points;
    }

}
