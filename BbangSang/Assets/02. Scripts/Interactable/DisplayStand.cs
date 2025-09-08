using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStand : BaseInteractable
{
    private DisplayChooseUI displayChooseUI;
    [SerializeField] private GameObject displayStandUI;
    SpriteRenderer spriteRenderer;
    public Sprite emptyDisplayImg;
    public Sprite fullDisplayImg;
    
    private void Start()
    {
        displayChooseUI = GetComponent<DisplayChooseUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        emptyDisplayImg = spriteRenderer.sprite;
    }
    
    public void EmptyDisplayImage()
    {
        spriteRenderer.sprite = emptyDisplayImg;
    }

    public void FullDisplayImage()
    {
        spriteRenderer.sprite = fullDisplayImg;
    }

    public override void Interact()
    {
        if (displayStandUI != null)
        {
            displayStandUI.SetActive(true);
            Debug.Log("�Ŵ� ����");
        }
    }
}
