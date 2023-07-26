using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerOffMenu : MonoBehaviour
{
    public GameObject choices;
    public PowerOffConfirmation confirmation;
    void OnEnable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(child == choices.transform);
        }
    }
    public void Restart()
    {
        confirmation.gameObject.SetActive(true);
        choices.SetActive(false);
        confirmation.toDo = () => SceneManager.LoadScene(gameObject.scene.name);
    }
    public void Shutdown()
    {
        confirmation.gameObject.SetActive(true);
        choices.SetActive(false);
        confirmation.toDo = () => Application.Quit();
    }
}
