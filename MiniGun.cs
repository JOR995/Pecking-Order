using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGun : Weapons
{
    protected override void Start()
    {
        weaponCollider = transform.GetChild(1).GetComponent<Collider>();
        firePoint = transform.GetChild(0);
        bulletParent = GameObject.Find("BulletParent").transform;

        weaponType = WeaponType.MiniGun;
        fireRate = 0.05f;
        damage = 3f;
        projectileSpeed = 30f;
        projectileSize = 0.08f;
        ammoCount = 100;
        bulletNum = 1;

        base.Start();
    }
}
