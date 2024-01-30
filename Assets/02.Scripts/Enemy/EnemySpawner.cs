using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    // 역할: 일정시간마다 적을 프리팹으로부터 생성해서 내 위치에 갖다 놓고 싶다.
    // 필요 속성:
    // - 적 프리팹
    // - 일정시간
    // - 현재시간
    // 구현 순서:
    // 1. 시간이 흐르다가
    // 2. 만약에 시간이 일정시간(1초)이 되면
    // 3. 프리팹으로부터 적을 생성한다.
    // 4. 생성한 적의 위치를 내 위치로 바꾼다.

    [Header("적 프리팹")]
    public GameObject EnemyPrefab;          // Basic
    public GameObject EnemyPrefabTarget;    // Target
    public GameObject EnemyPrefabFollow;    // Follow
    public GameObject EnemyPrefabHorizon;   // Horizon

    [Header("타이머")]
    public float Timer = 0f;
    public float Spawn_Time = 1f;

    // 목표: 적 생성 시간을 랜덤하게 하고 싶다.
    // 필요 속성:
    // - 최소 시간
    // - 최대 시간
    [Header("적 생성 시간")]
    public float MinTime = 0.5f;
    public float MaxTime = 1.5f;

    public int PoolSize = 5;
    private List<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new List<Enemy>();

        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefab);
            enemyObject.SetActive(false);
            _enemyPool.Add(enemyObject.GetComponent<Enemy>());
        }
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefabTarget);
            enemyObject.SetActive(false);
            _enemyPool.Add(enemyObject.GetComponent<Enemy>());
        }
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefabFollow);
            enemyObject.SetActive(false);
            _enemyPool.Add(enemyObject.GetComponent<Enemy>());
        }
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefabHorizon);
            enemyObject.SetActive(false);
            _enemyPool.Add(enemyObject.GetComponent<Enemy>());
        }
    }

    private void Start()
    {
        // 시작할 때 적 생성 시간을 랜덤으로 설정한다.
        Spawn_Time = Random.Range(MinTime, MaxTime);
    }

    void Update()
    {
        int probability = Random.Range(0, 10); // 0 ~ 9 사이 랜덤숫자 하나
        Timer += Time.deltaTime;
        if (Timer >= Spawn_Time)
        {
            Enemy enemy = null;
                       
            if (probability < 1)
            {
                //enemy = Instantiate(EnemyPrefabFollow);
                foreach (Enemy e in _enemyPool)
                {
                    if (e.gameObject.activeInHierarchy == false && e.EType == EnemyType.Follow)
                    {
                        enemy = e;
                        break;
                    }
                }
                enemy.transform.position = this.transform.position;
                enemy.gameObject.SetActive(true);
            }
            else if (probability < 3)
            {
                //enemy = Instantiate(EnemyPrefabHorizon);
                foreach (Enemy e in _enemyPool)
                {
                    if (e.gameObject.activeInHierarchy == false && e.EType == EnemyType.Horizon)
                    {
                        enemy = e;
                        break;
                    }
                }
                enemy.transform.position = new Vector2(-4, 3);
                enemy.gameObject.SetActive(true);
            }
            else if (probability < 5)
            {         
                //enemy = Instantiate(EnemyPrefabTarget);
                foreach (Enemy e in _enemyPool)
                {
                    if (e.gameObject.activeInHierarchy == false && e.EType == EnemyType.Target)
                    {
                        enemy = e;
                        break;
                    }
                }
                enemy.transform.position = this.transform.position;
                enemy.gameObject.SetActive(true);
            }
            else
            {
                //enemy = Instantiate(EnemyPrefab);
                foreach (Enemy e in _enemyPool)
                {
                    if (e.gameObject.activeInHierarchy == false && e.EType == EnemyType.Basic)
                    {
                        enemy = e;
                        break;
                    }
                }
                enemy.transform.position = this.transform.position;
                enemy.gameObject.SetActive(true);
            } 
                       
            // GameObject enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
                        
            Timer = 0f; // 타이머 초기화
            Spawn_Time = Random.Range(MinTime, MaxTime); // 적 생성시간 랜덤
        }      
    }
}
