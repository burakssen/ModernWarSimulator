using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
        public static void LoadScene(string sceneName)
        {
                if (sceneName == "Start")
                        Global.gameState = Global.GameState.edit;
                SceneManager.LoadScene(sceneName);
        }

        public static void QuitGame()
        {
                Application.Quit();
        }
}
