using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Enemy Died");

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
