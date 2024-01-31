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
    private List<Bullet> _bulletPool = null;

    // 순서:
    // 1. 태어날 때: Awake
    private void Awake()
    {
        // 2. 오브젝트 풀 할당해주고
        _bulletPool = new List<Bullet>();

        // 3. 총알 프리팹으로부터 총알을 풀 사이즈만큼 생성해준다.
        // 3-1. 메인 총알
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.SetActive(false);

            // 4. 생성한 총알을 풀에다가 넣는다.
            _bulletPool.Add(bullet.GetComponent<Bullet>());          
        }

        // 3-2. 서브 총알
        //_subBulletPool = new List<GameObject>();
        for(int i = 0;i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(SubBulletPrefab);
            bullet.SetActive(false);
            _bulletPool.Add(bullet.GetComponent<Bullet>());          
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
    public float Boom_Cool_Time = 8f;

    private void Start()
    {
        // 전처리 단계: 코드가 컴파일(해석) 되기 전에 미리 처리되는 단계
        // 전처리문 코드를 이용해서 미리 처리되는 코드를 작성할 수 있다.
        // C#의 모든 전처리 코드는 '#'으로 시작한다. (#if, #elif, #endif)

#if UNITY_EDITOR || UNITY_STANDALONE
        GameObject.Find("Joystick canvas XYBZ").SetActive(false);
#endif

#if UNITY_ANDROID
        Debug.Log("안드로이드 입니다.");
#endif



        Timer = 0f; // 처음 시작할 때는 타이머 0초
        AutoMode = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && AutoMode == false)
        {
            AutoModeControl();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && AutoMode == true)
        {
            AutoModeControl();
        }

        Boom_Timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Boom();
        }
        
              
        // 타이머 계산
        Timer -= Time.deltaTime;

        // 1. 타이머가 0보다 작은 상태에서 발사 버튼을 누르면
        bool ready = AutoMode || Input.GetKeyDown(KeyCode.Space);
        if (Timer <= 0 && ready)
        {
            BulletFire();         
        } 
    }

    private void Boom()
    {  
        if (Boom_Timer <= 0)
        {
            Debug.Log("Boom");
            // 붐 타이머 시간을 다시 쿨타임으로
            Boom_Timer = Boom_Cool_Time;

            // 붐 프리팹을 씬으로 생성한다.
            GameObject boom = Instantiate(BoomPrefab);
            boom.transform.position = new Vector2(0, 1.6f);
        }
    }

    private void AutoModeControl()
    {
        if (AutoMode == true)
        {
            AutoMode = false;
            Debug.Log("수동 공격 모드");
        }
        else if (AutoMode == false)
        {
            AutoMode = true;
            Debug.Log("자동 공격 모드");
        }
    }

    private void BulletFire()
    {
        FireSource.Play();
        // 타이머 초기화
        Timer = Cool_Time;

        // 목표: 총구 개수 만큼 총알을 풀에서 꺼내쓴다.
        // 순서:
        for (int i = 0; i < MuzzlesList.Count; i++)
        {
            // 1. 꺼져 있는 총알을 찾아 꺼낸다.
            Bullet bullet = null;
            foreach (Bullet b in _bulletPool)
            {
                // 만약에 꺼져있고 && 메인총알이라면
                if (b.gameObject.activeInHierarchy == false && b.BType == BulletType.Main)
                {
                    bullet = b;
                    break;  // 찾았기 때문에 그 뒤까지 찾을 필요가 없다.
                }
            }

            // 2. 꺼낸 총알의 위치를 각 총구의 위치로 바꾼다.
            bullet.transform.position = MuzzlesList[i].transform.position;

            // 3. 총알을 킨다.(발사한다.)
            bullet.gameObject.SetActive(true);
        }

        for (int i = 0; i < SubMuzzlesList.Count; i++)
        {
            Bullet subBullet = null;
            foreach (Bullet b in _bulletPool)
            {
                if (b.gameObject.activeInHierarchy == false && b.BType == BulletType.Sub)
                {
                    subBullet = b;
                    break;
                }
            }
            subBullet.transform.position = SubMuzzlesList[i].transform.position;
            subBullet.gameObject.SetActive(true);
        }
    }

    // 총알 발사
    public void OnClickXButton()
    {
        Debug.Log("X버튼이 클릭되었습니다.");
        if (Timer <= 0)
        {
            BulletFire();
        }      
    }

    // 자동 공격 on/off
    public void OnClickYButton()
    {
        Debug.Log("Y버튼이 클릭되었습니다.");
        AutoModeControl();
    }

    // 궁극기 사용
    public void OnClickBButton()
    {
        Debug.Log("B버튼이 클릭되었습니다.");
        Boom();
    }
}
