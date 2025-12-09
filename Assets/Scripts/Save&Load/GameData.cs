using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int lvl;
    public int exp;

    public GameData()
    {
        this.lvl = 1;
        this.exp = 0;
    }
}
