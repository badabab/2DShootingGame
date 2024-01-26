using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BulletType  // �Ѿ� Ÿ�Կ� ���� ������(����� ����ϱ� ���� �ϳ��� �̸����� �׷�ȭ�ϴ� ��)
{
    Main = 0,
    Sub = 1,
    Pet = 2,
    Guide = 3
}

public class Bullet : MonoBehaviour
{
    // public int BType = 0; // 0 : ���Ѿ�, 1 : �����Ѿ�, 2 : ���� ��� �Ѿ�
    public BulletType BType = BulletType.Main;

    // [�Ѿ� �̵� ����]
    // ��ǥ: �Ѿ��� ���� ��� �̵��ϰ� �ʹ�.
    // �Ӽ�:
    // - �ӷ�
    // ���� ����
    // 1. �̵��� ������ ���Ѵ�.
    // 2. �̵��Ѵ�.

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
            // 1. �̵��� ������ ���Ѵ�.
            // ���� Ÿ���� ���ٸ�
            if (_target == null || _target.activeInHierarchy == false) // Ÿ���� ���ų� �����Ű�� ������
            {
                _target = GameObject.FindGameObjectWithTag("Enemy");
            }

            // ã�Ҵµ��� ���ٸ� ���ʹ���
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
