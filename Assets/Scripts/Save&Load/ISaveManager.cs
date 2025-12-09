using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager 
{
    void SaveData(ref GameData gameData);
    void LoadData(GameData gameData);
}
