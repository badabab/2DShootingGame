using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public float Boom_Destroy_Timer = 0f;
    public float Boom_Destroy_CoolTime = 3f;

    void Start()
    {
        Boom_Destroy_Timer = Boom_Destroy_CoolTime;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log(enemies.Length);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            enemy.Death();
            enemy.MakeItem();
        }
    }
    private void Update()
    {       
        Boom_Destroy_Timer -= Time.deltaTime;
       
        if (Boom_Destroy_Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Enemy enemy = otherCollider.GetComponent<Enemy>();
        if (otherCollider.CompareTag ("Enemy"))
        {
            enemy.Death();
            enemy.MakeItem();
        }
    }
}
