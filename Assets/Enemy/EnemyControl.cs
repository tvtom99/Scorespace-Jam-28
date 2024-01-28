using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private GameObject[] enemyList;

    [SerializeField]
    Transform playerPos;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    Player player;

    [SerializeField]
    GameObject bulletPrefab;

    void Start()
    {
        Debug.Log("Start of enemy control!");

        enemyList = new GameObject[1];

        enemyList[0] = Instantiate(enemyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        enemyList[0].AddComponent<NewEnemy>();
        //NewEnemy ne = enemyList[0].GetComponent<NewEnemy>();
        //ne.SetPlayer(player);
        enemyList[0].GetComponent<NewEnemy>().SetPlayer(player);
        enemyList[0].GetComponent<NewEnemy>().SetBulletPrefab(bulletPrefab);
        Debug.Log("Enemy should be instantiated");
    }
}