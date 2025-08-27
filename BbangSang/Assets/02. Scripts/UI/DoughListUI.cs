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
    [Header("UI (둘 중 하나만)")]
    [SerializeField] private Text uiText;
    [SerializeField] private TMP_Text tmpText;

    [Header("표시 설정")]
    [SerializeField] private float refreshInterval = 0.2f;
    [SerializeField] private string title = "반죽";

    private WaitForSeconds wait;
    private readonly List<(int recipeId, int count)> _buffer = new();

    void Awake()
    {
        if (uiText == null) uiText = GetComponent<Text>();
        if (tmpText == null) tmpText = GetComponent<TMP_Text>();
        if (uiText == null) uiText = GetComponentInChildren<Text>(true);
        if (tmpText == null) tmpText = GetComponentInChildren<TMP_Text>(true);

        if (uiText == null && tmpText == null)
        {
            Debug.LogWarning("[DoughListUI] Text 또는 TMP_Text 연결 필요");
            enabled = false;
            return;
        }

        wait = new WaitForSeconds(refreshInterval);
    }

    void OnEnable()
    {
        Refresh();
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
        if (bm == null) return;

        _buffer.Clear();
        bm.GetAllDough(_buffer);

        var sb = new StringBuilder();
        sb.AppendLine(title);

        if (_buffer.Count == 0)
        {
            sb.AppendLine("  (없음)");
        }
        else
        {
            _buffer.Sort((a, b) => a.recipeId.CompareTo(b.recipeId));
            foreach (var (id, cnt) in _buffer)
                sb.Append("  ID ").Append(id).Append(" : ").Append(cnt).AppendLine();
        }

        SetText(sb.ToString());
    }

    private void SetText(string s)
    {
        if (tmpText != null) tmpText.text = s;
        else if (uiText != null) uiText.text = s;
    }
}
