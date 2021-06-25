using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PoolManager poolManager = null;
    public PoolManager2 poolManager2 = null;
    public PoolManager3 poolManager3 = null;
    
    [SerializeField]
    private Transform enemyPosition;

    private int life = 3;
    private int score = 0;
    private int highscore = 500;
    [SerializeField]
    private Text textHighScore = null;
    [SerializeField]
    private Text textScore = null;
    [SerializeField]
    private Text textLife = null;

    [SerializeField]
    private GameObject EnemyPrefab = null;

    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPosition { get; private set; }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("HIGHSCORE");
        poolManager = FindObjectOfType<PoolManager>();
        poolManager2 = FindObjectOfType<PoolManager2>();
        poolManager3 = FindObjectOfType<PoolManager3>();
        MinPosition = new Vector2(-4.5f, -1.9f);
        MaxPosition = new Vector2(4f, 1.9f);
        StartCoroutine(SpawnEnemy());
    }


    private IEnumerator SpawnEnemy()
    {
        float randomY;
        float randomDelay;
        while (true)
        {
            randomY = Random.Range(-1f, 1f);
            randomDelay = Random.Range(1, 3);
            PoolEnemy(randomY);
            yield return new WaitForSeconds(randomDelay);
        }

    }

    private void PoolEnemy(float y)
    {
        GameObject Enemy = null;
        Vector2 enemyVector = new Vector2(5f, y);
        enemyPosition.position = enemyVector;
        if (poolManager.transform.childCount > 0)
        {
            Enemy = poolManager.transform.GetChild(0).gameObject;
            Enemy.transform.position = enemyPosition.position;
            Enemy.transform.SetParent(null);
            Enemy.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            
            Enemy.GetComponent<Collider2D>().enabled = true;
            Enemy.GetComponent<EnemyMove>().isDamaged = false;
            Enemy.GetComponent<EnemyMove>().isDead = false;
            Enemy.GetComponent<EnemyMove>().hp = 3;
            Enemy.SetActive(true);
        }
        else
        {
            Enemy = Instantiate(EnemyPrefab, enemyPosition);
        }
        if (Enemy != null)
        {
            Enemy.transform.SetParent(null);
        }
    }

    private void UpdateUI()
    {
        textScore.text = string.Format("SCORE {0}", score);
        textHighScore.text = string.Format("HIGHSCORE {0}", highscore);
    }

    public void Dead()
    {
        life--;
        if (life <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        UpdateUI();
        textLife.text = string.Format("LIFE {0}", life);
    }
    public void Addscore(int addscore)
    {
        score += addscore;
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("HIGHSCORE", highscore);
        }
        UpdateUI();
    }
}
