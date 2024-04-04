using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{
    public void onMenuClick()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
