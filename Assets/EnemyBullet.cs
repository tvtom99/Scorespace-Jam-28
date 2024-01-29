using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField, Min(0.01f)]
    float timeToPop = 3f;   //Time until bullets fizzle out

    [SerializeField, Range(1f, 2f)]
    float slowDown = 1.02f;

    [SerializeField]
    AmmoController playerAmmo;

    [SerializeField, Min(0.01f)]
    float minVelocity = 3f;

    private void Awake()
    {
        StartCoroutine(PopBullet(timeToPop));
    }

    private void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb.velocity != Vector2.zero)
        {
            //Slow the object down gradually
            gameObject.GetComponent<Rigidbody2D>().velocity /= slowDown;


            if (Mathf.Abs(rb.velocity.x) < 0.1 && rb.velocity.y < 0.1)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit:" + collision.gameObject);

        if (collision.gameObject.GetComponent<Player>() != null) //player been hit
        {
            Debug.Log("hit playere");
            collision.gameObject.GetComponent<Player>().TakeDamage();
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<Bullet>() == null)    //if hit anything other than bullet
        {
            //Probs play the pop animation first, then set a coroutine timer to destroy the bullet
            Destroy(gameObject);
        }
    }

    IEnumerator PopBullet(float timeToPop)
    {
        yield return new WaitForSeconds(timeToPop);
        Destroy(gameObject);
    }
}
