using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon")]
public class Weapon : ScriptableObject {

    //Editor values
    [SerializeField] private int ClipAmmo;
    [SerializeField] private int MaxAmmo;
    [SerializeField] private float FireRate;

    [SerializeField] private int damage;
    [SerializeField] private bool IsMelee;
    [SerializeField] private Sprite weaponSprite;

    //Private values
    private int currClipAmmo;
    private int currTotalAmmo;

    private void OnEnable()
    {
        currClipAmmo = ClipAmmo;
        currTotalAmmo = MaxAmmo;
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Shoots the weapon - if it's not a melee
    void Shoot()
    {
        //If there is a bullet to shoot
        if (currClipAmmo > 0)
        {
            //TODO: Shoot the weapon

            //Remove one from the clip ammo
            ClipAmmo--;
        }
        else
        {
            Reload();
        }

    }

    //Reloads the weapon - if it's not a melee
    void Reload()
    {
        //If the current clip ammo is zero
        if (currClipAmmo <= 0)
        {

            //Check if there is enough ammo to reload in the weapon
            if (currTotalAmmo >= ClipAmmo)
            {
                //Remove the ammo from the total ammo and set the clip ammo
                currTotalAmmo -= ClipAmmo;
                currClipAmmo = ClipAmmo;
            }
            else
            {
                //TODO: Play click sound
            }
        }
        else if (currClipAmmo > 0)
        {
            //Get the difference
            int difference = ClipAmmo - currClipAmmo;

            //Check that the weapon has enough ammo left
            if (currTotalAmmo >= difference)
            {
                //Remove the ammo from the total and set the clip ammo
                currTotalAmmo -= difference;
                currClipAmmo = ClipAmmo;
            }
        }
        //If the current total weapon ammo is less than clip ammo
        else if (currTotalAmmo < ClipAmmo)
        {
            currClipAmmo = currTotalAmmo;
            currTotalAmmo = 0;
        }
    }
}
