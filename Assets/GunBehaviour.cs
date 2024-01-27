using System.Collections;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    
    public interface IShoot
    {
        public void Shoot(Vector2 firePos, Vector2 mousePos, GameObject bulletPrefab, Quaternion rotation, MonoBehaviour b);
    }

    public static IShoot GetBehaviour(int gun)
    {
        IShoot gunType;

        switch (gun)
        {
            case 0: //Pistol
                gunType = new Pistol();
                break;
            case 1: //Shotgun
                gunType = new Shotgun();
                break;
            case 2: //Machine Gun
                gunType = new MachineGun();
                break;
            case 3: //Sniper Rifle
                gunType = new Pistol();//CHANGE
                break;
            default:    //default to pistol
                gunType = new Pistol();
                break;
        }
        return gunType;
    }


    public struct Pistol : IShoot
    {

        public void Shoot(Vector2 firePos, Vector2 mousePos, GameObject bulletPrefab, Quaternion rotation, MonoBehaviour b)
        {
            float bulletForce = 20f;

            GameObject bullet = Instantiate(bulletPrefab, firePos, rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(mousePos * bulletForce, ForceMode2D.Impulse);
        }
    }

    public struct Shotgun : IShoot
    {
        public void Shoot(Vector2 firePos, Vector2 mousePos, GameObject bulletPrefab, Quaternion rotation, MonoBehaviour b)
        {
            float bulletForce = 20f;    //Speed of bullet
            float bulletSpread = 0.1f;  //How close together bullets are when fired
            int bulletAmount = 5;   //How many bullets a shotgun fires

            for (int i = 0; i < bulletAmount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePos, rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                Vector2 bulletPos;  //Bullet pos will create a random variation on the mousePos within a spread, so the bullets come out more like a shotgun
                bulletPos.x = Random.Range(mousePos.x - bulletSpread, mousePos.x + bulletSpread);
                bulletPos.y = Random.Range(mousePos.y - bulletSpread, mousePos.y + bulletSpread);
                bulletRb.AddForce(bulletPos * bulletForce, ForceMode2D.Impulse);
            }
        }
    }

    public struct MachineGun : IShoot
    {
        public void Shoot(Vector2 firePos, Vector2 mousePos, GameObject bulletPrefab, Quaternion rotation, MonoBehaviour b)
        {
            float bulletForce = 20f;

            IEnumerator co = MultiShot(firePos, mousePos, bulletPrefab, rotation, bulletForce, b);
            b.StartCoroutine(co);
        }

        /*void MultiShot(Vector2 firePos, Vector2 mousePos, GameObject bulletPrefab, Quaternion rotation, float bulletForce)
        {
            float bulletForce = 15f;
          

            GameObject bullet = Instantiate(bulletPrefab, firePos, rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(mousePos * bulletForce, ForceMode2D.Impulse);
        }*/

        private IEnumerator MultiShot(Vector2 firePos, Vector2 mousePos, GameObject bulletPrefab, Quaternion rotation, float bulletForce, MonoBehaviour b)
        {
            int clipSize = 10;
            float waitTime = 0.01f;

            for(int i = 0; i < clipSize; i++)
            {
                firePos = (Vector2)b.GetComponent<Transform>().position + mousePos; //Update the position to fire from

                GameObject bullet = Instantiate(bulletPrefab, firePos, rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.AddForce(mousePos * bulletForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}