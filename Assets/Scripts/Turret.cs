using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("ShootingMode")]
    public Transform shootingPos; //направление снаряда
    public int damage; // урон снаряда
    public float speedProjectile = 150; // скорость снаряда
    public GameObject projectile; //префаб снаряда
    public float shootingDelay; //интервал между вытрелом
    bool isShooting; //сейчас стреляю

    [Header("FixedEnemy")]
    public Transform fixEnemyPos; //захват позиции противника
    Transform target; //позиция текущей цели
    public bool fixEnemy; //вижу цель
    public GameObject curEnemy; //текущая цель
    public Transform wait;//взгляд турелли между волнами на ворота противника

    [Header("UpgreadMode")]
    public int price; // текущая цена улучшения
    public int turretlevel = 1; // уровень туррели
    public int checkLevel = 1; // текущий уровень туррели
    SphereCollider Sphere; // радиус поиска противников, увеличивается с улучшением туррели

    void Start()
    {
        StartSetting();
    }

    private void FixedUpdate()
    {
        AITurret();
    }

    public void StartSetting()
    {
        Sphere = gameObject.GetComponent<SphereCollider>();
        Sphere.radius = 6;
    }

    //интелект туррели
    void AITurret()
    {
        projectile.GetComponent<Projectile>().dgm = damage;
        speedProjectile = projectile.GetComponent<Projectile>().speed;

        if (!curEnemy)
        {
            fixEnemy = false;
        }

        if (target != null)
        {
            fixEnemyPos.transform.LookAt(target);

            if (!isShooting)
            {
                StartCoroutine(Shooting());
            }
        }
        

        
        if(turretlevel == 1)
        {
            price = 70;
            damage = 20;
            shootingDelay = 0.9f;
            //speedProjectile = 100f;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        if(turretlevel == 2)
        {
            checkLevel = turretlevel;
            price = 150;
            damage = 30;
            shootingDelay = 0.7f;
            //speedProjectile = 100f;
            gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
        if (turretlevel == 3)
        {
            damage = 45;
            shootingDelay = 0.5f;
            //speedProjectile = 100f;
            gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
        }
    }


    //фиксация противника триггером
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy")&& !fixEnemy)
        {
            target = other.gameObject.transform;
            curEnemy = other.gameObject;
            fixEnemy = true;
            
        }
    }
    //потеря противника
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy") && other.gameObject == curEnemy)
        {
            fixEnemy = false;
            
        }
    }

    //улучшение
    public void Upgread()
    {
        if(turretlevel <= 2 && checkLevel == turretlevel && !GameManager.instance.gameOver)
        {
            if (GameManager.instance.gold >= price)
            {
                GameManager.instance.gold -= price;
                turretlevel++;
                Sphere.radius++;
            }
        }       
    }

    //стрельба
    IEnumerator Shooting()
    {
        isShooting = true;

        yield return new WaitForSeconds(shootingDelay);
        GameObject project = Instantiate(projectile, shootingPos.position, Quaternion.identity) as GameObject;
        project.GetComponent<Projectile>().target = target;
        isShooting = false;
    }
    //переход взгляда на ворота противника
    IEnumerator DelayLookAtt()
    {
        yield return new WaitForSeconds(1.5f);
        fixEnemyPos.transform.LookAt(wait.position);
    }
}
