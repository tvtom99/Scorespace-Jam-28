using System.Collections;
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

    int currentClips = 0;

    int maxClips = 4;

    GameObject[] ammo = new GameObject[4];

    private void Start()
    {
        //Get the position of the camera and move to correct relative position
        //If camera position is pos, inventory should be at pos.x - 13 and pos.y + 7

        StickToHudPosition();

        AddAmmo((int)Random.Range(0f, 2f));
        //DebugLoadAllAmmo();
        //AddAmmo(0);
    }

    void DebugLoadAllAmmo()
    {
        for(int i = 0; i < ammo.Length; i++)
        {
            AddAmmo(i);
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

        //Destroy the current ammo object
        GameObject toDestroy = ammo[0];
        ammo[0] = null;
        StartCoroutine(ByeBullet(toDestroy));

        //This is called by a AmmoType object when it has run out of ammo. Ammo is only ever used from the first position, so shift all the ammo along 1 in array
        for(int i = 0; i < (currentClips - 1); i++)
        {
            Debug.Log("i: " + i + ", i + 1: " + (i + 1));

            newAmmos[i] = new GameObject();
            newAmmos[i] = ammo[i + 1];
            newAmmos[i].GetComponent<AmmoType>().SetMovementGoal(ammoPositions[i], ammoScales[i]);
        }

        ammo[currentClips - 1] = null;
        ammo = newAmmos;
        currentClips--;
    }


    private void Update()
    {
        StickToHudPosition();

        //FOR DEBUG
        for (int i = 0;i < ammo.Length;i++)
        {
            Debug.Log(i + " gunObject " + ammo[i]);
        }

        //if(Input.GetKeyDown(KeyCode.Space))
        //{
            //AddAmmo(1);
        //}
    }

    public void AddAmmo(int ammoTypeIndex)
    {
        //Find the last empty slot in the AmmoController
        int find = 0;
        while (find < 4 && ammo[find] != null)  //loop until find a null block in ammo array, or hit max ammo size of 4 (this is hardcoded, I know thats bad but pls just let it work until submission)
        {
            find++;
        }

        if (find != 4)  //If found null before reaching max
        {
            GameObject ammoType = GetAmmoType(ammoTypeIndex);
            ammoType = Instantiate(ammoType);

            ammo[find] = ammoType;
            ammo[find].transform.parent = gameObject.transform;
            ammo[find].transform.localPosition = ammoAnimStartPos;
            ammo[find].transform.localScale = Vector3.zero;
            ammo[find].GetComponent<AmmoType>().SetMovementGoal(ammoPositions[find], ammoScales[find]);
            currentClips++;
        }


        //For reference
        /*GameObject a = GetAmmoForLoad(i);
        ammo[i] = Instantiate(a);
        ammo[i].transform.parent = gameObject.transform;
        ammo[i].transform.localPosition = ammoAnimStartPos;
        ammo[i].transform.localScale = Vector3.zero;
        ammo[i].GetComponent<AmmoType>().SetMovementGoal(ammoPositions[i], ammoScales[i]);
        currentClips++;*/
    }

    public bool IsFull()
    {
        bool full = false;

        int find = 0;
        while (find < 4 && ammo[find] != null)  //loop until find a null block in ammo array, or hit max ammo size of 4 (this is hardcoded, I know thats bad but pls just let it work until submission)
        {
            find++;
        }

        if (find >= 4)  //if hit 4 then all spots in array must be full
        {
            full = true;
        }

        return full;
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

    IEnumerator ByeBullet(GameObject g)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(g);
    }
}