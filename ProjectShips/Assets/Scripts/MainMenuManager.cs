using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectShips.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}