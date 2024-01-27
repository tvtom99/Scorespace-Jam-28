using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Camera camera;

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
    float bulletDistFromPlayer = 3f;

    GunBehaviour.IShoot gun = GunBehaviour.GetBehaviour(0);

    delegate void Shoot();

    public float GetExtents() => extents;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    void ShootGun()
    {
        //Calculate the angle based on where the mouse is
        //Create an arrow at a set point away from the character in that direction
        //Have the arrow go in that direction
        //Have the arrow face in that direction

        //Get the screen position of the mouse
        Vector2 mousePos = Input.mousePosition;
        Debug.Log("Screen mouse pos: " +  mousePos);

        //Convert the mouse position from screen space to world space
        mousePos = camera.ScreenToWorldPoint(mousePos);
        Debug.Log("World mouse pos: " + mousePos);

        //Create new Vector2 for where the bullet will fire from
        Vector2 fireFrom = new Vector2(firePoint.position.x, firePoint.position.y);

        //Get a vector 'pointing' from fire position to mouse
        mousePos -= fireFrom;
        Debug.Log("Not-normalized mouse pos: " + mousePos);

        //Set the distance from the fire position to be equal to variable 'bulletDistFromPlayer'
        mousePos = mousePos.normalized * bulletDistFromPlayer;
        Debug.Log("Normalized mouse pos: " + mousePos);

        //Calculate the angle that the arrow should point
        float angleFromFirePoint = Mathf.Rad2Deg * Mathf.Asin(mousePos.x);
        Debug.Log("arrow angle: " + angleFromFirePoint);

        //Add the final bullet position to the fireFrom vector
        fireFrom += mousePos;


        /*Quaternion arrowRotation = Quaternion.Euler(new Vector3(angleFromFirePoint, 0, 0));
        //Set the arrow rotation to point in direction from centre of where it was shot from to where the arrow was made
        arrowRotation = Quaternion.FromToRotation(Vector2.zero, mousePos);*/
        //^^Dont need rotation if bullets are circles hehe


        
        //GameObject bullet = Instantiate(bulletPrefab, fireFrom, Quaternion.identity);
        //Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        //bulletRb.AddForce(mousePos * bulletForce, ForceMode2D.Impulse);
        
        gun.Shoot(fireFrom, mousePos, bulletPrefab, Quaternion.identity);
    }
}
