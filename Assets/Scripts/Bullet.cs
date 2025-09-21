using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    public GameObject impactEffect;
    void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        ZombieScript zombie = hitInfo.GetComponent<ZombieScript>();
        if (zombie != null)
        {
            zombie.TakeDamage(1);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        
    }


}
