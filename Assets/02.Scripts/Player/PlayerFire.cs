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
    public GameObject SubBulletPrefab; // 총알 프리팹


    // 목표: 태어날 때 풀에다가 메인 총알을 (풀 사이즈)개 생성한다.
    // 속성:
    // - 풀 사이즈
    public int PoolSize = 20;
    // - 오브젝트(총알) 풀
    private List<GameObject> _bulletPool = null;
    private List<GameObject> _subBulletPool = null;
    // 순서:
    // 1. 태어날 때: Awake
    private void Awake()
    {
        // 2. 오브젝트 풀 할당해주고
        _bulletPool = new List<GameObject>();

        // 3. 총알 프리팹으로부터 총알을 풀 사이즈만큼 생성해준다.
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab);

            // 4. 생성한 총알을 풀에다가 넣는다.
            _bulletPool.Add(bullet);

            bullet.SetActive(false);
        }

        _subBulletPool = new List<GameObject>();
        for(int i = 0;i < PoolSize; i++)
        {
            GameObject subBullet = Instantiate(SubBulletPrefab);
            _subBulletPool.Add(subBullet);
            subBullet.SetActive(false);
        }
    }


    [Header("총구 들")]
    // public GameObject[] Muzzles; // 총구 들
    public List<GameObject> MuzzlesList = new List<GameObject>();
    // public GameObject[] SubMuzzles; // 총구 들
    public List<GameObject> SubMuzzlesList = new List<GameObject>();

    
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
              
              /*
              // 목표 : 총구 개수 만큼 총알을 만들고, 만든 총알의 위치를 각 총구의 위치로 바꾼다.
              for (int i = 0; i < MuzzlesList.Count; i++)
              {
                    // 2. 프리팹으로부터 총알을 만든다.
                    GameObject bullet = Instantiate(BulletPrefab);
                    GameObject subBullet = Instantiate(SubBulletPrefab);

                    // 3. 만든 총알의 위치를 총구의 위치로 바꾼다.
                    bullet.transform.position = MuzzlesList[i].transform.position;
                    subBullet.transform.position = SubMuzzlesList[i].transform.position;
              }
              */
              

            // 목표: 총구 개수 만큼 총알을 풀에서 꺼내쓴다.
            // 순서:
            for (int i = 0; i < MuzzlesList.Count; i++)
            {
                // 1. 꺼져 있는 총알을 찾아 꺼낸다.
                GameObject bullet = null;
                foreach(GameObject b in _bulletPool)
                {
                    if (b.activeInHierarchy == false)
                    {
                        bullet = b;
                        break;  // 찾았기 때문에 그 뒤까지 찾을 필요가 없다.
                    }
                }

                // 2. 꺼낸 총알의 위치를 각 총구의 위치로 바꾼다.
                bullet.transform.position = MuzzlesList[i].transform.position;

                // 3. 총알을 킨다.(발사한다.)
                bullet.SetActive(true);
            }

            for (int i = 0;i < SubMuzzlesList.Count; i++)
            {
                GameObject subBullet = null;
                foreach (GameObject b in _subBulletPool)
                {
                    if(b.activeInHierarchy == false)
                    {
                        subBullet = b;
                        break;
                    }
                }
                subBullet.transform.position = SubMuzzlesList[i].transform.position;
                subBullet.SetActive(true);
            }
        } 
    }
}
