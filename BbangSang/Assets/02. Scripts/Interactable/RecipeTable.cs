using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeTable : BaseInteractable
{
    [SerializeField] private GameObject recipeTablePanel;
    public override void Interact()
    {
        if (recipeTablePanel != null)
        {
            recipeTablePanel.SetActive(true);
            Debug.Log("레시피 테이블 보기");
        }
    }
}
