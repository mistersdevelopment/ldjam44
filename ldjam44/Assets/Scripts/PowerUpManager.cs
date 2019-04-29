using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpDef
{
    public string name;
    public Sprite icon;
    public PowerUp prefab;
}

public class PowerUpManager : MonoBehaviour
{
    public PowerUpDef[] lootTable;
    public GameObject coinDropPrefab;

    public int pityTimer = 4; // Every pityTimer'th is guaranteed
    public int spinsSinceUpgrade = 0;
    public bool jackpotGiven = false;

    private static PowerUpManager _instance;
    public static PowerUpManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
   
    public PowerUpDef RandomPowerUp()
    {
        return lootTable[Random.Range(0, lootTable.Length)];
    }
}
