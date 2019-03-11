using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : Weapons
{
    protected override void Start()
    {
        weaponCollider = transform.GetChild(2).GetComponent<Collider>();
        firePoint = transform.GetChild(0);
        bulletParent = GameObject.Find("BulletParent").transform;

        weaponType = WeaponType.Gun;
        fireRate = 0.55f;
        damage = 20f;
        projectileSpeed = 16f;
        projectileSize = 0.1f;
        ammoCount = 20;
        bulletNum = 1;

        base.Start();
    }
}
