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
    int oldVibes;
    float vibing;
    private static int combo;

    // Start is called before the first frame update
    void Start()
    {
        oldVibes = -1;
        combo = 0;
    }
    public void Punching()
    {
        //keep track of time
        vibing = vibes.checkVibes();
        //check if on-time and if a successful combo has not been initiated yet
        if (vibing > -1 && Mathf.Abs(vibes.songPosInBeats - oldVibes) > 0.2f)
        {
            Collider2D[] hitEnemies;
            switch (combo) //Figure out where in the combo player is
            {
                case 0:
                    Debug.Log("punch 1\n" + vibing);
                    combo++;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Whiff");
                    animator.SetInteger("Combo", combo);
                    attackPoint.localPosition = new Vector2(0.45f, 0.45f);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
                case 1:
                    Debug.Log("Punch 2\n" + vibing);
                    combo++;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Whiff");
                    animator.SetInteger("Combo", combo);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
                case 2:
                    Debug.Log("Punch 3 End of Combo\n" + vibing);
                    combo++;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Whiff");
                    animator.SetInteger("Combo", combo);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
                default:
                    combo = 0;
                    Debug.Log("punch 1\n" + vibing);
                    combo++;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Whiff");
                    animator.SetInteger("Combo", combo);
                    attackPoint.localPosition = new Vector2(0.45f, 0.45f);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
            }
            oldVibes = (int)vibing;
            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("Hit! " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(34);
                if(ps.currentHealth != ps.maxHealth)
                {
                    ps.currentHealth += 10;
                    ps.healthBar.setHeatlh(ps.currentHealth);
                }
            }
            return;
        }
        else
        {
            Debug.Log("You have failed the Vibe Check");
            combo = 0;
            animator.SetInteger("Combo", combo);
            return;
        }
    }

    public void HoldPunch()
    {
        
            vibing = vibes.checkVibes();
            if(vibing > -1 && Mathf.Abs(vibes.songPosInBeats - oldVibes) > 0.2f)
            {
                switch (combo)
                {
                    case 1:
                        Debug.Log("launcher End of Combo\n" + vibing);
                        combo = 0;
                        animator.SetInteger("Combo", combo);
                        break;
                    default:
                        Debug.Log("not a combo\n" + vibing);
                        combo = 0;
                        animator.SetInteger("Combo", combo);
                        break;
                }
                oldVibes = (int)vibing;
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
