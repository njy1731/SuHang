using UnityEngine;

public class EnemyBulletMove : BulletMove
{

    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        CheckLimit();
    }

    private void CheckLimit()
    {

        if (transform.position.y > gameManager.MaxPosition.y + 2f || transform.position.y < gameManager.MinPosition.y - 2f ||
            transform.position.x > gameManager.MaxPosition.x + 2f || transform.position.x < gameManager.MinPosition.x - 2f)
        {
            Despawn();
        }
    }
    new void Despawn()
    {
        gameObject.SetActive(false);
        transform.SetParent(gameManager.poolManager3.transform, false);
    }
}
