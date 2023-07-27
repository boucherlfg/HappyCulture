using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotification : MonoSingleton<AchievementNotification>
{
    public Image image;
    public TMP_Text label;
    public GameObject mainGameObject;
    public float notifTime = 5;
    private CanvasGroup grp;

    public void Start()
    {
        grp = mainGameObject.GetComponent<CanvasGroup>();
        Achievements.Instance.Changed += Instance_Changed;
    }

    private void Instance_Changed(Achievement ach)
    {
        if (!this) return;
        StopAllCoroutines();
        grp.alpha = 1;
        
        mainGameObject.SetActive(true);
        image.sprite = ach.sprite;
        label.text = "achievement unlocked<br><i>" + ach.name + "</i>";
        StartCoroutine(DeactivateAfterTime());
        
        IEnumerator DeactivateAfterTime()
        {
            var grp = mainGameObject.GetComponent<CanvasGroup>();
            while (grp.alpha > 0)
            {
                grp.alpha -= Time.deltaTime / notifTime;
                yield return null;
            }
            grp.gameObject.SetActive(false);
        }
    }
}
