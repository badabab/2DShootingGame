using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    // ����: �����ð����� ���� ���������κ��� �����ؼ� �� ��ġ�� ���� ���� �ʹ�.
    // �ʿ� �Ӽ�:
    // - �� ������
    // - �����ð�
    // - ����ð�
    // ���� ����:
    // 1. �ð��� �帣�ٰ�
    // 2. ���࿡ �ð��� �����ð�(1��)�� �Ǹ�
    // 3. ���������κ��� ���� �����Ѵ�.
    // 4. ������ ���� ��ġ�� �� ��ġ�� �ٲ۴�.

    [Header("�� ������")]
    public GameObject EnemyPrefab;          // Basic
    public GameObject EnemyPrefabTarget;    // Target
    public GameObject EnemyPrefabFollow;    // Follow
    public GameObject EnemyPrefabHorizon;   // Horizon

    [Header("Ÿ�̸�")]
    public float Timer = 0f;
    public float Spawn_Time = 1f;

    // ��ǥ: �� ���� �ð��� �����ϰ� �ϰ� �ʹ�.
    // �ʿ� �Ӽ�:
    // - �ּ� �ð�
    // - �ִ� �ð�
    [Header("�� ���� �ð�")]
    public float MinTime = 0.5f;
    public float MaxTime = 1.5f;

    private void Start()
    {
        // ������ �� �� ���� �ð��� �������� �����Ѵ�.
        Spawn_Time = Random.Range(MinTime, MaxTime);
    }

    void Update()
    {
        int probability = Random.Range(0, 10); // 0 ~ 9 ���� �������� �ϳ�
        Timer += Time.deltaTime;
        if (Timer >= Spawn_Time)
        {
            GameObject enemy = null;
                       
            if (probability < 1)
            {
                enemy = Instantiate(EnemyPrefabFollow);
                enemy.transform.position = this.transform.position;
            }
            else if (probability < 3)
            {
                enemy = Instantiate(EnemyPrefabHorizon);
                enemy.transform.position = new Vector2(-4, 3);
            }
            else if (probability < 5)
            {         
                enemy = Instantiate(EnemyPrefabTarget);
                enemy.transform.position = this.transform.position;
            }
            else
            {
                enemy = Instantiate(EnemyPrefab);
                enemy.transform.position = this.transform.position;
            } 
                       
            // GameObject enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
                        
            Timer = 0f; // Ÿ�̸� �ʱ�ȭ
            Spawn_Time = Random.Range(MinTime, MaxTime); // �� �����ð� ����
        }      
    }
}
