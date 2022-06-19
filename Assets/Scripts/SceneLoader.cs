﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
        public static void LoadScene(string sceneName)
        {
                SceneManager.LoadScene(sceneName);
        }

        public static void QuitGame()
        {
                Application.Quit();
        }
}