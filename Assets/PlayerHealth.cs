using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 20;
    public int currentHealth;

    public HealthBar healthBar;

    public GameManagerScript gameManager;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            gameManager.GameOver();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            TakeDamage(4);
        }else if(collision.gameObject.tag == "Wave")
        {
            TakeDamage(2);
        }else if(collision.gameObject.tag == "Potion")
        {
            RecoverDamage(6);
        }

    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthBar.SetHealth(currentHealth);

    }

    public void RecoverDamage(int heal)
    {
        currentHealth += heal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }
}
