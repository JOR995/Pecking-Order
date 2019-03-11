using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapons
{
    protected override void Start()
    {
        weaponCollider = transform.GetChild(1).GetComponent<Collider>();
        firePoint = transform.GetChild(0);
        bulletParent = GameObject.Find("BulletParent").transform;

        weaponType = WeaponType.Gun;
        fireRate = 0.8f;
        damage = 20f;
        projectileSpeed = 20f;
        projectileSize = 0.07f;
        ammoCount = 12;
        bulletNum = 5;

        base.Start();
    }
}
