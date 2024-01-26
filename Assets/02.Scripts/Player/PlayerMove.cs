using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    /*
        ��ǥ : �÷��̾ �̵��ϰ� �ʹ�.
        �ʿ� �Ӽ� :
         - �̵� �ӵ�   
     */

    public float Speed = 3f; // �̵� �ӵ� : �ʴ� 3unit��ŭ �̵��ϰڴ�.

    public const float MinX = -3f;
    public const float MaxX = 3f;
    public const float MinY = -6f;
    public const float MaxY = 0f;

    public Animator MyAnimator;

    public AudioSource HitSource;
    


    private void Awake()
    {
        MyAnimator = GetComponent<Animator>();
    }

    void Update()   // �� �����Ӹ��� ȣ��
    {
        Move();
        SpeedUpDown();   
    }

    private void Move()
    {
        //transform.Translate(Vector2.up * Speed * Time.deltaTime);
        // (0, 1) * 3 -> (0, 3) * Time.deltaTime
        // deltaTime�� ������ �� �ð� ������ ��ȯ�Ѵ�.
        // 30fps : d-> 0.03��
        // 60fps : d-> 0.016��

        //����:
        // 1. Ű���� �Է��� �޴´�.
        // �������� ��, �ε巯�� �̵�
        // float h = Input.GetAxis("Horizontal");  // ���� �Է°�: -1.Of ~ 0f ~ +1.0f
        // float v = Input.GetAxis("Vertical");    // ���� �Է°�: -1.0f ~ 0f ~ +1.0f (InputManager����)

        // �ҿ������� ��, Ű���� �Է¿� ���� �ﰢ �̵�
        float h = Input.GetAxisRaw("Horizontal");  // ���� �Է°�: -1.Of, 0f, +1.0f
        float v = Input.GetAxisRaw("Vertical");    // ���� �Է°�: -1.0f, 0f, +1.0f (InputManager����)
                                                   // Debug.Log($"h : {h}, v : {v}");

        // �ִϸ����Ϳ��� �Ķ���� ���� �Ѱ��ش�.
        MyAnimator.SetInteger("h", (int)h);

        // 2. Ű���� �Է¿� ���� �̵��� ������ ����Ѵ�.
        // Vector2 dir = Vector2.right * h + Vector2.up * v;
        // (1, 0) * h + (0, 1) * v = (h, v)

        // ������ �� �������� ����
        Vector2 dir = new Vector2(h, v);
        // Debug.Log($"����ȭ �� : {dir.magnitude}");

        // �̵� ������ ����ȭ (������ ������ ���̸� 1�� �������)
        dir = dir.normalized;
        // Debug.Log($"����ȭ �� : {dir.magnitude}");

        // 3. �̵��� ����� �̵� �ӵ��� ���� �÷��̾ �̵���Ų��.
        // Debug.Log(Time.deltaTime);
        // transform.Translate(dir * Speed * Time.deltaTime);
        // ������ �̿��� �̵�
        // ���ο� ��ġ = ���� ��ġ + �ӵ� + �ð�
        Vector2 newPosition = transform.position + (Vector3)(dir * Speed) * Time.deltaTime;

        /*
        if (newPosition.x < MinX)
        {
            newPosition.x = MinX;
        }
        else if (newPosition.x > MaxX)
        {
            newPosition.x = MaxX;
        }
        */

        // �¿� �̵��� �־� �ݴ������� ������ �ϱ�
        if (newPosition.x < MinX)
        {
            newPosition.x = MaxX;
        }
        else if (newPosition.x > MaxX)
        {
            newPosition.x = MinX;
        }

        // newPosition.y = Mathf.Max(MinY, newPosition.y);
        // newPosition.y = Mathf.Min(newPosition.y, MaxY);

        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);

        /*
        if (newPosition.y < MinY)
        {
            newPosition.y = MinY;
        }
        else if (newPosition.y > MaxY)
        {
            newPosition.y = MaxY;
        }
        */

        //Debug.Log(newPosition);
        transform.position = newPosition;   // �÷��̾� ��ġ = ���ο� ��ġ

        // ���� ��ġ ���
        // Debug.Log(transform.position);
        // transform.position = new Vector2(0, 1);
    }

    private void SpeedUpDown()
    {
        // Ű���� E ������ ���ǵ� 1 up, Q ������ ���ǵ� 1 down
        // ��ǥ : Q/E ��ư�� ������ �ӷ��� �ٲٰ� �ʹ�.
        // �Ӽ�
        // - �ӷ� (Speed)

        // 1. Q/E ��ư �Է��� �Ǵ��Ѵ�.

        if (Input.GetKeyDown(KeyCode.E))
        {
            // 2. E��ư�� ���ȴٸ� ���ǵ� 1 ��
            Speed++;
            Debug.Log(Speed);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // 3. Q��ư�� ���ȴٸ� ���ǵ� 1 �ٿ�
            Speed--;
            Debug.Log(Speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            HitSource.Play();
        }
        
    }
}
