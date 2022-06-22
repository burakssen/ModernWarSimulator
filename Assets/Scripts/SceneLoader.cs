using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
        public static void LoadScene(string sceneName)
        {
                if (sceneName == "Start")
                {
                        Global.gameState = Global.GameState.edit;
                        Global.basePlaced = false;
                        Global.rightCounter = 0;
                        Global.leftCounter = 0;
                        Global.point = 0;
                }
                SceneManager.LoadScene(sceneName);
        }

        public static void QuitGame()
        {
                Application.Quit();
        }
}
