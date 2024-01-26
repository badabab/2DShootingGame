using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public enum EnemyType
{
    Basic,
    Target,
    Follow,
    Horizon
}

public class Enemy : MonoBehaviour
{
    // 목표: 적을 아래로 이동시키고 싶다.
    // 속성:
    // - 속력
    public float Speed = 3f;
    public int Health = 2;

    public EnemyType EType;

    private Vector2 _dir;

    private GameObject _target;

    public GameObject HealthItemPrefab;
    public GameObject SpeedItemPrefab;

    public Animator MyAnimator;

    public AudioSource BulletSource;

    public GameObject ExplosionVFXPrefab;

    private void Start()
    {       
        // 캐싱: 자주 쓰는 데이터를 더 가까운 장소에 저장해두고 필요할 때 가져다 쓰는 거
        // 시작할 때 플레이어를 찾아서 기억해둔다.
        _target = GameObject.Find("Player");
        MyAnimator = GetComponent<Animator>();

        GameObject bulletHit = GameObject.Find("BulletSource");
        BulletSource = bulletHit.GetComponent<AudioSource>();

        if (EType == EnemyType.Target)
        {
            // 1. 플레이어의 위치를 확인하다.
            //GameObject target = GameObject.Find("Player"); // 이름으로 찾기
            //GameObject.FindGameObjectWithTag("Player"); // 태그로 찾기

            
            // 2. 이동할 방향을 구한다. (target - me)
            _dir = _target.transform.position - this.transform.position;
            _dir.Normalize();

            // 1. 각도를 구한다.
            // tan@ = y/x     -> @ = y/x * atan
            float radian = Mathf.Atan2(_dir.y, _dir.x);
            //Debug.Log(radian);   // 호도법 -> 라디안 값
            float degree = radian * Mathf.Rad2Deg;  // degree 값
            //Debug.Log(degree);
            
            // 2. 각도에 맞게 회전한다.
            transform.rotation = Quaternion.Euler (new Vector3(0, 0, degree + 90));
            

            // transform.LookAt(_target.transform); // 3D에서는 LookAt 많이 사용함, 2D에서는 오류
        }
        else if(EType == EnemyType.Horizon)
        {
            _dir = Vector2.right;
        }
        else
        {
            _dir = Vector2.down;
        }
    }

    void Update()
    {
        if (EType == EnemyType.Follow)
        {
            //GameObject target = GameObject.Find("Player");
            _dir = _target.transform.position - this.transform.position;
            _dir.Normalize();


            float radian = Mathf.Atan2(_dir.y, _dir.x);
            float degree = radian * Mathf.Rad2Deg;  

            // transform.rotation = Quaternion.Euler(new Vector3(0, 0, degree + 90));
            transform.eulerAngles = new Vector3(0, 0, degree + 90);
        }

        // 구현 순서
        // 1. 방향 구하기
        //Vector2 dir = new Vector2(0, -1);
        //Vector2 dir = Vector2.down;

        // 2. 이동 시킨다.
        transform.position += (Vector3)(_dir * Speed) * Time.deltaTime;
    }

    // 목표: 충돌하면 적과 플레이어를 삭제하고 싶다.
    // 구현 순서:
    // 1. 만약에 충돌이 일어나면
    // 2. 적과 플레이어를 삭제한다.

    // 충돌이 일어나면 호출되는 이벤트 함수
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌을 시작했을 때
        // Debug.Log("Enter");

        // 충돌한 게임 오브젝트의 태그를 확인
        // Debug.Log(collision.collider.tag);  // Player or Bullet

        if (collision.collider.tag == "Player")
        {
            Player player = collision.collider.GetComponent<Player>();
            player.Health -= 1;

            Debug.Log($"Player Health: {player.Health}");
           
            Death();    // 나 죽자(Enemy)

            if (player.Health <= 0)
            {
                Destroy(collision.collider.gameObject);
            }
        }
        else if (collision.collider.tag == "Bullet")
        {
            Bullet bullet = collision.collider.GetComponent<Bullet>();
            /*
            if (bullet.BType == BulletType.Main)
            {
                Health -= 2;
            }
            else if (bullet.BType == BulletType.Sub)
            {
                Health -= 1;
            }
            */

            switch (bullet.BType)
            {
                case BulletType.Main:
                {
                    Health -= 2;
                    break;
                }
                case BulletType.Sub:
                {
                    Health -= 1;
                    break;
                }
            }

            if (Health <= 0)
            {              
                Death();    // 나 죽자(Enemy)
                MakeItem();
            }
            else
            {
                 MyAnimator.Play("Hit");
                 BulletSource.Play();   // 적이 총맞을 때 소리
                
            }
            // 너 죽고(Player)
            Destroy(collision.collider.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌 중일 때 매번
        // Debug.Log("Stay");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌이 끝났을 때
        // Debug.Log("Exit");
    }

    // 1. 만약에 적을 잡으면?
    public void Death()
    {
        // 나 죽자(Enemy)
        Destroy(this.gameObject);
        GameObject vfx = Instantiate(ExplosionVFXPrefab);
        vfx.transform.position = this.transform.position;
        
        // 목표: 스코어를 증가시키고 싶다.
        // 1. 씬에서 ScoreManager 게임 오브젝트를 찾아온다.
        GameObject smGameObject = GameObject.Find("ScoreManager");
        // 2. ScoreManager 게임 오브젝트에서 ScoreManager 스크립트 컴포넌트를 얻어온다.
        ScoreManager scoreManager = smGameObject.GetComponent<ScoreManager>();
        // 3. 컴포넌트의 Score 속성을 증가시킨다.
        int score = scoreManager.GetScore();
        scoreManager.SetScore(score + 1);
        //Debug.Log(scoreManager.GetScore());
    }

    public void MakeItem()
    {
        // 목표: 50% 확률로 체력 올려주는 아이템, 50% 확률로 이동속도 올려주는 아이템
        //Item item = collision.collider.GetComponent<Item>();
        GameObject item = null;
        if (Random.Range(0, 2) == 0)
        {
            // 체력 올려주는 아이템 생성
            item = Instantiate(HealthItemPrefab);
        }
        else
        {
            // 이동속도 올려주는 아이템 생성
            item = Instantiate(SpeedItemPrefab);
        }
        // 위치를 나(Enemy)의 위치로 수정
        item.transform.position = this.transform.position;
    }
}