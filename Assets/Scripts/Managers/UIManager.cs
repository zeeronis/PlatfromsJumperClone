using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameScreen gameScreen;
    [SerializeField] ResultScreen resultScreen;
    [Space]
    [SerializeField] GameObject[] screens;

    public GameScreen GameScreen => gameScreen;
    public ResultScreen ResultScreen => resultScreen;


    public void ActivScreen(Screens screen)
    {
        for (int i = 0; i < screens.Length; i++)
        {
            if ((int)screen == i)
                screens[i].SetActive(true);
            else
                screens[i].SetActive(false);
        }
    }
}

[System.Serializable]
public enum Screens
{
    Game,
    Result,
}
