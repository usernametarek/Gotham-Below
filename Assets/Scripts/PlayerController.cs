using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    public int lives = 10; 
    public HealthBar healthBar;

    private bool isInvincible = false;
    public float invincibleTime = 3f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        healthBar.SetMaxHealth(lives);
        healthBar.SetHealth(lives);
    }

    public void TakeHit()
    {
        if (isInvincible) return; 

        lives--;
        healthBar.SetHealth(lives);
        Debug.Log("Vies restantes : " + lives);

        if (lives <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincibility()); 
        }
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Game Over !");
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie"))
        {
            TakeHit(); 
        }
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Keyboard.current.upArrowKey.isPressed) movement.y += 1;
        if (Keyboard.current.downArrowKey.isPressed) movement.y -= 1;
        if (Keyboard.current.leftArrowKey.isPressed) movement.x -= 1;
        if (Keyboard.current.rightArrowKey.isPressed) movement.x += 1;

        movement = movement.normalized;

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("isMoving", movement.magnitude > 0);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
