using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoType : MonoBehaviour
{
    [SerializeField]
    int ammoAmount; //Amount of shots available

    [SerializeField]
    int ammoType;   //0 - pistol, 1 - shotgun, 3 - machine gun, 4 - sniper

    [SerializeField, Min(0.01f)]
    float animationSpeed = 0.5f;

    [SerializeField]
    GameObject ammoVisualPrefab;

    [SerializeField]
    float ammoVisualOffset = 0.05f, ammoDistBetween = 0.1f;

    [SerializeField]
    Vector3 ammoVisualScale = new Vector3(1f, 1f, 1f);

    Vector3 endPos;

    Vector3 endScale;

    GameObject[] ammoVisual;

    // Start is called before the first frame update
    void Start()
    {
        ammoVisual = new GameObject[ammoAmount];

        for(int i = 0; i < ammoAmount; i++)
        {
            ammoVisual[i] = Instantiate(ammoVisualPrefab);
            ammoVisual[i].transform.parent = gameObject.transform;
        }

        KeepAmmoVisualInPosition();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateIn();
        KeepAmmoVisualInPosition();
    }

    void AnimateIn()
    {
        //While not at ideal position, move towards it
        Transform t = gameObject.GetComponent<Transform>();

        if (t.localPosition != endPos)
        {
            Vector3 moveAmount = Vector3.zero;
            moveAmount.x = Mathf.Lerp(t.localPosition.x, endPos.x, animationSpeed * Time.deltaTime);
            moveAmount.y = Mathf.Lerp(t.localPosition.y, endPos.y, animationSpeed * Time.deltaTime);
            moveAmount.z = Mathf.Lerp(t.localPosition.z, endPos.z, animationSpeed * Time.deltaTime);

            t.localPosition = moveAmount;

            //Do the same for scale
            Vector3 scaleAmount = Vector3.zero;
            scaleAmount.x = Mathf.Lerp(t.localScale.x, endScale.x, animationSpeed * Time.deltaTime);
            scaleAmount.y = Mathf.Lerp(t.localScale.y, endScale.y, animationSpeed * Time.deltaTime);
            scaleAmount.z = Mathf.Lerp(t.localScale.z, endScale.z, animationSpeed * Time.deltaTime);

            t.localScale = scaleAmount;
        }
    }

    void KeepAmmoVisualInPosition()
    {
        for (int i = 0; i < ammoVisual.Length; i++)
        {
            ammoVisual[i].transform.localPosition = new Vector3(0, -ammoVisualOffset - ((i * ammoDistBetween) + 0.2f), 0);
            ammoVisual[i].transform.localScale = ammoVisualScale;
        }
    }

    public void SetMovementGoal(Vector3 endPos, Vector3 endScale)
    {
        this.endPos = endPos;
        this.endScale = endScale;
    }
}
