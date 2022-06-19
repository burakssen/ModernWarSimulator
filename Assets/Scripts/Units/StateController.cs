using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    void Update()
    {
        if (Global.gameState == Global.GameState.won)
        {
            PlayerPrefs.SetString("GameState", "won");
            PlayerPrefs.SetFloat("GamePoint", Global.point);
            SceneLoader.LoadScene("WinLoseScene");
        }

        if (Global.gameState == Global.GameState.lose)
        {
            PlayerPrefs.SetString("GameState", "lose");
            SceneLoader.LoadScene("WinLoseScene");
        }
    }
}
