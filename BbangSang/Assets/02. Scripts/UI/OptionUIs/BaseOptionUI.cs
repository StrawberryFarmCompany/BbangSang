using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOptionUI : MonoBehaviour
{
    private OptionState state;
    public OptionState State {  get { return state; } set { state = value;}}

    private OptionUI optionUi;

    public virtual void SetUI(OptionState state)
    {
        gameObject.SetActive(State == state);
    }

    public void Init(OptionUI option)
    {
        optionUi = option;
    }
}
