using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    /*
        목표 : 플레이어를 이동하고 싶다.
        필요 속성 :
         - 이동 속도   
     */

    public float Speed = 3f; // 이동 속도 : 초당 3unit만큼 이동하겠다.

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

    void Update()   // 매 프레임마다 호출
    {
        Move();
        SpeedUpDown();   
    }

    private void Move()
    {
        //transform.Translate(Vector2.up * Speed * Time.deltaTime);
        // (0, 1) * 3 -> (0, 3) * Time.deltaTime
        // deltaTime은 프레임 간 시간 간격을 반환한다.
        // 30fps : d-> 0.03초
        // 60fps : d-> 0.016초

        //순서:
        // 1. 키보드 입력을 받는다.
        // 연속적인 값, 부드러운 이동
        // float h = Input.GetAxis("Horizontal");  // 수평 입력값: -1.Of ~ 0f ~ +1.0f
        // float v = Input.GetAxis("Vertical");    // 수직 입력값: -1.0f ~ 0f ~ +1.0f (InputManager참고)

        // 불연속적인 값, 키보드 입력에 따라 즉각 이동
        float h = Input.GetAxisRaw("Horizontal");  // 수평 입력값: -1.Of, 0f, +1.0f
        float v = Input.GetAxisRaw("Vertical");    // 수직 입력값: -1.0f, 0f, +1.0f (InputManager참고)
                                                   // Debug.Log($"h : {h}, v : {v}");

        // 애니메이터에게 파라미터 값을 넘겨준다.
        MyAnimator.SetInteger("h", (int)h);

        // 2. 키보드 입력에 따라 이동할 방향을 계산한다.
        // Vector2 dir = Vector2.right * h + Vector2.up * v;
        // (1, 0) * h + (0, 1) * v = (h, v)

        // 방향을 각 성분으로 제작
        Vector2 dir = new Vector2(h, v);
        // Debug.Log($"정규화 전 : {dir.magnitude}");

        // 이동 방향을 정규화 (방향은 같지만 길이를 1로 만들어줌)
        dir = dir.normalized;
        // Debug.Log($"정규화 후 : {dir.magnitude}");

        // 3. 이동할 방향과 이동 속도에 따라 플레이어를 이동시킨다.
        // Debug.Log(Time.deltaTime);
        // transform.Translate(dir * Speed * Time.deltaTime);
        // 공식을 이용한 이동
        // 새로운 위치 = 현재 위치 + 속도 + 시간
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

        // 좌우 이동에 있어 반대쪽으로 나오게 하기
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
        transform.position = newPosition;   // 플레이어 위치 = 새로운 위치

        // 현재 위치 출력
        // Debug.Log(transform.position);
        // transform.position = new Vector2(0, 1);
    }

    private void SpeedUpDown()
    {
        // 키보드 E 누르면 스피드 1 up, Q 누르면 스피드 1 down
        // 목표 : Q/E 버튼을 누르면 속력을 바꾸고 싶다.
        // 속성
        // - 속력 (Speed)

        // 1. Q/E 버튼 입력을 판단한다.

        if (Input.GetKeyDown(KeyCode.E))
        {
            // 2. E버튼이 눌렸다면 스피드 1 업
            Speed++;
            Debug.Log(Speed);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // 3. Q버튼이 눌렸다면 스피드 1 다운
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
