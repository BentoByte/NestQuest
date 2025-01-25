using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Money : MonoBehaviour
{
    public int money;
    public int moneyPoss;
    public int Add;
    public bool Visable;
    [SerializeField]  TextMeshProUGUI MoneyText;
    public void Start()
    {
        moneyPoss = money;
    }
    void Update()
    {
        if (moneyPoss > money)
        {
            money++;
        }
        // MoneyText.text = money.ToString();
        if (Input.GetKeyDown(KeyCode.M))
        {
            MakeMoney(Add);
        }
    }
    public void MakeMoney(int MoneyAdd)
    {
        moneyPoss += MoneyAdd;
        // for (int i = 0; i <= MoneyAdd; i++)
        // {
        //     money++;
        // }
        MoneyText.gameObject.SetActive(true);
    }
}
