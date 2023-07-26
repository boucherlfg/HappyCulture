using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text count;
    private Achievement achievement;
    public Achievement Achievement
    {
        get => achievement;
        set
        {
            achievement = value;
            image.sprite = achievement.sprite;
            count.text = "" + achievement.count;
        }
    }
}
