using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public Button startButton;
    public Button aiVsAiButton;
    public Button quitButton;

    public void AiVsAi()
    {
        Board.gameType = 2;
        SceneManager.LoadScene( "Chess" );
    }

    public void StartEasy()
    {
        Board.gameType = 1;
        SceneManager.LoadScene( "Chess" );
    }

    public void StartHard()
    {
        Board.gameType = 0;
        SceneManager.LoadScene( "Chess" );
    }

    public void Quit()
    {
        Application.Quit();
    }
}
