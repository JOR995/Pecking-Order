using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapons
{
    protected override void Start()
    {
        weaponCollider = transform.GetChild(1).GetComponent<Collider>();
        firePoint = transform.GetChild(0);
        bulletParent = GameObject.Find("RocketParent").transform;

        weaponType = WeaponType.RocketLauncher;
        fireRate = 1.5f;
        damage = 80f;
        projectileSpeed = 40f;
        projectileSize = 0.1f;
        ammoCount = 4;
        bulletNum = 1;

        base.Start();
    }
}
