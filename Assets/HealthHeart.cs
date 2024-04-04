using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, emptyHeart;
    private Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void UpdateHeart(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}