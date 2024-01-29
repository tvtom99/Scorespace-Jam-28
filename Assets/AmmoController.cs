using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [SerializeField]
    Transform camTransform;

    [SerializeField]
    Vector3 offset = new Vector3(-13, 7, 1000);

    [SerializeField]
    GameObject
        pistolAmmo,
        shotgunAmmo,
        machinegunAmmo,
        sniperAmmo;

    [SerializeField]
    Vector3[] ammoPositions;

    [SerializeField]
    Vector3[] ammoScales;

    [SerializeField]
    Vector3 ammoAnimStartPos = new Vector3(3, 0, -4);

    GameObject[] ammo = new GameObject[4];

    private void Start()
    {
        //Get the position of the camera and move to correct relative position
        //If camera position is pos, inventory should be at pos.x - 13 and pos.y + 7

        StickToHudPosition();

        ammo[0] = Instantiate(pistolAmmo);
        ammo[0].transform.parent = gameObject.transform;
        ammo[0].transform.localPosition = ammoAnimStartPos;
        ammo[0].transform.localScale = Vector3.zero;
        ammo[0].GetComponent<AmmoType>().SetMovementGoal(ammoPositions[0], ammoScales[0]);
        //ammo[0].transform.localPosition = ammoPositions[0];
        //ammo[0].transform.localScale = ammoScales[0];
        //ammo[0].GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
        //ammo[0].GetComponent<Transform>().position = ammoPositions[0];
    }

    private void Update()
    {
        StickToHudPosition();
    }

    void AddAmmo(int ammo)
    {
        GameObject ammoType = GetAmmoType(ammo);
        ammoType = Instantiate(ammoType);


    }

    GameObject GetAmmoType(int ammo)
    {
        GameObject ammoType;
        switch (ammo)
        {
            case 0:
                ammoType = pistolAmmo;
                break;
            case 1:
                ammoType = shotgunAmmo;
                break;
            case 2:
                ammoType = machinegunAmmo;
                break;
            case 3:
                ammoType = sniperAmmo;
                break;
            default:
                ammoType = pistolAmmo;
                break;
        }

        return ammoType;
    }

    void StickToHudPosition()
    {
        Transform t = gameObject.GetComponent<Transform>();
        t.position = camTransform.position + offset;
    }
}