using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("아이템 프리팹")]
    public GameObject ItemPrefab;

    [Header("타이머")]
    public float Timer = 0f;
    public float Spawn_Time = 2f;

    [Header("아이템 생성 시간")]
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
