using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    /*
     * NewEnemy
     * --------
     * 
     * Second simplified approach to creating enemy.
     * Will have a gun chosen at random.
     * 
     */

    [SerializeField]
    GunBehaviour.IShoot gun;

    [SerializeField]
    Player player;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    float enemyBulletPower = 10f;

    bool canShoot = true;



    void Start()
    {
        //Get a random gun
        //int gunInt = (int)Random.Range(0, 4);
        int gunInt = 0;
        gun = GunBehaviour.GetBehaviour(gunInt);

        switch(gunInt)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().color = Color.white; 
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red; 
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue; 
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().color = Color.cyan; 
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            canShoot = false;
            Shoot();
        }
    }

    void Shoot()
    {
        float bulletDistFromEnemy = 3f;

        Vector2 firePos = gameObject.transform.position;

        Vector2 playerPos = player.transform.position;  //effectively mousePos from the og script

        playerPos -= firePos;

        playerPos = playerPos.normalized * bulletDistFromEnemy;

        firePos += playerPos;

        StartCoroutine(TakeShot(firePos, playerPos, bulletPrefab, Quaternion.identity, this));

        //gun.Shoot(firePos, )
    }

    IEnumerator TakeShot(Vector2 firePos, Vector2 playerPos, GameObject bulletPrefab, Quaternion q, MonoBehaviour m)
    {
        yield return new WaitForSeconds(0.2f);  //Add delay so that the enemies aren't using aimbot
        gun.Shoot(firePos, playerPos, bulletPrefab, Quaternion.identity, this, null, enemyBulletPower);   //I should probably have the enemies shoot diff bullets hey
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }

    public void SetPlayer(Player p)
    {
        this.player = p;
    }

    public void SetBulletPrefab(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;
    }
}
