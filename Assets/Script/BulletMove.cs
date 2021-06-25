using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    protected float speed = 10f;
    protected GameManager gameManager = null;

    protected void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
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
    public void Despawn()
    {
        gameObject.SetActive(false);
        transform.SetParent(gameManager.poolManager2.transform, false);
    }
}
