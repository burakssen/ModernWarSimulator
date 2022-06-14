using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Global
{
    public static GameObject spawnObject = null;
    public static float budget;
    public static DefenceSelection selectedDefence;
    public static float mapSize = 1;
    public enum GameState{ 
        edit,
        play
    }

    public static GameState gameState = GameState.edit;
}