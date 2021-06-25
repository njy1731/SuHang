using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    private int score = 200;
    public int hp = 3;
    [SerializeField]
    private float speed = 3f;

    private GameManager gameManager = null;
    private Animator ani = null;
    private Collider2D col = null;
    
    [SerializeField]
    private GameObject EnemyBullet = null;
    [SerializeField]
    private float delay = 2f;

    private float delaytime = 2f;
    public bool isDamaged;
    public bool isDead;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ani = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isDead) return;
        delaytime += Time.deltaTime;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        CheckLimit();

        if(delaytime > delay)
        {
            delaytime = 0f;
            PoolFire();
        }
        
    }

    private void PoolFire()
    {
        GameObject EnemyBullet = null;

        if (gameManager.poolManager3.transform.childCount > 0)
        {
            EnemyBullet = gameManager.poolManager3.transform.GetChild(0).gameObject;
            EnemyBullet.transform.position = transform.position;
            EnemyBullet.transform.SetParent(null);
            EnemyBullet.SetActive(true);
        }
        else
        {
            EnemyBullet = Instantiate(this.EnemyBullet, transform);
        }
        if (EnemyBullet != null)
        {
            EnemyBullet.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            BulletMove bulletMove = collision.GetComponent<BulletMove>();
            bulletMove.Despawn();
            if (isDamaged) return;
            isDamaged = true;
            StartCoroutine(Damaged());

           
        }
    }

    private IEnumerator Damaged()
    {

        hp--;
        ani.Play("EnemyHurt");
        yield return new WaitForSeconds(0.1f);
        ani.Play("Enemy_Attack");
        yield return new WaitForSeconds(0.1f);
        isDamaged = false;
        if (hp <= 0)
        {
            isDead = true;
            gameManager.Addscore(score);
            StartCoroutine(Dead());
        }
    }

    private IEnumerator Dead()
    {
        col.enabled = false;
        ani.Play("POK PAL");
        yield return new WaitForSeconds(0.8f);
        Despawn();
    }

    private void CheckLimit()
    {
       
        if(transform.position.y > gameManager.MaxPosition.y + 2f || transform.position.y < gameManager.MinPosition.y - 2f ||
            transform.position.x > gameManager.MaxPosition.x + 2f || transform.position.x < gameManager.MinPosition.x - 2f)
        {
            Despawn();
        }

    }
    private void Despawn()
    {
        gameObject.SetActive(false);
        transform.SetParent(gameManager.poolManager.transform, false);
    }

}
