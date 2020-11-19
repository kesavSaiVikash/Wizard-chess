using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
    public Text txt;
    // Start is called before the first frame update
    void Start()
    {
        txt.text = Board.finishState;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene( "Menu" );
    }
}
