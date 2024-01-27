
using UnityEngine;

using static Unity.Mathematics.math;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Player player;

    Transform position;

    float targetX = 0f, targetY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos, targetPos, myPos = gameObject.transform.position;
        playerPos = player.transform.position;
        
        

        if(targetX != gameObject.transform.position.x)
        {
            targetPos = Vector2.Lerp(playerPos, myPos, Time.deltaTime);

            position.position = targetPos;

            if(abs(playerPos.x - myPos.x) < 0.1f && abs(playerPos.y - myPos.y) < 0.1f)
            {
                myPos = playerPos;
                position.position = myPos;
            }

            position.position = playerPos;
        }
    }
}
