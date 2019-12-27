using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public GameObject enemy;

    void Start()
    {
        
    }

    void Update()
    {
        //GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(enemy.transform.position);
    }

    internal object value;
}
