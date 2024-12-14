using UnityEngine;

public class Monster : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public Monster(int health)
    {
        maxHealth = health;
        currentHealth = health;
    }

    public void TakeDamge(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Die");
        }
    }
}
