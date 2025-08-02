using System;
using System.Collections.Generic;

[Serializable]
public class Bread
{
    public int ID;
    public string Name;
    public int RecipePrice;
    public int BreadPrice;
    public string Description;
}

[Serializable]
public class BreadList
{
    public List<Bread> Recipes;
}
