using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // [�Ѿ� �߻� ����]
    // ��ǥ: �Ѿ��� ���� �߻��ϰ� �ʹ�.
    // �Ӽ�:
    // - �Ѿ� ������
    // - �ѱ�
    // ���� ����
    // 1. �߻� ��ư�� ������
    // 2. ���������κ��� �Ѿ��� �������� �����,
    // 3. ���� �Ѿ��� ��ġ�� �ѱ��� ��ġ�� �ٲ۴�.

    [Header("�Ѿ� ������")] // ��ũ, �Ҵ�
    public GameObject BulletPrefab; // �Ѿ� ������
    [Header("�ѱ� ��")]
    public GameObject[] Muzzles; // �ѱ� ��

    [Header("�����Ѿ� ������")] // ��ũ, �Ҵ�
    public GameObject SubBulletPrefab; // �Ѿ� ������
    [Header("�����ѱ� ��")]
    public GameObject[] SubMuzzles; // �ѱ� ��

    [Header("Ÿ�̸�")]
    public float Timer = 10f;
    public const float Cool_Time = 0.6f;

    [Header("�ڵ� ���")]
    public bool AutoMode = false;

    public AudioSource FireSource;

    public GameObject BoomPrefab;
    public float Boom_Timer = 0f;
    public float Boom_Cool_Time = 5f;

    private void Start()
    {
        Timer = 0f; // ó�� ������ ���� Ÿ�̸� 0��
        AutoMode = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("�ڵ� ���� ���");
            AutoMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("���� ���� ���");
            AutoMode = false;
        }

        Boom_Timer -= Time.deltaTime;
        
        if (Boom_Timer <= 0 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Boom");
            // �� Ÿ�̸� �ð��� �ٽ� ��Ÿ������
            Boom_Timer = Boom_Cool_Time;

            // �� �������� ������ �����Ѵ�.
            GameObject boom = Instantiate(BoomPrefab);
            boom.transform.position = new Vector2 (0, 1.6f);         
        }
              
        // Ÿ�̸� ���
        Timer -= Time.deltaTime;

        // 1. Ÿ�̸Ӱ� 0���� ���� ���¿��� �߻� ��ư�� ������
        bool ready = AutoMode || Input.GetKeyDown(KeyCode.Space);
        if (Timer <= 0 && ready)
        {
            FireSource.Play();
              // Ÿ�̸� �ʱ�ȭ
              Timer = Cool_Time;

              // ��ǥ : �ѱ� ���� ��ŭ �Ѿ��� �����, ���� �Ѿ��� ��ġ�� �� �ѱ��� ��ġ�� �ٲ۴�.
              for (int i = 0; i < Muzzles.Length; i++)
              {
                    // 2. ���������κ��� �Ѿ��� �����.
                    GameObject bullet = Instantiate(BulletPrefab);
                    GameObject subBullet = Instantiate(SubBulletPrefab);

                    // 3. ���� �Ѿ��� ��ġ�� �ѱ��� ��ġ�� �ٲ۴�.
                    bullet.transform.position = Muzzles[i].transform.position;
                    subBullet.transform.position = SubMuzzles[i].transform.position;
              }
        } 
    }
}
