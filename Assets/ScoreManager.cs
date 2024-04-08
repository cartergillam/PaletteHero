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
    public TMP_Text rank1;
    public TMP_Text rank2;
    public TMP_Text rank3;
    public TMP_Text rank4;
    public TMP_Text rank5;
    public int[] hs = {7500, 6000, 5000, 2500, 1000};
    public int score = 10000;
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
        rank1.text = hs[0].ToString();
        rank2.text = hs[1].ToString();
        rank3.text = hs[2].ToString();
        rank4.text = hs[3].ToString();
        rank5.text = hs[4].ToString();
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

    public void highScores()
    {
        if (score > hs[4])
        {
            int i;
            for (i = 4; i >= 0; i--)
            {
                if (i == 0 || score <= hs[i - 1]) {
                    break;
                }
                if (i <= 4) {
                    hs[i] = hs[i - 1];
                }
            }
            hs[i] = score;
            rank1.text = hs[0].ToString();
            rank2.text = hs[1].ToString();
            rank3.text = hs[2].ToString();
            rank4.text = hs[3].ToString();
            rank5.text = hs[4].ToString();
        }
        return;
    }
}
