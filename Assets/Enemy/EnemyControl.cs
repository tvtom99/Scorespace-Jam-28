using System.Collections;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private GameObject[] enemyList;

    [SerializeField]
    int initialEnemies = 1, maxEnemies = 4;

    [SerializeField]
    Transform playerPos;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    Player player;

    [SerializeField]
    Highscore highscore;

    [SerializeField]
    GameObject[] spawnPoints;

    float timerTimerInit = 5f, timerTime = 5f;

    public bool gameOver = false;

    private void Start()
    {
        StartGame();

    }

    public void StartGame()
    {
        /*Debug.Log("Start of enemy control!");

        enemyList = new GameObject[1];

        enemyList[0] = Instantiate(enemyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        enemyList[0].AddComponent<NewEnemy>();
        //NewEnemy ne = enemyList[0].GetComponent<NewEnemy>();
        //ne.SetPlayer(player);
        enemyList[0].GetComponent<NewEnemy>().SetPlayer(player);
        enemyList[0].GetComponent<NewEnemy>().SetBulletPrefab(bulletPrefab);
        Debug.Log("Enemy should be instantiated");*/
        timerTime = timerTimerInit;    //what are these variables
        gameOver = false;
        enemyList = new GameObject[maxEnemies];
        SpawnEnemies();
        StartCoroutine(SpawnTimer());
    }

    public void GameOver()
    {
        gameOver = true;
        ClearEnemies();
    }

    void ClearEnemies()
    { 
        for(int i = 0; i < enemyList.Length; i++)
        {
            Debug.Log("enemy to be destroyed: " + enemyList[i]);
            Destroy(enemyList[i]);
        }
    }

    void SpawnEnemies()
    {
        int find = 0;
        while(find < maxEnemies && enemyList[find] != null)
        {
            find++;
        }   //this finds the first entry in the array that is empty



        if (find < maxEnemies)
        {
            //Vector2 randomRange = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            int spawnPoint = (int)Random.Range(0f, 2f);
            Vector2 spawnPosition = spawnPoints[spawnPoint].transform.localPosition;

            enemyList[find] = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemyList[find].GetComponent<NewEnemy>().SetPlayer(player);
            enemyList[find].GetComponent<NewEnemy>().SetHighscoreControl(highscore);
        }
    }

    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(timerTime);
        if (!gameOver)
        {
            SpawnEnemies();
            timerTime *= 0.95f;
            StartCoroutine(SpawnTimer());
        }

        //this will continue forever
    }
}