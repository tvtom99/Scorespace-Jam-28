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

        DebugLoadAllAmmo();

        //DEBUG just give the controller 1 pistol ammo
        /*ammo[0] = Instantiate(machinegunAmmo);
        ammo[0].transform.parent = gameObject.transform;
        ammo[0].transform.localPosition = ammoAnimStartPos;
        ammo[0].transform.localScale = Vector3.zero;
        ammo[0].GetComponent<AmmoType>().SetMovementGoal(ammoPositions[0], ammoScales[0]);*/
    }

    void DebugLoadAllAmmo()
    {
        for(int i = 0; i < ammo.Length; i++)
        {
            GameObject a = GetAmmoForLoad(i);
            ammo[i] = Instantiate(a);
            ammo[i].transform.parent = gameObject.transform;
            ammo[i].transform.localPosition = ammoAnimStartPos;
            ammo[i].transform.localScale = Vector3.zero;
            ammo[i].GetComponent<AmmoType>().SetMovementGoal(ammoPositions[i], ammoScales[i]);
        }
    }
    
    GameObject GetAmmoForLoad(int i)
    {
        //dont even comment on this, man. I know what a switch is. I just didnt use it
        if(i == 0)
        {
            return pistolAmmo;
        }
        else if (i == 1)
        {
            return shotgunAmmo;
        }
        else if (i == 2)
        {
            return machinegunAmmo;
        }
        else if (i == 3)
        {
            return sniperAmmo;
        }
        else
        {
            return pistolAmmo;
        }
    }

    public void AmmoEmpty()
    {
        GameObject[] newAmmos = new GameObject[4];

        //This is called by a AmmoType object when it has run out of ammo. Ammo is only ever used from the first position, so shift all the ammo along 1 in array
        for(int i = 0; i < newAmmos.Length - 1; i++)
        {
            Debug.Log("i: " + i + ", i + 1: " + (i + 1));

            newAmmos[i] = new GameObject();
            newAmmos[i] = ammo[i + 1];
            newAmmos[i].GetComponent<AmmoType>().SetMovementGoal(ammoPositions[i], ammoScales[i]);
            Debug.Log("end loop " + i);
        }

        ammo[ammo.Length - 1] = null;
        ammo = newAmmos;
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

    public GameObject GetCurrentAmmo()
    {
        return ammo[0];
    }

    void StickToHudPosition()
    {
        Transform t = gameObject.GetComponent<Transform>();
        t.position = camTransform.position + offset;
    }
}