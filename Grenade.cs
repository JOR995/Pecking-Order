using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Transform grenadeParent;
    private GameObject explosionEffect;
    private AudioSource audioSource;

    public float damage, fuseTimer;
    private bool fired;


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot(float bulletDamage, float timer)
    {
        explosionEffect = transform.GetChild(0).gameObject;
        grenadeParent = transform.parent;

        fuseTimer = timer;
        damage = bulletDamage;
        fired = true;

        StartCoroutine(Detonate());
    }


    private void Despawn()
    {
        explosionEffect.transform.parent = transform;
        explosionEffect.transform.localPosition = Vector3.zero;
        explosionEffect.SetActive(false);
        gameObject.SetActive(false);
    }


    private IEnumerator Detonate()
    {
        yield return new WaitForSeconds(fuseTimer);

        explosionEffect.transform.parent = null;
        explosionEffect.SetActive(true);
        audioSource.PlayOneShot(audioSource.clip);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null)
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
        }

        fired = false;
        gameObject.transform.position = grenadeParent.position;

        yield return new WaitForSeconds(3f);
        Despawn();
    }

}
