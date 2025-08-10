using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplayer : MonoBehaviour
{
    private TextMeshProUGUI tmpro;

    // Start is called before the first frame update
    void Start()
    {
        tmpro= GetComponent<TextMeshProUGUI>();

        UpdateMoney();
    }
    
    public void UpdateMoney()
    {
        int money = PlayerPrefs.GetInt("Money",0);
        tmpro.text = money.ToString();
    }
}
