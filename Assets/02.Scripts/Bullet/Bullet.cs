using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BulletType  // 총알 타입에 대한 열거형(상수를 기억하기 좋게 하나의 이름으로 그룹화하는 것)
{
    Main = 0,
    Sub = 1,
    Pet = 2,
    Guide = 3
}

public class Bullet : MonoBehaviour
{
    // public int BType = 0; // 0 : 주총알, 1 : 보조총알, 2 : 펫이 쏘는 총알
    public BulletType BType = BulletType.Main;

    // [총알 이동 구현]
    // 목표: 총알이 위로 계속 이동하고 싶다.
    // 속성:
    // - 속력
    // 구현 순서
    // 1. 이동할 방향을 구한다.
    // 2. 이동한다.

    public float Speed = 5f;
    private GameObject _target;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy");
    }

    void Update()
    {
        if (BType == BulletType.Main)
        {
            Vector2 dir = Vector2.up;
            // Vector2 dir = new Vector2(0, 1);
            transform.position += (Vector3)(dir * Speed) * Time.deltaTime;
        }
        
        else if (BType == BulletType.Sub)
        {
            // 1. 이동할 방향을 구한다.
            // 만약 타겟이 없다면
            if (_target == null || _target.activeInHierarchy == false) // 타겟이 없거나 히어라키가 없으면
            {
                _target = GameObject.FindGameObjectWithTag("Enemy");
            }

            // 찾았는데도 없다면 위쪽방향
            if (_target == null)
            {
                transform.position += (Vector3)(Vector2.up * Speed) * Time.deltaTime;
            }
            else
            {
                Vector3 newPosition = Vector3.MoveTowards(transform.position, _target.transform.position, Speed * Time.deltaTime);
                transform.position = newPosition;
            }
        }       
    }
}
