using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapons
{
    protected override void Start()
    {
        weaponCollider = transform.GetChild(1).GetComponent<Collider>();
        firePoint = transform.GetChild(0);
        bulletParent = GameObject.Find("GrenadeParent").transform;

        weaponType = WeaponType.GrenadeLauncher;
        fireRate = 1.1f;
        damage = 80f;
        projectileSpeed = 40f;
        projectileSize = 0.1f;
        ammoCount = 6;
        bulletNum = 1;

        base.Start();
    }
}
