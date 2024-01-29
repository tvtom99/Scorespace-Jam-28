using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float moveSpeed = 1.5f;

    [SerializeField]
    float bulletDistFromEnemy = 3f;

    [SerializeField]
    int maxHP = 3;

    [SerializeField]
    GameObject pistolAmmo, shotgunAmmo, machinegunAmmo, sniperAmmo;

    int HP;

    bool canShoot = true;
    bool onScreen = false;

    int gunInt;



    void Start()
    {
        HP = maxHP;

        //Get the rigidbody
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Get a random gun
        gunInt = (int)Random.Range(0, 4);
        //int gunInt = 0;
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
        Vector2 relativeCameraPosition = player.GetComponent<Player>().camera.WorldToViewportPoint(gameObject.transform.position);

        onScreen = (relativeCameraPosition.x > 0 && relativeCameraPosition.y > 0 && relativeCameraPosition.x < 1 && relativeCameraPosition.y < 1);

        float distance = Vector3.Distance(player.GetPosition(), transform.position);

        if (onScreen && (distance <= 10f))  //oooo nice another hardcoded value that should really be variable and a serialized field
        {
            if (canShoot)
            {
                canShoot = false;
                StartCoroutine(PrepareShot());
            }
            //else
            //{
                //StartCoroutine(ShootTimer());
            //}

            rb.velocity *= 0.0001f * Time.deltaTime;
        }
        else
        {
            MoveTowardPlayer();
        }
    }

    void Shoot()
    {
        Vector2 firePos = gameObject.transform.position;

        Vector2 playerPos = player.transform.position;  //effectively mousePos from the og script

        playerPos -= firePos;

        playerPos = playerPos.normalized * bulletDistFromEnemy;

        firePos += playerPos;

        StartCoroutine(TakeShot(firePos, playerPos, bulletPrefab, Quaternion.identity, this));
    }

    void MoveTowardPlayer()
    {
        Vector3 playerPos = player.transform.position;

        Vector2 direction = playerPos - transform.position;
        direction = direction.normalized * moveSpeed;

        rb.AddForce(direction);
        Mathf.Clamp(rb.velocity.x, 0f, moveSpeed);
        Mathf.Clamp(rb.velocity.y, 0f, moveSpeed);
    }

    IEnumerator TakeShot(Vector2 firePos, Vector2 playerPos, GameObject bulletPrefab, Quaternion q, MonoBehaviour m)
    {
        yield return new WaitForSeconds(0.2f);  //Add delay so that the enemies aren't using aimbot
        gun.Shoot(firePos, playerPos, bulletPrefab, Quaternion.identity, this, null, enemyBulletPower);   //I should probably have the enemies shoot diff bullets hey
        yield return new WaitForSeconds(1f);
    }

    IEnumerator PrepareShot()
    {
        yield return new WaitForSeconds(1f);
        if (onScreen)   //Check if still on screen
        {
            Shoot();
        }
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }

    IEnumerator ShootTimer()
    {
        //yield return new WaitForSeconds(1f);
        if (onScreen)
        {
            yield return new WaitForSeconds(1f);
            canShoot = true;
        }
    }

    public void SetPlayer(Player p)
    {
        this.player = p;
    }

    public void SetBulletPrefab(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;
    }

    public void TakeDamage()
    {
        //Maybe flash white when hit? or black lol thats fine
        StartCoroutine(DamageFlash());
        HP--;
        
        if(HP <= 0)
        {
            //Add point to leaderboard'
            //TODO

            GameObject ammoPack;

            switch (gunInt)
            {
                case 0:
                    ammoPack = Instantiate(pistolAmmo, transform.position, Quaternion.identity);
                    break;
                case 1:
                    ammoPack = Instantiate(shotgunAmmo, transform.position, Quaternion.identity);
                    break;
                case 2:
                    ammoPack = Instantiate(machinegunAmmo, transform.position, Quaternion.identity);
                break;
                case 3:
                    ammoPack = Instantiate(sniperAmmo, transform.position, Quaternion.identity);
                break;
                default:
                    ammoPack = Instantiate(pistolAmmo, transform.position, Quaternion.identity);
                    break;
            }

            Destroy(gameObject);
        }
    }

    IEnumerator DamageFlash()
    {
        int flashes = 4;
        Color ogColour = gameObject.GetComponent<SpriteRenderer>().color;

        for (int i = 0; i < flashes; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().color = ogColour;
        }
    }
}
