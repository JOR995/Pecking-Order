using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Collider heldPlayerCollider;
    private Transform rocketParent;
    private GameObject explosionEffect;
    private AudioSource audioSource;

    public float damage, travelSpeed;
    private bool fired;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }


    void Update()
    {
        if (fired)
        {
            transform.Translate(Vector3.left * travelSpeed * Time.deltaTime);
        }
    }


    public void Shoot(float bulletDamage, float bulletSpeed, float bulletSize, Collider originCollider)
    {
        explosionEffect = transform.GetChild(0).gameObject;
        rocketParent = transform.parent;

        damage = bulletDamage;
        travelSpeed = bulletSpeed;
        fired = true;
        heldPlayerCollider = originCollider;

        Invoke("EnableCollision", 0.2f);
        StartCoroutine(DespawnTimer());
    }


    private void EnableCollision()
    {
        if (heldPlayerCollider != null)
        {
            Physics.IgnoreCollision(heldPlayerCollider, GetComponent<Collider>(), false);
        }
    }


    private void Despawn()
    {
        explosionEffect.transform.parent = transform;
        explosionEffect.transform.localPosition= Vector3.zero;
        explosionEffect.SetActive(false);
        gameObject.SetActive(false);
    }


    private IEnumerator Detonate()
    {
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
        gameObject.transform.position = rocketParent.position;

        yield return new WaitForSeconds(3f);
        Despawn();
    }


    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(Detonate());
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (fired)
        {
            StopCoroutine(DespawnTimer());
            StartCoroutine(Detonate());
        }
    }
}
