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

    bool canShoot = true;



    void Start()
    {
        //Get a random gun
        int gunInt = (int)Random.Range(0, 4);
        gun = GunBehaviour.GetBehaviour(gunInt);
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
        gun.Shoot(firePos, playerPos, bulletPrefab, Quaternion.identity, this);
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
