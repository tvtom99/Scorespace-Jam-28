using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    /*
     *  Enemy
     *  -----
     *  The enemy class can consist of 1 of 4 types of enemies;
     *      Pistol 
     *      Shotgun
     *      Machine Gun
     *      Sniper
     *      
     *  Each enemy will know what type it is, and be able to return that in a public acesible method.
     *  I will use an interface for the enemies, with structs that implement it. This way I can code generalist behaviour without caring what type of enemy there is.
     *  
     *  Enemies will use a state machine to control if they are shooting, walking or dying. Enemies will stand still before shooting, and provide the player character with ammo when they die.
     *  Coroutines will be used to handle timing of the states
     * 
     */

    /*[SerializeField]
    private static Vector2[] play = new Vector2[4];

    

    public interface IEnemy
    {
        public void Shoot();   //Handle shooting, what type of gun is used, etc
        public void Walk();
        public void Dying();
    }

    public class PistolEnemy : IEnemy  //Index is 0
    {


        public void Shoot() 
        {
            FindShootPosition(play[0]);

        }
        public void Walk() { }
        public void Dying() { }
    }*/

    [SerializeField]
    float speed = 2f;

    enum State
    {
        Walking,
        Shooting,
        Dying
    }

    private State state = State.Walking;

    public abstract void Shoot();

    public void Walk(GameObject player)
    {
        Transform transform = gameObject.GetComponent<Transform>();
        Vector2 curPos = transform.position, playerPos = player.GetComponent<Transform>().position;

        Vector2 dirToPlayer;
        dirToPlayer.x =  Mathf.Lerp(curPos.x, playerPos.x, speed);
        dirToPlayer.y = Mathf.Lerp(curPos.y, playerPos.y, speed);

        transform.position = dirToPlayer;
    }

    private void Update()
    {
        switch(state)
        {
            case State.Walking:
                Walk();
                break;
        }
    }


    //FindShootPosition will get the position of the player and add an amount of 'play'
    private static Vector2 FindShootPosition(Vector2 play)
    {
        return Vector2.zero;
    }
}