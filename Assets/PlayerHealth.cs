using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 20;
    private int currentHealth;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Air Ball")
        {
            TakeDamage(4);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public void RecoverDamage(int heal)
    {
        currentHealth += heal;

        healthBar.SetHealth(currentHealth);
    }
}
