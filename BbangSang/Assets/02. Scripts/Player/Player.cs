using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static string SaveFilePath => Application.persistentDataPath + "/playerData.json";
    public PlayerData playerData;

    void Save()
    {
        var saveData = JsonUtility.ToJson(playerData);
        File.WriteAllText(SaveFilePath, saveData);
    }

    void Load()
    {
        var loadData = File.ReadAllText(SaveFilePath);
        if(loadData == null || loadData == "")
        {
            Debug.LogError("There's No Player Data");
            Debug.Log("Creating New Player Data");
            CreateData();
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(loadData);
        }
    }

    void CreateData()
    {
        playerData = new PlayerData
        {
            name = "plyaer",
            recipes = new List<int>(),
            craftingTableLevel = 1,
            recipeTableLevel = 1,
            ovenLevel = 1,
            displayLevel = 1,
            uniforms = new List<int>(),
            wearedUniformId = null,
            employees = new List<int>(),
            money = 0,
            debt = 300000000 // ºú 3¾ï
        };
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class PlayerData
{
    public string name;
    public List<int> recipes;
    public int craftingTableLevel;
    public int recipeTableLevel;
    public int ovenLevel;
    public int displayLevel;
    public List<int> uniforms;
    public int? wearedUniformId;
    public List<int> employees;
    public long money;
    public long debt;
}
