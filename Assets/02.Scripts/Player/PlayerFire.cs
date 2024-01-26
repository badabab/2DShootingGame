using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // [총알 발사 제작]
    // 목표: 총알을 만들어서 발사하고 싶다.
    // 속성:
    // - 총알 프리팹
    // - 총구
    // 구현 순서
    // 1. 발사 버튼을 누르면
    // 2. 프리팹으로부터 총알을 동적으로 만들고,
    // 3. 만든 총알의 위치를 총구의 위치로 바꾼다.

    [Header("총알 프리팹")] // 링크, 할당
    public GameObject BulletPrefab; // 총알 프리팹
    [Header("총구 들")]
    public GameObject[] Muzzles; // 총구 들

    [Header("서브총알 프리팹")] // 링크, 할당
    public GameObject SubBulletPrefab; // 총알 프리팹
    [Header("서브총구 들")]
    public GameObject[] SubMuzzles; // 총구 들

    [Header("타이머")]
    public float Timer = 10f;
    public const float Cool_Time = 0.6f;

    [Header("자동 모드")]
    public bool AutoMode = false;

    public AudioSource FireSource;

    public GameObject BoomPrefab;
    public float Boom_Timer = 0f;
    public float Boom_Cool_Time = 5f;

    private void Start()
    {
        Timer = 0f; // 처음 시작할 때는 타이머 0초
        AutoMode = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("자동 공격 모드");
            AutoMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("수동 공격 모드");
            AutoMode = false;
        }

        Boom_Timer -= Time.deltaTime;
        
        if (Boom_Timer <= 0 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Boom");
            // 붐 타이머 시간을 다시 쿨타임으로
            Boom_Timer = Boom_Cool_Time;

            // 붐 프리팹을 씬으로 생성한다.
            GameObject boom = Instantiate(BoomPrefab);
            boom.transform.position = new Vector2 (0, 1.6f);         
        }
              
        // 타이머 계산
        Timer -= Time.deltaTime;

        // 1. 타이머가 0보다 작은 상태에서 발사 버튼을 누르면
        bool ready = AutoMode || Input.GetKeyDown(KeyCode.Space);
        if (Timer <= 0 && ready)
        {
            FireSource.Play();
              // 타이머 초기화
              Timer = Cool_Time;

              // 목표 : 총구 개수 만큼 총알을 만들고, 만든 총알의 위치를 각 총구의 위치로 바꾼다.
              for (int i = 0; i < Muzzles.Length; i++)
              {
                    // 2. 프리팹으로부터 총알을 만든다.
                    GameObject bullet = Instantiate(BulletPrefab);
                    GameObject subBullet = Instantiate(SubBulletPrefab);

                    // 3. 만든 총알의 위치를 총구의 위치로 바꾼다.
                    bullet.transform.position = Muzzles[i].transform.position;
                    subBullet.transform.position = SubMuzzles[i].transform.position;
              }
        } 
    }
}
