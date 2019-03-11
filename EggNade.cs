using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggNade : Weapons
{
    protected override void Start()
    {
        weaponCollider = transform.GetChild(0).GetComponent<Collider>();
        firePoint = null;
        explosionEffect = transform.GetChild(1).gameObject;
        explosionEffect.SetActive(false);

        weaponType = WeaponType.Explosive;
        fireRate = 1f;
        damage = 100f;
        projectileSpeed = 10f;
        projectileSize = 1f;
        ammoCount = 1;
        bulletNum = 1;
        fuseTimer = 3f;

        base.Start();
    }
}
