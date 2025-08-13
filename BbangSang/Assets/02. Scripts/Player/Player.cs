using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static string SaveFilePath => Application.persistentDataPath + "/playerData.json";
    public PlayerData playerData;
    public PlayerControl control;

    private void Awake()
    {
        GameManager.Instance.Player = this;
        control = GetComponent<PlayerControl>();
    }

    public void Save()
    {
        var saveData = JsonUtility.ToJson(playerData);
        File.WriteAllText(SaveFilePath, saveData);
        Debug.Log($"{SaveFilePath}");
    }

    public void Load()
    {
        string loadData;
        try
        {
            loadData = File.ReadAllText(SaveFilePath);
        }catch (FileNotFoundException)
        {
            Debug.LogError("Player data file not found. Creating new player data.");
            CreateData();
            return;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error reading player data file: {e.Message}");
            return;
        }

        if (loadData == null || loadData == "")
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
            inGameDay = 1,
            uniforms = new List<int>(),
            wearedUniformId = null,
            employees = new List<int>(),
            money = 0,
            debt = 300000000 // ºú 3¾ï
        };
    }

    public void CreateDataWithName(string playerName)
    {
        CreateData();

        if (!string.IsNullOrWhiteSpace(playerName))
            playerData.name = playerName.Trim();
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
    public int inGameDay;
    public List<int> uniforms;
    public int? wearedUniformId;
    public List<int> employees;
    public long money;
    public long debt;
}
