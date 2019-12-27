using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgreadTurret : MonoBehaviour
{
    public GameObject thisUpgread; // канвас с кнопкой улучшения в объекте
    public GameObject Upgread; // объект улучшения

    void Start()
    {
        thisUpgread = transform.Find("Canvas").gameObject;
    }

    void Update()
    {
        PriceUIButton();
    }
    //передача актуального прайса для улечшения в текст кнопки
    public void PriceUIButton()
    {
        thisUpgread.transform.Find("Upgread").GetComponentInChildren<Text>().text = Upgread.GetComponent<Turret>().price.ToString() + "coins";
        if (Upgread.GetComponent<Turret>().turretlevel >= 3)
        {
            thisUpgread.transform.Find("Upgread").GetComponentInChildren<Text>().text = "MAX";
        }
    }
    //делегрование кнопке функции улучшения 
    public void OpenDialog()
    {
        if (!GameManager.instance.gameOver)
        {
            thisUpgread.SetActive(true);
            thisUpgread.transform.Find("Upgread").GetComponent<Button>().onClick.AddListener(delegate { Upgread.GetComponent<Turret>().Upgread(); });
            thisUpgread.transform.Find("Upgread").GetComponent<Button>().onClick.AddListener(delegate { thisUpgread.SetActive(false); });
        }

    }
}
