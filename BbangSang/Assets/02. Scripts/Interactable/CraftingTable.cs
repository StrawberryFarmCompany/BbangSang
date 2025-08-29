using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CraftingTable : BaseInteractable
{
    public GameObject craftingUI;
    private bool isUIOpen = false;

    [Header("시간 / 레벨")]
    public float maxTimeBase = 120f;            // m
    public float decreasePerLevel = 5f;         // i
    public int level = 1;
    public int maxLevel = 10;
    public float curTime;                       //남은 시간

    public enum CraftStartMode { Manual, AutoOnAwake, AutoOnInteract }

    [Header("시작 방식")]
    [SerializeField] private CraftStartMode startMode = CraftStartMode.Manual;

    [Header("저장 옵션")]
    [SerializeField] private bool savePlayerOnUpgrade = true;
    [SerializeField] private bool justCurrentToNewMax = false;

    private Player player;

    private const long BASE_COST = 500_000;
    private const double GROWTH = 1.8;
    private const long ROUND_UNIT = 100_000;

    private bool isCrafting = false;
    private bool craftComplete = false;
    private float _lastLoggedTime = -1f;

    [Header("기본 생성 갯수")]
    [SerializeField] private int producedPerBatch = 20;

    private void Awake()
    {
        ResetTime();
    }

    private void Start()
    {
        player = GameManager.Instance?.Player;

        if (player != null && player.playerData != null)
        {
            level = Mathf.Clamp(player.playerData.craftingTableLevel, 1, maxLevel);
            ResetTime();
        }

        if (startMode == CraftStartMode.AutoOnAwake)
            StartCraft(true);
    }

    private void Update()
    {
        if (!isCrafting) return;

        curTime -= Time.deltaTime;
        if (curTime < 0f) curTime = 0f;

        if (curTime <= 0f && !craftComplete)
        {
            RefreshTimeLog(true);
            CompleteCraft();
        }
    }

    private void RefreshTimeLog(bool force = false)
    {
        if (force || _lastLoggedTime < 0f || Mathf.Abs(curTime - _lastLoggedTime) >= 0.1f)
        {
            _lastLoggedTime = curTime;
            Debug.Log($"[Level {level}] 남은 시간 : {curTime:0.00}초");
        }
    }

    public override void Interact()
    {
        if (GameManager.Instance.CurGameState != GameState.PreGame) return;
        
        craftingUI.SetActive(true);
        isUIOpen = true;
        Debug.Log("제작테이블 열림");

        if (startMode == CraftStartMode.AutoOnInteract && !isCrafting)
            StartCraft(true);
    }

    public void CloseUI()
    {
        craftingUI.SetActive(false);
        isUIOpen = false;
        Debug.Log("제작테이블 닫힘");
    }

    private int craftingRecipeID = 0;

    public void StartCraft(bool resetTime)
    {
        if (isCrafting && !craftComplete) return;

        if (!BreadManager.Instance.HasSelection)
        {
            Debug.LogWarning("������ ���� �� ��, ���� �Ұ�");
            return;
        }
        craftingRecipeID = BreadManager.Instance.CurrentRecipeID.Value;

        if (resetTime) curTime = GetMaxTime();
        isCrafting = true;
        craftComplete = false;
        _lastLoggedTime = -1f;

        Debug.Log($"[CRAFT] 제작 시작: (id = {craftingRecipeID}) : {curTime:0}s");
        RefreshTimeLog(true);
    }

    public void StopCraft()
    {
        isCrafting = false;
        Debug.Log("[CRAFT] 제작 중지");
    }

    private void CompleteCraft()
    {
        craftComplete = true;
        isCrafting = false;

        if (craftingRecipeID <= 0)
        {
            Debug.LogWarning("제작 중 레시피ID 없음");
            return;
        }

        var bm = BreadManager.Instance;
        if (bm == null)
        {
            return;
        }

        if (!bm.TryAddDoughSelected(producedPerBatch, out int newCount))
        {
            Debug.LogWarning("선택된 레시피 없음");
            return;
        }

        Debug.Log($"[CRAFT] 제작 완료: ID={craftingRecipeID}, +{producedPerBatch} 현재 {newCount}개");
    }

    //시간 감소 버튼
    public void DecreaseTime()
    {
        if (!isCrafting || craftComplete) StartCraft(true);

        if (!craftComplete)
        {
            curTime = Mathf.Max(0f, curTime - 20f);    //클릭 시 시간 감소 조정 가능
            RefreshTimeLog(true);

            if (curTime <= 0f)
            {
                CompleteCraft();
            }
            else
            {
                Debug.Log($"[BTN] -0.2s → 남은 {curTime:0.00}초");
            }
        }
    }

    //업그레이드 버튼
    public void UpgradeLevel()
    {
        if (level >= maxLevel)
        {
            Debug.Log("최대 레벨입니다");
            return;

        }

        if (player == null || player.playerData == null)
        {
            Debug.LogWarning("플레이어 데이터가 없습니다.");
            return;
        }

        int targetLevel = level + 1;
        long cost = GetCostForLevel(targetLevel);
        long before = player.Money;

        if (!player.TrySpendMoney(cost))
        {
            Debug.Log($"업그레이드 실패: 필요 {cost:N0}원, 보유 {before:N0}원");
            return;
        }

        float oldMax = GetMaxTime();

        level = targetLevel;
        player.playerData.craftingTableLevel = level;

        float newMax = GetMaxTime();

        if (justCurrentToNewMax && isCrafting)
        {
            curTime = (oldMax > 0f) ? Mathf.Clamp((curTime / oldMax) * newMax, 0f, newMax) : newMax;
            RefreshTimeLog(true);
        }
        else if (!isCrafting)
        {
            curTime = newMax;
            _lastLoggedTime = -1f;
            RefreshTimeLog(true);
        }

        if (savePlayerOnUpgrade) player.Save();

        Debug.Log($"[업그레이드 성공] -{cost:N0}원 / 잔액 {player.Money:N0}원\n현재 레벨: {level}, 최대 시간: {newMax:0.00}초");
    }

    private long GetCostForLevel(int levelN)
    {
        double raw = BASE_COST * Math.Pow(GROWTH, levelN - 1);
        long rounded = (long)(Math.Round(raw / ROUND_UNIT) * ROUND_UNIT);
        return rounded;
    }

    public float GetMaxTime()
    {
        return maxTimeBase - (level - 1) * decreasePerLevel;
    }

    public void ResetTime()
    {
        curTime = GetMaxTime();
        craftComplete = false;
        _lastLoggedTime = -1;
    }
}
