using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Recipe
{
    public int ID;
    public string Name;
    public int RecipePrice;
    public int BreadPrice;
    public string Description;

    [NonSerialized] public Sprite Icon;  // Sprite는 Unity 에셋이므로 JSON에 저장할 수 없음 그래서 NonSerialized 사용

    public Sprite GetIcon()
    {
        if (Icon == null)
            Icon = Resources.Load<Sprite>($"Art/{Name}");
        return Icon;
    }
}

[Serializable]
public class RecipeList
{
    public List<Recipe> Recipes;
}
