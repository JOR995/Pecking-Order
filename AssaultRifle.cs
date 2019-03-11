using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapons
{
    protected override void Start()
    {
        weaponCollider = transform.GetChild(1).GetComponent<Collider>();
        firePoint = transform.GetChild(0);
        bulletParent = GameObject.Find("BulletParent").transform;

        weaponType = WeaponType.Gun;
        fireRate = 0.2f;
        damage = 5f;
        projectileSpeed = 25f;
        projectileSize = 0.08f;
        ammoCount = 20;
        bulletNum = 1;

        base.Start();
    }
}
