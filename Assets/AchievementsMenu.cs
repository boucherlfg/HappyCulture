using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsMenu : MonoBehaviour
{
    public GameObject achievementItem;
    void OnEnable()
    {
        Achievements.Instance.Changed += Refresh;
    }

    private void Refresh(Achievement achievement)
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        foreach (var ach in Achievements.Instance.unlocked)
        {
            var instance = Instantiate(achievementItem);
            var comp = instance.GetComponent<AchievementItem>();
            comp.Achievement = ach;
            instance.transform.SetParent(transform);
        }
    }
}
