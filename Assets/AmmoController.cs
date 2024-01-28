using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [SerializeField]
    Transform camTransform;

    [SerializeField]
    Vector3 offset = new Vector3(-13, 7, 1000);

    private void Start()
    {
        //Get the position of the camera and move to correct relative position
        //If camera position is pos, inventory should be at pos.x - 13 and pos.y + 7

        Transform t = gameObject.GetComponent<Transform>();
        t.position = camTransform.position + offset;
        


    }

    private void Update()
    {
        Transform t = gameObject.GetComponent<Transform>();
        t.position = camTransform.position + offset;
    }
}