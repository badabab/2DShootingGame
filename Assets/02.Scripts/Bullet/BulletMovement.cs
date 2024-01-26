using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed = 1f;
    private float t = 0;
    private Vector3 targetPos;
    private Vector3 controlPoint;
    private Vector3 startPos;

    void Start()
    {
        // 모든 Enemy Layer의 오브젝트를 찾습니다.
        var enemies = FindObjectsOfType<GameObject>();
        GameObject enemy = null;

        foreach (var e in enemies)
        {
            if (e.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemy = e;
                break;
            }
        }

        // Enemy 오브젝트가 없을 경우
        if (enemy == null)
        {
            Debug.LogError("Enemy object not found");
            return;
        }

        startPos = transform.position;
        targetPos = enemy.transform.position;

        // 중간 컨트롤 포인트를 설정합니다. 이 예에서는 시작점과 목표점의 중간을 사용합니다.
        controlPoint = (startPos + targetPos) / 2;
    }

    void Update()
    {
        t += Time.deltaTime * speed;

        if (t > 1)
        {
            t = 1;
        }

        transform.position = CalculateBezierPoint(t, startPos, controlPoint, targetPos);
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 B = uu * p0 + 2 * u * t * p1 + tt * p2;
        return B;
    }
}
