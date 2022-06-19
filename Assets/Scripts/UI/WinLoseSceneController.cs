using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class WinLoseSceneController : MonoBehaviour
{
    [SerializeField] private TMP_Text message;
    [SerializeField] private TMP_Text point;
    private void Start()
    {
        if (PlayerPrefs.GetString("GameState") == "won")
        {
            message.text = "You have won the level";
            point.text = PlayerPrefs.GetFloat("GamePoint").ToString();
        } 
        else if(PlayerPrefs.GetString("GameState") == "lose")
        {
            message.text = "You have lost.";
            point.text = "";
        }
    }
}
