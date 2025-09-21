using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    
    // Temps d'attente entre chaque tir (2 secondes)
    public float fireCooldown = 1f;
    private float lastFireTime; // Temps du dernier tir

    void Update()
    {
        // Si la touche X est pressée et qu'on a dépassé le cooldown
        if (Keyboard.current.xKey.wasPressedThisFrame && Time.time >= lastFireTime + fireCooldown)
        {
            Shoot();
            lastFireTime = Time.time; // Met à jour le temps du dernier tir
        }
    }

    void Shoot()
    {
        // On instancie la balle à la position + rotation du FirePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}