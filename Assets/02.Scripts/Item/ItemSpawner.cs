using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("������ ������")]
    public GameObject ItemPrefab;

    [Header("Ÿ�̸�")]
    public float Timer = 0f;
    public float Spawn_Time = 2f;

    [Header("������ ���� �ð�")]
    public float MinTime = 1f;
    public float MaxTime = 3f;

    private void Start()
    {
        Spawn_Time = Random.Range(MinTime, MaxTime);
    }
    private void Update()
    {
        Timer += Time.deltaTime;
        if  (Timer > Spawn_Time)
        {        
            GameObject item = Instantiate(ItemPrefab);
            item.transform.position = this.transform.position;

            Timer = 0f;
            Spawn_Time = Random.Range(MinTime, MaxTime);
        }        
    }
}
