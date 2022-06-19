using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Global
{
    public static GameObject spawnObject = null;
    public static float budget;
    public static DefenceSelection selectedDefence;
    public static bool basePlaced = false;
    public static float mapSize = 1;
    public static int leftCounter;
    public static int rightCounter;
    public static int attackerNumber;
    public static float point = 0;

    public enum GameState
    {
        edit,
        play,
        won,
        lose
    }

    public static GameState gameState = GameState.edit;
}