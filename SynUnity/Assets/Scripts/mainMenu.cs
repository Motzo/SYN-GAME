using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void StartButton(){
        SceneManager.LoadScene(1);
    }

    public void CreditsButton(){
        SceneManager.LoadScene(3);
    }
    
    public void QuitButton(){
        Application.Quit();
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene(0);
    }
}
