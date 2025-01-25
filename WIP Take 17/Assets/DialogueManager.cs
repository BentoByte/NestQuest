using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMaanager : MonoBehaviour
{

    private static DialogueMaanager instance;
    

    private void Awake()
    {
        instance = this;
    }

    public static DialogueMaanager GetInstance()
    {
        return instance;
    }
}
