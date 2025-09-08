using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTableUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private CraftingTable craft;

    [Header("Buttons")]
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnUpgrade;
    [SerializeField] private Button btnClose;

    private bool isUIOpen = false;

    private void Awake()
    {
        if (craft == null) craft = GetComponentInParent<CraftingTable>();

        btnStart?.onClick.AddListener(() => craft.DecreaseTime());
        btnUpgrade?.onClick.AddListener(() => craft.UpgradeLevel());
        btnClose?.onClick.AddListener(Close);
    }

    public void Open()
    {
        if (isUIOpen) return;

        if (gameObject.activeSelf) gameObject.SetActive(true);

        if (panel != null && !panel.activeSelf) panel.SetActive(true);

        isUIOpen = true;
    }

    public void Close()
    {
        if (!isUIOpen) return;

        if (panel != null && panel.activeSelf) panel.SetActive(false);        

        isUIOpen = false;
    }
}
