using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenWarningUI : MonoBehaviour
{
    private Oven oven;
    
    public void Init(Oven oven)
    {
        this.oven = oven;
    }

    public void OnClickTrash()
    {
        oven.StopBake();
        gameObject.SetActive(false);
    }
}
