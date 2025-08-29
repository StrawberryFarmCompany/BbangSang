using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughInventory : Singleton<DoughInventory>
{
    private readonly Dictionary<int, int> counts = new();

    public event Action<int, int> OnChanged;

    public int Get(int recipeID) => counts.TryGetValue(recipeID, out var c) ? c : 0;

    public void Add(int recipeID, int amount)
    {
        if (recipeID <= 0 || amount <= 0) return;

        counts.TryGetValue(recipeID, out var cur);
        var next = cur + amount;
        counts[recipeID] = next;

        OnChanged?.Invoke(recipeID, next);
        Debug.Log($"���� �߰�: id = {recipeID}, ���� = {next}");
    }

    public bool TryConsume(int recipeID, int amount)
    {
        if (!counts.TryGetValue(recipeID, out var cur) || cur < amount)
            return false;

        cur -= amount;
        if (cur <= 0) counts.Remove(recipeID);
        else counts[recipeID] = cur;

        OnChanged?.Invoke(recipeID, cur);
        Debug.Log($"���� �Һ�: id = {recipeID}, ���� ���� = {cur}");
        return true;
    }

    public IEnumerable<KeyValuePair<int, int>> All() => counts;
}
