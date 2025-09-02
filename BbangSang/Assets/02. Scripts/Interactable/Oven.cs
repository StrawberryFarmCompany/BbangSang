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
    
    [SerializeField] GameObject ovenUI;
    [SerializeField] GameObject selectDoughUI;
    SpriteRenderer spriteRenderer;
    
    public bool IsActive { get; set; }

    public List<(int recipeId, int count)> InOven = new();
    public int InOvenCount;
    
    private void Start()
    {
        if (ovenUI != null) ovenUI.GetComponent<OvenUI>().Init(this);
        else Debug.LogError("OvenUI is not assigned in the Oven script.");
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
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
        ovenUI.SetActive(true);
    }

    public void OvenImageSetting(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
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

}
