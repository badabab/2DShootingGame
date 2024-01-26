using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health = 3;

    private void Start()
    {
        // GetComponent< 컴포넌트 타입 > (); -> 게임 오브젝트의 컴포넌트를 가져오는 메서드
        // SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // sr.color = Color.white;

        // Transform tr = GetComponent<Transform>();
        // tr.position = new Vector2(0f, -2.7f); // 초기 위치
        transform.position = new Vector2(0f, -2.7f); // 초기 위치

        PlayerMove playerMove = GetComponent<PlayerMove>(); // PlayerMove에서 선언한 거 가져올 수 있음
        //Debug.Log(playerMove.Speed);
        playerMove.Speed = 5f;  // 속도를 바꿔줌
        //Debug.Log(playerMove.Speed);

        Debug.Log($"Player Health: {Health}");

        // 아무노래
        // 아ㅣㅇ나ㅓㅇ리니야러ㅑ
    }
}
