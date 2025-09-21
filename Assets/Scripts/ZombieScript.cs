using UnityEngine;
using Pathfinding;

public class ZombieScript : MonoBehaviour
{
    public AIPath aiPath;
    private Vector3 initialScale;
    
    // 🔹 Système de vie
    public int maxHealth = 3;
    private int currentHealth;
    
    // 🔹 Système de détection du joueur
    public float detectionRange = 10f; // Distance de détection du joueur
  
    private Transform player;
    private bool isPlayerDetected = false;
    
    // 🔹 Options de comportement
    public bool showDetectionGizmo = true; // Pour voir la zone de détection dans l'éditeur
  
    
    void Start()
    {
        initialScale = transform.localScale;
        
        // Initialise la vie
        currentHealth = maxHealth;
        
        // Trouve le joueur dans la scène
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'Player' trouvé! Assurez-vous que votre joueur a le tag 'Player'.");
        }
        
        // Désactive l'IA au départ
        if (aiPath != null)
        {
            aiPath.canMove = false;
        }
    }
    
    void Update()
    {
        // Vérifie la distance avec le joueur
        CheckPlayerDistance();
        
        // Flip du sprite en fonction de la direction (seulement si en mouvement)
        if (isPlayerDetected && aiPath != null && aiPath.canMove)
        {
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
            }
        }
    }
    
    void CheckPlayerDistance()
    {
        if (player == null || aiPath == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Active le zombie si le joueur est dans la zone de détection
        if (!isPlayerDetected && distanceToPlayer <= detectionRange)
        {
            ActivateZombie();
        }

    }
    
    void ActivateZombie()
    {
        isPlayerDetected = true;
        aiPath.canMove = true;
        
        // Définit le joueur comme cible
        AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();
        if (destinationSetter != null)
        {
            destinationSetter.target = player;
        }
        
        Debug.Log(name + " a détecté le joueur!");
    }
    
    void DeactivateZombie()
    {
        isPlayerDetected = false;
        aiPath.canMove = false;
        
        // Optionnel : réinitialise la vélocité pour arrêter immédiatement
        aiPath.SetPath(null);
        
        Debug.Log(name + " a perdu le joueur de vue.");
    }
    
    // 🔹 Appelé quand le zombie prend un dégât
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(name + " a perdu " + damage + " PV. Restant : " + currentHealth);
        
        // Optionnel : Active le zombie s'il prend des dégâts (même hors de portée)
        if (!isPlayerDetected)
        {
            ActivateZombie();
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // 🔹 Mort du zombie
    void Die()
    {
        Debug.Log(name + " est mort !");
        
        // Si le zombie a un parent (Enemy), on détruit le parent
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject); // Sinon on détruit juste le zombie
        }
    }
    
    // 🔹 Dessine la zone de détection dans l'éditeur Unity
    void OnDrawGizmosSelected()
    {
        if (!showDetectionGizmo) return;
        
        // Zone de détection en vert
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    
    }
}