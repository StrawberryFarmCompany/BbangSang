using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    { 
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if(_instance == null)
                {
                    GameObject go = new GameObject();
                    try
                    {
                        _instance = go.AddComponent<T>();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    
                    go.name = $"[{typeof(T)}]";
                    if(!Application.isBatchMode)
                    {
                        if(Application.isPlaying) DontDestroyOnLoad(go);
                    }
                }
            }
            return _instance;
        }    
    }

    public static bool IsCreatedInstance()
    {
        return (_instance != null);
    }

    public virtual void Release()
    {
        if (_instance == null) return;

        if (_instance.gameObject) Destroy(_instance.gameObject);

        _instance = null;
    }
}
