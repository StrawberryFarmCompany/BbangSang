using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dough : MonoBehaviour
{
    public int recipeId;
    public int doughCount;
    public Image image; 
    
    public void Init(int recipeId, int doughCount)
    {
        this.recipeId = recipeId;
        this.doughCount = doughCount;
        image.sprite = BreadManager.Instance.GetRecipe(recipeId).GetIcon();
    }
    
    
}
