using System.Collections;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform bulletPosition = null;
    [SerializeField]
    private GameObject bulletPreFab = null;
    private GameManager gameManager = null;
    
    private Rigidbody2D rigid;
    private Animator ani;

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private Vector2 targetPosition = Vector2.zero;

    private bool isAttack = false;
    private bool isWalking = false;

    [SerializeField]
    private float attackDelay = 0.5f;

    private bool isDead;
    private bool isDamaged;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
       if (Input.GetMouseButton(0))
       {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.x = Mathf.Clamp(targetPosition.x, gameManager.MinPosition.x, gameManager.MaxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, gameManager.MinPosition.y, gameManager.MaxPosition.y);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);

       }
        if (isAttack == false) 
        StartCoroutine(Attack());
    }


    private IEnumerator Attack()
    {
        while(true)
        {
            isAttack = true;
            isWalking = true;
            ani.Play("Attack_Animation");
            PoolFire();
            yield return new WaitForSeconds(0.3f);
            ani.Play("Stop_Animation");
            isWalking = false;
            yield return new WaitForSeconds(attackDelay);
            isAttack = false;
        }
    }
    private void PoolFire()
    {
        GameObject Bullet = null;

        if(gameManager.poolManager2.transform.childCount > 0)
        {
            Bullet = gameManager.poolManager2.transform.GetChild(0).gameObject;
            Bullet.transform.position = bulletPosition.position;
            Bullet.transform.SetParent(null);
            Bullet.SetActive(true);
        }
        else
        {
            Bullet = Instantiate(bulletPreFab, bulletPosition);
        }
        if (Bullet != null)
        {
            Bullet.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDamaged) return;
        if (isDead) return;
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        gameManager.Dead();
        isDamaged = true;
        ani.Play("Player_Hurt");
        isWalking = true;
        yield return new WaitForSeconds(1f);
        isDamaged = false;
        isWalking = false;
    }

    private void PlayerAnimation(float x, float y)
    {
        if (isWalking == false)
        {
            if (x > 0)
            {
                ani.Play("Player_right");
            }
            else if (x < 0)
            {
                ani.Play("Player_left");
            }
            else if (y > 0)
            {
                ani.Play("Player_up");
            }
            else if (y < 0)
            {
                ani.Play("Player_down");
            }
            else
            {
                ani.Play("Stop_Animation");
            }
        }
    }
}
