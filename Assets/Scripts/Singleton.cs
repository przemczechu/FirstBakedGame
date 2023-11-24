using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T> : MonoBehaviour where T : MonoBehaviour
{
    private static T Instance;
    public static T FindInstance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<T>();
            }
            return Instance;
        }
    }
}
