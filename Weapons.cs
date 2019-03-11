using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    protected enum WeaponType { Gun, RocketLauncher, Explosive, Melee, GrenadeLauncher, MiniGun }

    public AudioClip fire, empty;

    protected WeaponType weaponType;
    protected Collider weaponCollider;
    protected float fireRate, damage, projectileSize, projectileSpeed, fuseTimer;
    protected int ammoCount, bulletNum;
    protected Transform firePoint, bulletParent;
    protected GameObject explosionEffect;

    private AudioSource audioSource;
    private PlayerControl playerController;
    private Transform handPos, bone;
    private GameObject holdingPlayer;
    private Collider holdingPlayerCollider;
    private Rigidbody rb;

    private bool canFire, pickedUp, aiHeld;
    private int playerNum;


    private void Awake()
    {
        canFire = false;
    }


    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        weaponCollider.enabled = false;

        pickedUp = false;
        rb.useGravity = false;
        aiHeld = false;
    }


    private void LateUpdate()
    {
        if (pickedUp & !aiHeld)
        {
            transform.position = handPos.position;

            if (Mathf.Abs(Input.GetAxis("Joy" + playerNum + "_RightStickHorizontal")) > 0 || Mathf.Abs(Input.GetAxis("Joy" + playerNum + "_RightStickVertical")) > 0)
            {
                float angle = Mathf.Atan2(Input.GetAxis("Joy" + playerNum + "_RightStickVertical"), Input.GetAxis("Joy" + playerNum + "_RightStickHorizontal")) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 90, angle);
            }
            else
            {
                transform.rotation = handPos.rotation;
            }
        }
    }


    public void Use()
    {
        switch (weaponType)
        {
            case WeaponType.Gun:
                ShootBullet();
                break;

            case WeaponType.MiniGun:
                ShootBullet();
                break;

            case WeaponType.RocketLauncher:
                ShootRocket();
                break;

            case WeaponType.Explosive:
                ThrowExplosive();
                break;

            case WeaponType.Melee:

                break;

            case WeaponType.GrenadeLauncher:
                ShootGrenade();
                break;
        }
    }


    private void ShootBullet()
    {
        if (canFire && ammoCount > 0 & pickedUp)
        {
            for (int i = 0; i < bulletNum; i++)
            {
                foreach (Transform bullet in bulletParent)
                {
                    if (!bullet.gameObject.activeSelf)
                    {
                        bullet.position = firePoint.position;
                        bullet.rotation = transform.rotation;

                        if (bulletNum > 1)
                        {
                            bullet.transform.Rotate(0, 0, Random.Range(-15f, 15f));
                        }
                        else if (weaponType == WeaponType.MiniGun)
                        {
                            bullet.transform.Rotate(0, 0, Random.Range(-8f, 8f));
                        }

                        bullet.gameObject.SetActive(true);
                        Physics.IgnoreCollision(holdingPlayerCollider, bullet.GetComponent<Collider>(), true);
                        bullet.GetComponent<playerBulletScript>().Shoot(damage, projectileSpeed, projectileSize, holdingPlayerCollider);
                        audioSource.PlayOneShot(fire);

                        break;
                    }
                }
            }

            canFire = false;
            StartCoroutine(WaitToFire());

            ammoCount--;

            if (ammoCount <= 0)
            {
                audioSource.PlayOneShot(empty);
                Drop();
                Invoke("Despawn", 3);
            }
        }
    }


    private void ShootRocket()
    {
        if (canFire && ammoCount > 0 & pickedUp)
        {
            foreach (Transform bullet in bulletParent)
            {
                if (!bullet.gameObject.activeSelf)
                {
                    bullet.position = firePoint.position;
                    bullet.rotation = transform.rotation;

                    if (bulletNum > 1)
                    {
                        bullet.transform.Rotate(0, 0, Random.Range(-15f, 15f));
                        Debug.Log("Bullet rotation: " + bullet.localRotation);
                    }

                    bullet.gameObject.SetActive(true);
                    Physics.IgnoreCollision(holdingPlayerCollider, bullet.GetComponent<Collider>(), true);
                    bullet.GetComponent<Rocket>().Shoot(damage, projectileSpeed, projectileSize, holdingPlayerCollider);
                    audioSource.PlayOneShot(fire);

                    break;
                }
            }

            canFire = false;
            StartCoroutine(WaitToFire());

            ammoCount--;

            if (ammoCount <= 0)
            {
                audioSource.PlayOneShot(empty);
                Drop();
                Invoke("Despawn", 3);
            }
        }

    }


    private void ThrowExplosive()
    {
        if (pickedUp)
        {
            pickedUp = false;
            canFire = false;
            transform.SetParent(null);

            rb.AddRelativeForce(-1000, 0, 0, ForceMode.Impulse);
            rb.useGravity = true;
            weaponCollider.enabled = true;
            Physics.IgnoreCollision(weaponCollider, holdingPlayerCollider, true);
            ammoCount--;

            StartCoroutine(Detonate());
        }
    }


    private void ShootGrenade()
    {
        if (canFire && ammoCount > 0 & pickedUp)
        {
            foreach (Transform bullet in bulletParent)
            {
                if (!bullet.gameObject.activeSelf)
                {
                    bullet.position = firePoint.position;
                    bullet.rotation = transform.rotation;

                    if (bulletNum > 1)
                    {
                        bullet.transform.Rotate(0, 0, Random.Range(-15f, 15f));
                        Debug.Log("Bullet rotation: " + bullet.localRotation);
                    }

                    bullet.gameObject.SetActive(true);
                    Physics.IgnoreCollision(holdingPlayerCollider, bullet.GetComponent<Collider>(), true);
                    bullet.GetComponent<Rigidbody>().AddRelativeForce(-1150, 0, 0, ForceMode.Impulse);
                    audioSource.PlayOneShot(fire);

                    bullet.GetComponent<Grenade>().Shoot(damage, 3f);
                    break;
                }
            }

            canFire = false;
            StartCoroutine(WaitToFire());

            ammoCount--;

            if (ammoCount <= 0)
            {
                audioSource.PlayOneShot(empty);
                Drop();
                Invoke("Despawn", 3);
            }
        }
    }


    public void RotateWeapon(GameObject target)
    {
        if (target == null)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            var rotateRightToUp = Quaternion.FromToRotation(Vector3.left, Vector3.forward);
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up) * rotateRightToUp;
        }
    }


    public void AIShoot(Collider sourceCollider)
    {
        if (canFire)
        {
            if (weaponType == WeaponType.Gun || weaponType == WeaponType.MiniGun)
            {
                for (int i = 0; i < bulletNum; i++)
                {
                    foreach (Transform bullet in bulletParent)
                    {
                        if (!bullet.gameObject.activeSelf)
                        {
                            bullet.position = firePoint.position;
                            bullet.rotation = transform.rotation;

                            if (bulletNum > 1)
                            {
                                bullet.transform.Rotate(0, 0, Random.Range(-15f, 15f));
                            }
                            else
                            {
                                bullet.transform.Rotate(0, 0, Random.Range(-10f, 10f));
                            }

                            bullet.gameObject.SetActive(true);
                            Physics.IgnoreCollision(sourceCollider, bullet.GetComponent<Collider>(), true);
                            bullet.GetComponent<playerBulletScript>().Shoot(damage, projectileSpeed, projectileSize, sourceCollider);

                            break;
                        }
                    }
                }
            }
            else if (weaponType == WeaponType.RocketLauncher)
            {
                foreach (Transform bullet in bulletParent)
                {
                    if (!bullet.gameObject.activeSelf)
                    {
                        bullet.position = firePoint.position;
                        bullet.rotation = transform.rotation;

                        if (bulletNum > 1)
                        {
                            bullet.transform.Rotate(0, 0, Random.Range(-15f, 15f));
                        }
                        else
                        {
                            bullet.transform.Rotate(0, 0, Random.Range(-10f, 10f));
                        }

                        bullet.gameObject.SetActive(true);
                        Physics.IgnoreCollision(sourceCollider, bullet.GetComponent<Collider>(), true);
                        bullet.GetComponent<Rocket>().Shoot(damage, projectileSpeed, projectileSize, sourceCollider);

                        break;
                    }
                }
            }

            audioSource.PlayOneShot(fire);
            canFire = false;
            StartCoroutine(WaitToFire());
        }
    }


    private IEnumerator Detonate()
    {
        yield return new WaitForSeconds(fuseTimer);

        explosionEffect.transform.parent = null;
        explosionEffect.SetActive(true);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            switch (hitColliders[i].gameObject.layer)
            {
                case 8: //Player
                    hitColliders[i].GetComponent<PlayerControl>().TakeHit(500, damage);
                    break;
                case 10: //Enemy
                    hitColliders[i].GetComponent<enemyMovment>().TakeDamage(damage);
                    break;
                case 13: //Terrain
                    hitColliders[i].GetComponent<destructableTile>().TakeDamage(damage);
                    break;
                case 15: //Ladder
                    hitColliders[i].GetComponent<ladder>().TakeDamage(damage);
                    break;
            }
        }

        gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        Despawn();
        Destroy(explosionEffect);
    }
    

    public void PickUp(Transform hand, GameObject player, int numPlayer)
    {
        canFire = true;
        pickedUp = true;
        handPos = hand;
        holdingPlayer = player;
        playerNum = numPlayer;
        holdingPlayerCollider = holdingPlayer.GetComponent<Collider>();
        weaponCollider.enabled = false;

        transform.position = handPos.position;
        transform.rotation = handPos.rotation;
        transform.parent = handPos;
        rb.useGravity = false;
    }


    public void Drop()
    {
        canFire = false;
        pickedUp = false;
        transform.SetParent(null);
        weaponCollider.enabled = true;
        transform.rotation = Quaternion.Euler(Random.Range(-25f, 35f), Random.Range(70f, 100f), Random.Range(-15f, 20f));
        rb.useGravity = true;
    }


    public void Despawn()
    {
        Destroy(gameObject);
    }


    private IEnumerator WaitToFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }


    #region PublicAccessors
    public bool PickedUp
    {
        get
        {
            return pickedUp;
        }
    }

    public int AmmoCount
    {
        get
        {
            return ammoCount;
        }
    }

    public bool AIHeld
    {
        set
        {
            aiHeld = value;
            canFire = value;
        }
    }
    #endregion
}
