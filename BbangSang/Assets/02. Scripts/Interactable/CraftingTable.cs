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

    [Header("저장 옵션")]
    [SerializeField] private bool savePlayerOnUpgrade = true;

    private Player player;

    private const long BASE_COST = 500_000;
    private const double GROWTH = 1.8;
    private const long ROUND_UNIT = 100_000;

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
    }

    public override void Interact()
    {
        craftingUI.SetActive(true);
        isUIOpen = true;
        Debug.Log("제작테이블 열림");
    }

    public void CloseUI()
    {
        craftingUI.SetActive(false);
        isUIOpen = false;
        Debug.Log("제작테이블 닫힘");
    }

    //시간 감소 버튼
    public void DecreaseTime()
    {
        curTime = Mathf.Max(0f, curTime - 0.2f);
        Debug.Log($"[Level {level}] 남은 시간 : {curTime:0.00}초");
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

        level = targetLevel;
        player.playerData.craftingTableLevel = level;
        
        curTime = GetMaxTime();

        if (savePlayerOnUpgrade) player.Save();

        Debug.Log($"[업그레이드 성공] -{cost:N0}원 / 잔액 {player.Money:N0}원\n" + $"현재 레벨: {level}, 최대 시간: {GetMaxTime():0.00}초");
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
    }
}
