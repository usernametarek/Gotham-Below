using UnityEngine;
using UnityEngine.InputSystem;

public class FirePointController : MonoBehaviour
{
    // Si FirePoint n'est pas enfant du Player, tu peux assigner manuellement le Player ici (optionnel)
    public Transform player;

    // Direction sauvegardée (par défaut vers la droite)
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        // Si aucun player assigné, tente de trouver un objet taggé "Player"
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        // On lit les touches flèches + WASD (facultatif)
        Vector2 input = Vector2.zero;
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) input.x += 1;

        if (input != Vector2.zero)
        {
            // Choisit l'axe dominant (horizontal ou vertical)
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                lastDirection = input.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                lastDirection = input.y > 0 ? Vector2.up : Vector2.down;
            }

            ApplyRotation(lastDirection);
        }

        // Si input == Vector2.zero -> ne rien faire, on garde la dernière rotation
    }

    void ApplyRotation(Vector2 dir)
    {
        float angle = 0f;
        if (dir == Vector2.right) angle = 0f;
        else if (dir == Vector2.left) angle = 180f;
        else if (dir == Vector2.up) angle = 90f;
        else if (dir == Vector2.down) angle = -90f;

        // Utilise la rotation locale (si FirePoint est enfant du player, c'est mieux)
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
