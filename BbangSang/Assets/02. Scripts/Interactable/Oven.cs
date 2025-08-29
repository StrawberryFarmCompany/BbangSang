using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : BaseInteractable
{
    [Header("Oven Bake Time Settings")]
    public const float MaxBakeTime = 240f; // 최대 시간 m

    [SerializeField] public float reduceTimePerLevel = 10f; // 레벨 당 감소 시간 i
    private float adjustedBakeTime; // 실제 걸리는 시간
    // 레벨 n일 때 adjustedBakeTime = m - (레벨 * i)
    private float currentBakeTime; 
    
    [Header("Oven Bake Count Settings")]
    public const int MinBakeCount = 100; // 최소 개수 m
    [SerializeField] public int increaseCountPerLevel = 10; // 레벨 당 증가 개수 i
    private int adjustedBakeCount;
    // 레벨 n일 때 adjustedBakeCount = m + (레벨 * i)
    private int currentBakeCount;
    
    private int ovenLevel; // 오븐 레벨
    
    [SerializeField] GameObject ovenUI;
    [SerializeField] GameObject selectDoughUI;
    SpriteRenderer spriteRenderer;
    
    public bool IsActive { get; set; }
    
    private void Start()
    {
        if (ovenUI != null) ovenUI.GetComponent<OvenUI>().Init(this);
        else Debug.LogError("OvenUI is not assigned in the Oven script.");
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        SettingOvenByLevel();
    }

    // 레벨 별 오븐 설정
    // TODO : 나중에 플레이어 오븐 레벨 오르면 이거 불러줘야됨
    public void SettingOvenByLevel()
    {
        ovenLevel = GameManager.Instance.Player.playerData.ovenLevel;
        adjustedBakeTime = MaxBakeTime - (ovenLevel * reduceTimePerLevel);
        adjustedBakeCount = MinBakeCount + (ovenLevel * increaseCountPerLevel);
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
