using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenScript : MonoBehaviour
{
    public void StartGame()
    {
        DayManagerScript.Instance.EndDay();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}