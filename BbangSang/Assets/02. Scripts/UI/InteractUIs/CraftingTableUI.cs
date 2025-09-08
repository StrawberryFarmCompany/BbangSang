using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Confirm (레시피 교체 확인)")]
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TMP_Text confirmText;
    [SerializeField] private Button btnConfirmYes;
    [SerializeField] private Button btnConfirmNo;

    private bool isUIOpen = false;
    private int _pendingRecipeId = 0;

    private void Awake()
    {
        if (craft == null) craft = GetComponentInParent<CraftingTable>();

        btnStart?.onClick.AddListener(() => craft.DecreaseTime());
        btnUpgrade?.onClick.AddListener(() => craft.UpgradeLevel());
        btnClose?.onClick.AddListener(Close);

        if (btnConfirmYes) btnConfirmYes.onClick.AddListener(OnConfirmYes);
        if (btnConfirmNo) btnConfirmNo.onClick.AddListener(OnConfirmNo);
        if (confirmPanel) confirmPanel.SetActive(false);
    }

    public void Open()
    {
        if (isUIOpen) return;

        if (!gameObject.activeSelf) gameObject.SetActive(true);

        if (panel != null && !panel.activeSelf) panel.SetActive(true);

        isUIOpen = true;
    }

    public void Close()
    {
        if (!isUIOpen) return;

        if (panel != null && panel.activeSelf) panel.SetActive(false);        

        isUIOpen = false;
    }

    public void RequestChangeRecipe(int newRecipeId)
    {
        if (craft == null)
        {
            Debug.LogWarning("[CraftingTableUI] CraftingTable 참조 없음");
            ApplyRecipeSelection(newRecipeId);
            return;
        }

        //반죽 완료 전 다른 걸로 바꾸려는 경우 : 확인창
        if (craft.IsCrafting && !craft.CraftComplete && newRecipeId != craft.CurrentRecipeIdInProgress)
        {
            OpenConfirm(newRecipeId);
        }
        else
        {
            //제작 중이 아니거나 동일 레시피이면 그냥 선택만 바꾼다
            ApplyRecipeSelection(newRecipeId);
        }
    }

    private void OpenConfirm(int pendingRecipeId)
    {
        _pendingRecipeId = pendingRecipeId;

        if (confirmText)
            confirmText.text = "반죽을 변경하면 현재 제작 중인 반죽이 폐기됩니다.\n정말 교체하시겠습니까?";

        if (confirmPanel && !confirmPanel.activeSelf)
            confirmPanel.SetActive(true);
    }

    private void CloseConfirm()
    {
        if (confirmPanel && confirmPanel.activeSelf)
            confirmPanel.SetActive(false);

        _pendingRecipeId = 0;
    }

    private void OnConfirmYes()
    {
        // 1) 진행 중 제작 폐기 + 시간 초기화
        craft.AbortAndResetProgress();

        // 2) 실제 레시피 선택 적용
        if (_pendingRecipeId != 0)
            ApplyRecipeSelection(_pendingRecipeId);

        CloseConfirm();

        Debug.Log("[Confirm] 레시피 교체 완료. 제작은 다시 시작하세요.");
    }

    private void OnConfirmNo()
    {
        CloseConfirm();
        Debug.Log("[Confirm] 교체 취소. 기존 제작을 그대로 이어갑니다.");
    }

    // BreadManager에 실제 선택 적용
    private void ApplyRecipeSelection(int recipeId)
    {
        var bm = BreadManager.Instance;
        if (bm == null)
        {
            return;
        }

        bm.SelectRecipe(recipeId);

        Debug.Log($"[Recipe] 현재 선택: {recipeId}");
    }
}
