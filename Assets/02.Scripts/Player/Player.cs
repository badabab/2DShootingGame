using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health = 3;

    private void Start()
    {
        // GetComponent< ������Ʈ Ÿ�� > (); -> ���� ������Ʈ�� ������Ʈ�� �������� �޼���
        // SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // sr.color = Color.white;

        // Transform tr = GetComponent<Transform>();
        // tr.position = new Vector2(0f, -2.7f); // �ʱ� ��ġ
        transform.position = new Vector2(0f, -2.7f); // �ʱ� ��ġ

        PlayerMove playerMove = GetComponent<PlayerMove>(); // PlayerMove���� ������ �� ������ �� ����
        //Debug.Log(playerMove.Speed);
        playerMove.Speed = 5f;  // �ӵ��� �ٲ���
        //Debug.Log(playerMove.Speed);

        Debug.Log($"Player Health: {Health}");
    }
}
