using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Min(0.01f)]
    float timeToPop = 3f;   //Time until bullets fizzle out

    [SerializeField, Range(1f,2f)]
    float slowDown = 1.02f;

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

        timeToPop--;
    }

    void OnCollisionEnter2D()
    {
        //Destroy(gameObject);      find better way for this cause atm the bullets are destorying themselves lol
    }

    IEnumerator PopBullet(float timeToPop)
    {
        yield return new WaitForSeconds(timeToPop);
        Destroy(gameObject);
    }
}