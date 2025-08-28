using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BreadManager : Singleton<BreadManager>
{
    public int CurrentDayIndex { get; private set; } = -1;

    public int? CurrentRecipeID { get; private set; } = null;
    public bool HasSelection => CurrentRecipeID.HasValue;

    private readonly Dictionary<int, int> dough = new();

    public RecipeList recipeList { get; private set; }
    
    public void InitDay(int dayIndex)
    {
        if (CurrentDayIndex == dayIndex) return;
        CurrentDayIndex = dayIndex;
        dough.Clear();
        CurrentRecipeID = null;
    }

    private void Awake()
    {
        LoadRecipeData();
    }

    
    public void LoadRecipeData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("RecipesData");
        if (jsonFile == null)
        {
            Debug.LogError("Error: 'RecipesData' 파일을 찾을 수 없습니다.");
            return;
        }
        recipeList = JsonUtility.FromJson<RecipeList>(jsonFile.text);
    }

    public void NextDay()
    {
        CurrentDayIndex += 1;
        dough.Clear();
        CurrentRecipeID = null;
    }

    public void SelectRecipe(int recipeID) => CurrentRecipeID = recipeID;
    public void ClearRecipeSelection() => CurrentRecipeID = null;

    public int GetDough(int recipeID) => dough.TryGetValue(recipeID, out var c) ? c : 0;

    public int AddDough(int recipeID, int amount)
    {
        if (amount <= 0)
        {
            return GetDough(recipeID);
        }

        int newCount = GetDough(recipeID) + amount;
        dough[recipeID] = newCount;
        return newCount;
    }

    public bool UseDough(int recipeID, int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        int cur = GetDough(recipeID);

        if (cur < amount)
        {
            return false;
        }

        cur -= amount;

        if (cur == 0)
        {
            dough.Remove(recipeID);
        }
        else
        {
            dough[recipeID] = cur;
        }

        return true;
    }

    public bool TryAddDoughSelected(int amount, out int newCount)
    {
        newCount = 0;
        if (!HasSelection)
        {
            return false;
        }

        newCount = AddDough(CurrentRecipeID.Value, amount);

        return true;
    }

    public bool TryUseDoughSelected(int amount)
    {
        if (!HasSelection)
        {
            return false;
        }

        return UseDough(CurrentRecipeID!.Value, amount);
    }

    public void GetAllDough(List<(int recipeId, int count)> buffer)
    {
        buffer.Clear();
        foreach (var kv in dough)
            buffer.Add((kv.Key, kv.Value));
    }
}
