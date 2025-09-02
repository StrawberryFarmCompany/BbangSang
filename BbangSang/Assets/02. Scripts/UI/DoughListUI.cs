using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DoughListUI : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private string title = "반죽";

    [Header("리스트 아이템 프리팹")]
    [SerializeField] private RectTransform content;
    [SerializeField] private DoughListItem itemPrefab;

    [Header("아이콘")]
    [SerializeField] private Sprite[] recipeIcons;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private bool showCount = true;

    [Header("갱신 주기")]
    [SerializeField] private float refreshInterval = 0.2f;

    private readonly List<(int recipeId, int count)> _buffer = new();
    private readonly Dictionary<int, DoughListItem> _views = new();
    private WaitForSeconds wait;

    void Awake()
    {
        wait = new WaitForSeconds(refreshInterval);
        if (titleText) titleText.text = title;
    }

    void OnEnable()
    {
        StartCoroutine(Loop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            Refresh();
            yield return wait;
        }
    }

    public void Refresh()
    {
        var bm = BreadManager.Instance;
        if (bm == null || content == null || itemPrefab == null) return;

        _buffer.Clear();
        bm.GetAllDough(_buffer);
        _buffer.Sort((a, b) => a.recipeId.CompareTo(b.recipeId));

        foreach (var v in _views.Values) v.gameObject.SetActive(false);

        foreach (var (id, cnt) in _buffer)
        {
            var view = GetOrCreate(id);
            view.Set(GetIconById(id), cnt, showCount);
        }
    }

    private DoughListItem GetOrCreate(int id)
    {
        if (_views.TryGetValue(id, out var v) && v != null) return v;

        var inst = Instantiate(itemPrefab, content);
        inst.name = $"Dough_{id}";
        _views[id] = inst;
        return inst;
    }

    private Sprite GetIconById(int id)
    {
        int idx = id - 1;
        if (idx >= 0 && idx < recipeIcons.Length && recipeIcons[idx] != null)
            return recipeIcons[idx];
        return defaultIcon;
    }

    private void OnValidate()
    {
        if (titleText) titleText.text = title;
    }
}
