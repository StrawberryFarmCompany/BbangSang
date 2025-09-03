using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CraftingTable : BaseInteractable
{
    [SerializeField] private CraftingTableUI uiController;

    [Header("시간 / 레벨")]
    public float maxTimeBase = 120f;            // 기본 최대 제작 시간
    public float decreasePerLevel = 5f;         // 레벨당 최대시간 감소량
    public int level = 1;                       //현재 레벨
    public int maxLevel = 10;                   //최대 레벨
    public float curTime;                       //남은 시간

    public enum CraftStartMode { Manual, AutoOnAwake, AutoOnInteract }

    [Header("시작 방식")]
    [SerializeField] private CraftStartMode startMode = CraftStartMode.Manual;

    [Header("저장 옵션")]
    [SerializeField] private bool savePlayerOnUpgrade = true;   //업그레이드 후 저장 여부

    private Player player;

    //업그레이드 비용 커브
    private const long baseCost = 500_000;
    private const double growth = 1.8;
    private const long roundUnit = 100_000;

    //상태 구분
    private bool isCrafting = false;        //제작 중인지
    private bool craftComplete = false;     //제작 완료인지
    private float _lastLoggedTime = -1f;

    [Header("기본 생성 갯수")]
    [SerializeField] private int producedPerBatch = 20;

    [Header("시간 감소")]
    [SerializeField] private float decreasePerClick = 20f;  //수정 가능 테스트로 20초 감소

    private int craftingRecipeID = 0;   //제작 시점 레시피 ID

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

    //제작 중 타이머 감소 및 완료 확인
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

    //남은 시간 로그
    private void RefreshTimeLog(bool force = false)
    {
        if (force || _lastLoggedTime < 0f || Mathf.Abs(curTime - _lastLoggedTime) >= 0.1f)
        {
            _lastLoggedTime = curTime;
            Debug.Log($"[Level {level}] 남은 시간 : {curTime:0.00}초");
        }
    }

    //상호작용
    public override void Interact()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurGameState != GameState.PreGame)
            return;

        if (uiController == null) return;

        if (!uiController.gameObject.activeSelf)
            uiController.gameObject.SetActive(true);

        uiController.Open();
        Debug.Log("제작테이블 열림");

        if (startMode == CraftStartMode.AutoOnInteract && !isCrafting)
            StartCraft(true);
    }

    //제작 시작
    public void StartCraft(bool resetTime)
    {
        if (isCrafting && !craftComplete) return;

        if (!BreadManager.Instance.HasSelection)
        {
            Debug.LogWarning("레시피 선택 필요");
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

    //제작 완료
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
        if (!isCrafting || craftComplete)
        {
            StartCraft(true);
            return;
        }

        curTime = Mathf.Max(0f, curTime - decreasePerClick);
        RefreshTimeLog(true);

        if (curTime <= 0f)
        {
            CompleteCraft();
        }
        else
        {
            Debug.Log($"[-{decreasePerClick:0.#}s], 남은 {curTime:0.00}초");
        }

    }

    //업그레이드 버튼
    public void UpgradeLevel()
    {
        if (isCrafting && !craftComplete)
        {
            Debug.Log("제작 중 업그레이드 불가");
            return;
        }

        //최대 레벨 체크
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

        //비용 지불(실패 시 종료)
        if (!player.TrySpendMoney(cost))
        {
            Debug.Log($"업그레이드 실패: 필요 {cost:N0}원, 보유 {before:N0}원");
            return;
        }

        level = targetLevel;
        player.playerData.craftingTableLevel = level;

        //새로운 최대 시간 계산
        float newMax = GetMaxTime();

        curTime = newMax;
        _lastLoggedTime = -1f;
        RefreshTimeLog(true);

        if (savePlayerOnUpgrade) player.Save();

        Debug.Log($"[업그레이드 성공] -{cost:N0}원 / 잔액 {player.Money:N0}원\n현재 레벨: {level}, 최대 시간: {newMax:0.00}초");
    }

    //레벨 비용 계산
    private long GetCostForLevel(int levelN)
    {
        double raw = baseCost * Math.Pow(growth, levelN - 1);
        long rounded = (long)(Math.Round(raw / roundUnit) * roundUnit);
        return rounded;
    }

    //현재 레벨 기준 최대 제작 시간
    public float GetMaxTime()
    {
        return maxTimeBase - (level - 1) * decreasePerLevel;
    }

    //시간 초기화
    public void ResetTime()
    {
        curTime = GetMaxTime();
        craftComplete = false;
        _lastLoggedTime = -1;
    }
}
