using UnityEngine;
using Pathfinding;

public class ZombieScript : MonoBehaviour
{
    public AIPath aiPath;

    private Vector3 initialScale;

    void Start()
    {
    
        initialScale = transform.localScale;
    }

    void Update()
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
