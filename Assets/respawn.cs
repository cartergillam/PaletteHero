using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;
    public HealthBar healthBar; // Add a reference to the HealthBar script in the inspector

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            // Reduce the player's health by one heart
            healthBar.TakeDamage(1);
            // Move the player to the respawn point
            player.transform.position = respawnPoint.position;
        }
    }
}
