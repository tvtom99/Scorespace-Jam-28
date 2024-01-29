using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField]    
    float despawnTime = 10f;

    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    Vector2 targetScale = new Vector2(4f, 4f);

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        StartCoroutine(Despawn());
    }

    // Update is called once per frame
    void Update()
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;

        Vector2 newScale;
        newScale.x = Mathf.Lerp(transform.localScale.x, targetScale.x, frequency);
        newScale.y = Mathf.Lerp(transform.localScale.y, targetScale.y, frequency);

        transform.localScale = newScale;

    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        targetScale = Vector2.zero;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
