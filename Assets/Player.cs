using System.Collections;
using System.Collections.Generic;
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
            Shoot();
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

    void Shoot()
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
        //Quaternion arrowRotatation = Quaternion.Euler(new Vector3(angleFromFirePoint, 0, 0));

        //Add the final bullet position to the fireFrom vector
        fireFrom += mousePos;

        GameObject bullet = Instantiate(bulletPrefab, fireFrom, Quaternion.identity);
        Transform bulletTran = bullet.GetComponent<Transform>();
        bulletTran.Rotate(angleFromFirePoint, 0f, 0f);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(mousePos * bulletForce, ForceMode2D.Impulse);
    }
}
