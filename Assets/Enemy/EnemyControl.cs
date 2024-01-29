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

    void Start()
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
        SpawnEnemies();
    }


    void SpawnEnemies()
    {
        enemyList = new GameObject[initialEnemies];

        for(int i = 0; i < enemyList.Length; i++)
        {
            Vector2 randomRange = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

            enemyList[i] = Instantiate(enemyPrefab, Vector2.zero + randomRange, Quaternion.identity);
            enemyList[i].GetComponent<NewEnemy>().SetPlayer(player);
        }
    }
}