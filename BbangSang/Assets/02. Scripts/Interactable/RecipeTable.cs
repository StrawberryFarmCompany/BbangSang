using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeTable : BaseInteractable
{
    [SerializeField] private GameObject recipeUI;
    public override void Interact()
    {
        if (GameManager.Instance.CurGameState != GameState.PreGame) return;
        
        if (recipeUI != null)
        {
            recipeUI.SetActive(true);
            Debug.Log("레시피 테이블 보기");
        }
    }
}
