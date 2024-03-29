using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Camera camera;

    float extents = 0.5f;
    
    Rigidbody2D body;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;

    [SerializeField]
    Vector2 firePointOffset;

    [SerializeField]
    float bulletDistFromPlayer = 3f;

    GunBehaviour.IShoot gun = GunBehaviour.GetBehaviour(0);

    [SerializeField]
    float[] gunDelay = {0.5f, 1.5f, 1f, 1.5f };   //Pistol, Shotgun, Machine Gun, Sniper

    [SerializeField]
    AmmoController ammoController;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Highscore highscore;

    [SerializeField]
    EnemyControl enemyControl;

    [SerializeField]
    int maxHP = 5;

    [SerializeField]
    TextMeshPro gameOver;

    int hp;

    bool gameEnd = false;

    bool canShoot = true;

    bool canTakeDamage = true;

    bool direction = true; //true = left, false = right

    delegate void Shoot();

    public float GetExtents() => extents;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        hp = maxHP;
        gameOver.gameObject.SetActive(false);
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down



        //Shooting mechanic
        if (Input.GetButtonDown("Fire1"))
        {
            ShootGun();
        }

        //debug gun changing ability
        if(Input.GetKeyDown("1"))
        {
            gun = GunBehaviour.GetBehaviour(0);
        }
        if (Input.GetKeyDown("2"))
        {
            gun = GunBehaviour.GetBehaviour(1);
        }
        if (Input.GetKeyDown("3"))
        {
            gun = GunBehaviour.GetBehaviour(2);
        }
        if (Input.GetKeyDown("4"))
        {
            gun = GunBehaviour.GetBehaviour(3);
        }

        if(hp <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        //reset the player position, dont let them move, ask for press spacebar to restart
        gameOver.gameObject.SetActive(true);
        enemyControl.GameOver();
        highscore.GameOver();

        gameEnd = true;
        canShoot = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameOver.gameObject.SetActive(false);
            hp = maxHP;
            gameEnd = false;
            canShoot = true;
            ammoController.AddAmmo((int)Random.Range(0f, 2f));
            enemyControl.StartGame();
        }
        
    }

    public void TakeDamage()
    {
        if(canTakeDamage)
        {
            canTakeDamage = false;
            StartCoroutine(DamageFlash());
            hp--;
            Debug.Log("hp is now: " + hp);
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

        Debug.Log("can now take damage");
        canTakeDamage = true;
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        if (!gameEnd)
        {
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }
        animator.SetFloat("moveSpeed", Mathf.Abs(((body.velocity.x + body.velocity.y) / 2f)));

        UpdateBodyDirection();
    }

    void UpdateBodyDirection()
    {
        if(body.velocity.x > 0.1)
        {
            direction = true;
        }
        else if(body.velocity.x < -0.1)
        {
            direction = false;
        }

        gameObject.GetComponent<SpriteRenderer>().flipX = direction;
    }

    void ShootGun()
    {
        //A lot is happening here that could probably be moved into it's own function, but this is a game jam and it works so it stays

        //Get the screen position of the mouse
        Vector2 mousePos = Input.mousePosition;
        //Uncomment for debug; Debug.Log("Screen mouse pos: " +  mousePos);

        //Convert the mouse position from screen space to world space
        mousePos = camera.ScreenToWorldPoint(mousePos);
        //Uncomment for debug; Debug.Log("World mouse pos: " + mousePos);

        //Find out if the direction is left or right and thus if the offset should be inversed
        if(direction)
        {
            firePointOffset.x = -firePointOffset.x;
        }

        //Create new Vector2 for where the bullet will fire from
        Vector2 fireFrom = new Vector2(firePoint.position.x + firePointOffset.x, firePoint.position.y + firePointOffset.y);

        //Put it back to regular so it doesn't fk with anything next time we inverse
        firePointOffset.x = -firePointOffset.x;

        //Get a vector 'pointing' from fire position to mouse
        mousePos -= fireFrom;
        //Uncomment for debug; Debug.Log("Not-normalized mouse pos: " + mousePos);

        //Set the distance from the fire position to be equal to variable 'bulletDistFromPlayer'
        mousePos = mousePos.normalized * bulletDistFromPlayer;
        //Uncomment for debug; Debug.Log("Normalized mouse pos: " + mousePos);

        //Calculate the angle that the arrow should point
        float angleFromFirePoint = Mathf.Rad2Deg * Mathf.Asin(mousePos.x);
        //Uncomment for debug; Debug.Log("arrow angle: " + angleFromFirePoint);

        //Add the final bullet position to the fireFrom vector
        fireFrom += mousePos;

        //Call the shoot method of whatever gun is currently being used

        GameObject currentAmmoType = ammoController.GetCurrentAmmo();

        if (canShoot && currentAmmoType != null && ammoController.GetCurrentAmmo().GetComponent<AmmoType>().HasAmmo())
        {
            canShoot = false;
            Debug.Log("gunBehaviour: " + ammoController.GetCurrentAmmo().GetComponent<AmmoType>().GetGunType());
            GameObject currentAmmo = ammoController.GetCurrentAmmo();

            if (currentAmmo != null)
            {
                gun = GunBehaviour.GetBehaviour(currentAmmo.GetComponent<AmmoType>().GetGunType());
                gun.Shoot(fireFrom, mousePos, bulletPrefab, Quaternion.identity, this, ammoController, 20f);
                StartCoroutine(GunWait(gunDelay[currentAmmo.GetComponent<AmmoType>().GetGunType()]));
                currentAmmo.GetComponent<AmmoType>().UseAmmo();
                //StartCoroutine(GunWait(gunDelay[gun.GetGunNumber()]));        So apparently 'gun.GetGunNumber()' is scuffed and is returning the wrong number. Instead of fixing it I'll use another function that does that same thing LOLOLLO>
                
            }
        }
        else
        {
            Debug.Log("cannot shoot!");
        }
    }

    public Vector2 GetPosition()
    {
        return gameObject.transform.position;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //in business
        if(collision.gameObject.GetComponent<PistolPickup>() != null)
        {
            //hit pistol
            if(ammoController.IsFull() != true)
            {
                ammoController.AddAmmo(0);
                Destroy(collision.gameObject);
            }
        } else if (collision.gameObject.GetComponent<ShotgunPickup>() != null)
        {
            //hit shotty
            if (ammoController.IsFull() != true)
            {
                ammoController.AddAmmo(1);
                Destroy(collision.gameObject);
            }
        } else if (collision.gameObject.GetComponent<MachinegunPickup>() != null)
        {
            //hit wooper gun
            if (ammoController.IsFull() != true)
            {
                ammoController.AddAmmo(2);
                Destroy(collision.gameObject);
            }
        } 
        //else if (collision.gameObject.GetComponent<PistolPickup>() != null)
        //{
            //hit pistol cause legit im not adding sniper
        //}
    }

    //This coroutine waits a specific delay to allow the user to shoot again
    //I know I should have setup the game to have this handled elsewhere but oops >.<
    IEnumerator GunWait(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
        Debug.Log("Can shoot again");
    }
}
