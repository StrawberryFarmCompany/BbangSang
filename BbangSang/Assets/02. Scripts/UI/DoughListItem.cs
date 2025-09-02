using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoughListItem : MonoBehaviour
{
    [SerializeField] private Image doughListIcon;
    [SerializeField] private TMP_Text doughListCount;

    private void Awake()
    {
        if (doughListIcon != null) doughListIcon.preserveAspect = true;
    }

    public void Set(Sprite sprite, int count, bool showCount)
    {
        if (doughListIcon != null)
        {
            doughListIcon.sprite = sprite;
            doughListIcon.enabled = sprite != null;
        }

        if (doughListCount != null)
        {
            doughListCount.gameObject.SetActive(showCount);
            if (showCount) doughListCount.text = $"x{count}";
        }

        gameObject.SetActive(true);
    }
}
