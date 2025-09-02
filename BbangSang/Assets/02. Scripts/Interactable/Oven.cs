using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Oven : BaseInteractable
{
    [Header("Oven Bake Time Settings")]
    public const float MaxBakeTime = 240f; // 최대 시간 m

    [SerializeField] public float reduceTimePerLevel = 10f; // 레벨 당 감소 시간 i
    private float AdjustedBakeTime { get; set; } // 실제 걸리는 시간
    // 레벨 n일 때 adjustedBakeTime = m - (레벨 * i)
    private float currentBakeTime; 
    
    [Header("Oven Bake Count Settings")]
    public const int MinBakeCount = 100; // 최소 개수 m
    [SerializeField] public int increaseCountPerLevel = 10; // 레벨 당 증가 개수 i
    public int AdjustedBakeCount { get; set; }
    // 레벨 n일 때 adjustedBakeCount = m + (레벨 * i)
    private int currentBakeCount;
    
    private int ovenLevel; // 오븐 레벨
    
    [SerializeField] OvenUI ovenUI;
    [SerializeField] GameObject selectDoughUI;
    SpriteRenderer spriteRenderer;
    
    public bool IsActive { get; set; }
    public bool IsBakeDone { get; set; } = false;

    public Coroutine bakeCoroutine;

    public List<(int recipeId, int count)> InOven = new();
    public int InOvenCount;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ovenUI.Init(this);
        SettingOvenByLevel();
        InOven.Clear();
        InOvenCount = 0;
    }

    // 레벨 별 오븐 설정
    // TODO : 나중에 플레이어 오븐 레벨 오르면 이거 불러줘야됨
    public void SettingOvenByLevel()
    {
        ovenLevel = GameManager.Instance.Player.playerData.ovenLevel;
        AdjustedBakeTime = MaxBakeTime - (ovenLevel * reduceTimePerLevel);
        AdjustedBakeCount = MinBakeCount + (ovenLevel * increaseCountPerLevel);


    }
    
    public override void Interact()
    {
        if (GameManager.Instance.CurGameState != GameState.PreGame) return;
        Debug.Log("오븐을 킵니다.");
        ovenUI.gameObject.SetActive(true);
    }

    public void OvenImageSetting(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void Bake()
    {
        IsActive = true;
        IsBakeDone = false;
        bakeCoroutine = StartCoroutine(BakeInTime());
        foreach (var item in InOven)
        {
            BreadManager.Instance.UseDough(item.recipeId, item.count);
        }

        InOven.Clear();
        InOvenCount = 0;
        ovenUI.OvenOn();
        ovenUI.CloseAllUI();
    }

    public void GetBakedBread()
    {
        foreach (var item in InOven)
        {
            BreadManager.Instance.AddBread(item.recipeId, item.count);
        }
    }

    public void StopBake()
    {
        if(bakeCoroutine != null) StopCoroutine(bakeCoroutine);
        IsBakeDone = false;
        IsActive = false;
        ovenUI.OvenOff();
        InOven.Clear();
    }

    // 오븐에 반죽 넣기 시도
    public bool TryAddDoughToOven(int recipeId, int doughCount)
    {
        if (InOvenCount + doughCount <= AdjustedBakeCount)
        {
            bool doughInOven = false;
            for (int i = 0; i < InOven.Count; i++)
            {
                if (InOven[i].recipeId == recipeId)
                {
                    var inOvenItem = InOven[i];
                    inOvenItem.count = doughCount;
                    InOven[i] = inOvenItem;
                    doughInOven = true;
                    break;
                }
            }

            if (!doughInOven)
            {
                InOven.Add((recipeId, doughCount));
            }

            InOvenCount = InOven.Sum(x => x.count);

            return true;
        }
        else return false;
    }

    IEnumerator BakeInTime()
    {
        // 테스트용으로 10초
        yield return new WaitForSecondsRealtime(10f);
        
        IsActive = false;
        bakeCoroutine = null;
        IsBakeDone = true;
        ovenUI.OvenOff();
        Debug.Log("빵 다 구워짐");
    }

}
