using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;//скорость противника

    public Transform[] waypoints;//массив точек следования

    int currentWaypoint = 0; //теущая точка следования

    public float curHP; //текущее здоровье противника
    public float maxHp; // начальное максимальное здоровье противника, присваивается текущему и увеличивается при увеличении волны,
                       //присваивается в скприпте GameManager.cs

    public int dmg = 1; // урон для базы

    public Image sliderHp; // слайдер жизни

    public GameObject sliderLook; // взгляд слайдера в 3д

    void Start()
    {
        StartSetting();
    }

    void Update()
    {
        SliderHP();
    }

    private void FixedUpdate()
    {
        AIEnemy();
    }

    public void StartSetting()
    {
        curHP = maxHp;
        waypoints = GameManager.instance.way; // присваение пути
        sliderLook.transform.parent.SetParent(null); // обнуление LookAtt для слаедера 
    }

    //интелект противника
    void AIEnemy()
    {
        // навигация движения по точкам 
        if(currentWaypoint < waypoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position,Time.deltaTime * speed);
            transform.LookAt(waypoints[currentWaypoint].position);
            
            if(Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.5f)
            {
                currentWaypoint++;
            }
        }

        //смерть
        if (curHP <= 0)
        {
            GameManager.instance.gold += Random.Range(10, 20) + GameManager.instance.factorGold;
            GameManager.instance.kills++;
            GameManager.instance.waveIndex--;

            Destroy(gameObject);
        }
    }
    // настройка сладера HP 
    public void SliderHP()
    {
        sliderHp.fillAmount = curHP / maxHp;
        sliderLook.transform.parent.position = transform.position + Vector3.up;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // попадание снаряда
        if(other.gameObject.tag == "projectile")
        {
            curHP -= other.gameObject.GetComponent<Projectile>().dgm;
        }
        // дошел до базы
        if (other.gameObject.tag == "base")
        {
            GameManager.instance.waveIndex--;
            GameManager.instance.baseHp -= dmg;
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if(sliderLook != null)
            Destroy(sliderLook.transform.parent.gameObject);
    }
}
