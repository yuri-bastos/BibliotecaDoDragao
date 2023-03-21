using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGameEasy()
    {
        //Chamada função jogo
        print("Start");
    }

    public void StartGameMedium()
    {
        //Chamada função jogo
        print("Start");
    }

    public void StartGameHard()
    {
        //Chamada função jogo
        print("Start");
    }

    public void QuitButton()
    {
        //Chamada sair do jogo
        Application.Quit();
        print("Quit");
    }

    public void ButtonPressSound()
    {
        //Play no som de botão
        print("Click");
    }

}
