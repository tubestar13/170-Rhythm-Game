using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public VibeCheck vibes;
    public Animator animator;
    public Transform attackPoint;
    public PlayerStats ps;
    public LayerMask enemyLayers;
    public float attackRange;
    public bool launched;

    int oldVibes;
    float vibing;

    private static int combo;


    // Start is called before the first frame update
    void Start()
    {
        oldVibes = -1;
        combo = 0;
        launched = false;
    }
    public void Punching()
    {
        //keep track of time
        vibing = vibes.checkVibes();
        //check if on-time and if a successful combo has not been initiated yet
        if (vibing > -1 && Mathf.Abs(vibes.songPosInBeats - oldVibes) > 0.2f)
        {
            Collider2D[] hitEnemies;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Whiff");
            if (combo > 2)
                combo = 0;
            combo++;
            animator.SetInteger("Combo", combo);
            animator.SetBool("Spike", false);
            if (launched)
            {
                hitEnemies = Physics2D.OverlapCircleAll(new Vector2(-3.6f, 3.45f), attackRange, enemyLayers);
                animator.SetBool("Launch", false);
                launched = false;
                animator.SetBool("Spike", true);
                foreach(Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Launched " + enemy.name);
                    enemy.GetComponent<Enemy>().TakeDamage(33);
                    if(ps.currentHealth != ps.maxHealth)
                    {
                        ps.currentHealth += 10;
                        ps.healthBar.setHealth(ps.currentHealth);
                    }
                }
            }
            else
            {
                hitEnemies = Physics2D.OverlapCircleAll(new Vector2(-3.6f, 1.45f), attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Hit! " + enemy.name);
                    enemy.GetComponent<Enemy>().TakeDamage(34);
                    if (ps.currentHealth != ps.maxHealth)
                    {
                        ps.currentHealth += 10;
                        ps.healthBar.setHealth(ps.currentHealth);
                    }
                }
            }
            oldVibes = (int)vibing;
            return;
        }
        else
        {
            Debug.Log("You have failed the Vibe Check");
            combo = 0;
            animator.SetInteger("Combo", combo);
            animator.SetBool("Launch", false);
            return;
        }
    }

    public void HoldPunch()
    {
        
        vibing = vibes.checkVibes();
        if(vibing > -1 && Mathf.Abs(vibes.songPosInBeats - oldVibes) > 0.2f)
        {
            animator.SetBool("Launch", true);
            launched = true;
            Collider2D[] hitEnemies;

            Debug.Log("launcher\n" + vibing);
            combo++;
            animator.SetInteger("Combo", combo);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Whiff");
            hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            oldVibes = (int)vibing;
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(1);
                if(ps.currentHealth != ps.maxHealth)
                {
                    Debug.Log("Healing");
                    ps.currentHealth += 10;
                    ps.healthBar.setHealth(ps.currentHealth);
                }
            }
            return;
        }
        else
        {
            Debug.Log("You have failed the Vibecheck");
            combo = 0;
            animator.SetInteger("Combo", combo);
            return;
        }
        
    }

    public void CheckForDrop()
    {
        if(vibes.songPosInBeats - oldVibes > 1.2f && combo != 0) //if the recorded beat is not n-1, reset combo
        {
            Debug.Log("Dropped Combo");
            combo = 0;
            animator.SetInteger("Combo", combo);
            animator.SetBool("Launch", false);
            launched = false;
            return;
        }
        else
            return;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
