using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerParams
{
    public int levelDamage;
}

[Serializable]
public class GameData
{
    public uint money;
    public bool isTutorialShown;
    public List<int> openedLevels;
    public List<int> upgradesIDs;

    public PlayerParams playerParams;
    
    public GameData()
    {
        money = 5;
        openedLevels = new List<int>() { 1 };
        upgradesIDs = new List<int>();
        isTutorialShown = false;

        playerParams.levelDamage = 1;
    }
}
