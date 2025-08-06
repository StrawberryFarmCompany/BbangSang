using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : BaseInteractable
{
    public GameObject craftingUI;
    private bool isUIOpen = false;

    public float maxTimeBase = 120f;            // m
    public float decreasePerLevel = 5f;         // i
    
    public int level = 1;
    public int maxLevel = 10;
    public float curTime;                       //남은 시간

    private void Awake()
    {
        ResetTime();
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
        if (level < maxLevel)
        {
            level++;
            ResetTime();
            Debug.Log($"[업그레이드] 현재 레벨: {level}, 최대 시간 : {GetMaxTime()}초");
        }
        else
        {
            Debug.Log("최대 레벨입니다");
        }
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
