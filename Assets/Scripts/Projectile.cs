using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed; // скорость выстрела

    public Transform target; // текущий противник

    public int dgm; // урон снаряда, присваивается в скрипте Turret.cs

    void Update()
    {
        Shoot();
    }

    //выстрел 
    public void Shoot()
    {
        if (target)
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        else
        {
            Destroy(gameObject);
        }
    }

    //попадание в противника
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }
    }
}
