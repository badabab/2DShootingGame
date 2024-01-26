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
    // ��ǥ: ���� �Ʒ��� �̵���Ű�� �ʹ�.
    // �Ӽ�:
    // - �ӷ�
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
        // ĳ��: ���� ���� �����͸� �� ����� ��ҿ� �����صΰ� �ʿ��� �� ������ ���� ��
        // ������ �� �÷��̾ ã�Ƽ� ����صд�.
        _target = GameObject.Find("Player");
        MyAnimator = GetComponent<Animator>();

        GameObject bulletHit = GameObject.Find("BulletSource");
        BulletSource = bulletHit.GetComponent<AudioSource>();

        if (EType == EnemyType.Target)
        {
            // 1. �÷��̾��� ��ġ�� Ȯ���ϴ�.
            //GameObject target = GameObject.Find("Player"); // �̸����� ã��
            //GameObject.FindGameObjectWithTag("Player"); // �±׷� ã��

            
            // 2. �̵��� ������ ���Ѵ�. (target - me)
            _dir = _target.transform.position - this.transform.position;
            _dir.Normalize();

            // 1. ������ ���Ѵ�.
            // tan@ = y/x     -> @ = y/x * atan
            float radian = Mathf.Atan2(_dir.y, _dir.x);
            //Debug.Log(radian);   // ȣ���� -> ���� ��
            float degree = radian * Mathf.Rad2Deg;  // degree ��
            //Debug.Log(degree);
            
            // 2. ������ �°� ȸ���Ѵ�.
            transform.rotation = Quaternion.Euler (new Vector3(0, 0, degree + 90));
            

            // transform.LookAt(_target.transform); // 3D������ LookAt ���� �����, 2D������ ����
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

        // ���� ����
        // 1. ���� ���ϱ�
        //Vector2 dir = new Vector2(0, -1);
        //Vector2 dir = Vector2.down;

        // 2. �̵� ��Ų��.
        transform.position += (Vector3)(_dir * Speed) * Time.deltaTime;
    }

    // ��ǥ: �浹�ϸ� ���� �÷��̾ �����ϰ� �ʹ�.
    // ���� ����:
    // 1. ���࿡ �浹�� �Ͼ��
    // 2. ���� �÷��̾ �����Ѵ�.

    // �浹�� �Ͼ�� ȣ��Ǵ� �̺�Ʈ �Լ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� �������� ��
        // Debug.Log("Enter");

        // �浹�� ���� ������Ʈ�� �±׸� Ȯ��
        // Debug.Log(collision.collider.tag);  // Player or Bullet

        if (collision.collider.tag == "Player")
        {
            Player player = collision.collider.GetComponent<Player>();
            player.Health -= 1;

            Debug.Log($"Player Health: {player.Health}");
           
            Death();    // �� ����(Enemy)

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
                Death();    // �� ����(Enemy)
                MakeItem();
            }
            else
            {
                 MyAnimator.Play("Hit");
                 BulletSource.Play();   // ���� �Ѹ��� �� �Ҹ�
                
            }
            // �� �װ�(Player)
            Destroy(collision.collider.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // �浹 ���� �� �Ź�
        // Debug.Log("Stay");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �浹�� ������ ��
        // Debug.Log("Exit");
    }

    // 1. ���࿡ ���� ������?
    public void Death()
    {
        // �� ����(Enemy)
        Destroy(this.gameObject);
        GameObject vfx = Instantiate(ExplosionVFXPrefab);
        vfx.transform.position = this.transform.position;
        
        // ��ǥ: ���ھ ������Ű�� �ʹ�.
        // 1. ������ ScoreManager ���� ������Ʈ�� ã�ƿ´�.
        GameObject smGameObject = GameObject.Find("ScoreManager");
        // 2. ScoreManager ���� ������Ʈ���� ScoreManager ��ũ��Ʈ ������Ʈ�� ���´�.
        ScoreManager scoreManager = smGameObject.GetComponent<ScoreManager>();
        // 3. ������Ʈ�� Score �Ӽ��� ������Ų��.
        scoreManager.Score += 1;
        //Debug.Log(scoreManager.Score);

        // ��ǥ: ���ھ ȭ�鿡 ǥ���Ѵ�.
        scoreManager.ScoreTextUI.text = $"���� : {scoreManager.Score}";


        // ��ǥ: �ְ� ������ �����ϰ� UI�� ǥ���ϰ� �ʹ�.
        // 1. ���࿡ ���� ������ �ְ� �������� ũ�ٸ�
        if (scoreManager.Score > scoreManager.BestScore)
        {
            // 2. �ְ� ������ �����ϰ�
            scoreManager.BestScore = scoreManager.Score;

            // ��ǥ: �ְ� ������ ����
            // 'PlayerPrefs' Ŭ������ ���
            // -> ���� 'Ű(Key)'�� '��(Value)' ���·� �����ϴ� Ŭ�����Դϴ�.
            // ������ �� �ִ� ������Ÿ��: int, float, string
            // Ÿ�Ժ��� ����/�ε尡 ������ Set/Get �޼��尡 �ִ�.
            PlayerPrefs.SetInt("BestScore", scoreManager.BestScore);

            // 3. UI�� ǥ���Ѵ�.
            scoreManager.BestScoreTextUI.text = $"�ְ� ���� : {scoreManager.BestScore}";
        }     
    }

    public void MakeItem()
    {
        // ��ǥ: 50% Ȯ���� ü�� �÷��ִ� ������, 50% Ȯ���� �̵��ӵ� �÷��ִ� ������
        //Item item = collision.collider.GetComponent<Item>();
        GameObject item = null;
        if (Random.Range(0, 2) == 0)
        {
            // ü�� �÷��ִ� ������ ����
            item = Instantiate(HealthItemPrefab);
        }
        else
        {
            // �̵��ӵ� �÷��ִ� ������ ����
            item = Instantiate(SpeedItemPrefab);
        }
        // ��ġ�� ��(Enemy)�� ��ġ�� ����
        item.transform.position = this.transform.position;
    }
}