using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEGI.Biblioteca
{
    public class ButtonUtility : MonoBehaviour
    {

        public void StartGame()
        {
            SceneController.Instance.LoadScene("Facil");
        }

        public void RestartLevel()
        {
            SceneController.Instance.RestartScene();
        }

        public void GoToMenu()
        {
            SceneController.Instance.LoadScene("MainMenu");
        }

        public void GoToDifficultyUp()
        {
            //Dar load na scene com dificuldade maior
            //SceneController.Instance.LoadScene(dificuldadeacima);
            print("Dificuldade Acima");
        }

        public void GoToDifficultyDown()
        {
            //Dar load na scene com dificuldade menor
            //SceneController.Instance.LoadScene(dificuldadeabaixo);
            print("Dificuldade Abaixo");
        }

        public void LoadScene(string sceneName)
        {
            SceneController.Instance.LoadScene(sceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}