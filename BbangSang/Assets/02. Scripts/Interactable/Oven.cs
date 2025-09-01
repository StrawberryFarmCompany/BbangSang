using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : BaseInteractable
{
    [SerializeField] GameObject ovenUI;

    SpriteRenderer spriteRenderer;
    private void Start()
    {
        if (ovenUI != null) ovenUI.GetComponent<OvenUI>().Init(this);
        else Debug.LogError("OvenUI is not assigned in the Oven script.");

        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public override void Interact()
    {
        if (GameManager.Instance.CurGameState != GameState.PreGame) return;
        Debug.Log("오븐을 킵니다.");
        ovenUI.SetActive(true);
    }

    public void OvenImageSetting(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

}
