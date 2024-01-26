using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health = 0,
    Speed = 1
}

public class Item : MonoBehaviour
{
    public ItemType IType;

    public float EatTimer = 0f;
    public float EatTime = 0.5f;
    public float DropTimer = 0f;
    public float DropTime = 3f;
    public float Speed = 2f;
    private GameObject _target;


    public Animator MyAnimator;

    public GameObject HealthItemVFXPrefab;
    public GameObject SpeedItemVFXPrefab;


    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //Debug.Log("트리거 시작!");
    }
    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        //Debug.Log("트리거 중!");
        EatTimer += Time.deltaTime;
        if (EatTimer >= EatTime)
        {
            if (IType == ItemType.Health)
            {
                Player player = otherCollider.GetComponent<Player>();
                player.Health += 1;
                Debug.Log($"Player Health: {player.Health}");

                GameObject vfx = Instantiate(HealthItemVFXPrefab);
                vfx.transform.position = this.transform.position;
            }
            else if (IType == ItemType.Speed)
            {
                PlayerMove player = otherCollider.GetComponent<PlayerMove>();
                player.Speed += 1;
                Debug.Log($"Player Speed: {player.Speed}");

                GameObject vfx = Instantiate(SpeedItemVFXPrefab);
                vfx.transform.position = this.transform.position;
            }
            Destroy(this.gameObject);       
        }       
    }

    private void Awake()
    {
        MyAnimator = GetComponent<Animator>();
        MyAnimator.SetInteger("ItemType", (int)IType);
    }

    private void Start()
    {
        _target = GameObject.Find("Player");
    }
    private void Update()
    {
        DropTimer += Time.deltaTime;
        if (DropTimer >= DropTime)
        {        
            Vector2 dir = _target.transform.position - this.transform.position;
            dir.Normalize();
            transform.position += (Vector3)(dir * Speed) * Time.deltaTime;
        }
        
    }
   

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        //Debug.Log("트리거 종료!");
        EatTimer = 0f;
    }
}
