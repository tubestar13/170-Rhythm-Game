using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    VibeCheck vibes;
    public AudioSource audioSource;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange;
    int oldVibes;
    float vibing;
    public static int combo;

    // Start is called before the first frame update
    void Start()
    {
        vibes = audioSource.GetComponent<VibeCheck>();
        oldVibes = -1;
        combo = 0;
    }
    public int Punching(int combo)
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
                    animator.SetInteger("Combo", combo);
                    attackPoint.localPosition = new Vector2(0.31f, 0.05f);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
                case 1:
                    Debug.Log("Punch 2\n" + vibing);
                    combo++;
                    animator.SetInteger("Combo", combo);
                    attackPoint.localPosition = new Vector2(0.32f, 0.05f);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
                case 2:
                    Debug.Log("Punch 3 End of Combo\n" + vibing);
                    combo++;
                    animator.SetInteger("Combo", combo);
                    attackPoint.localPosition = new Vector2(0.5f, 0.05f);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
                default:
                    combo = 0;
                    Debug.Log("punch 1\n" + vibing);
                    combo++;
                    animator.SetInteger("Combo", combo);
                    attackPoint.localPosition = new Vector2(0.32f, 0.05f);
                    hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    break;
            }
            oldVibes = (int)vibing;
            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("Hit! " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(34);
            }
            return combo;
        }
        else
        {
            Debug.Log("You have failed the Vibe Check");
            combo = 0;
            animator.SetInteger("Combo", combo);
            return combo;
        }
    }

    public int HoldPunch(int combo)
    {
        if(vibes.songPosInBeats - oldVibes > 0.5f)
        {
            vibing = vibes.checkVibes();
            if(vibing > -1 && Mathf.Abs(vibes.songPosInBeats - oldVibes) > 0.2f)
            {
                switch (combo)
                {
                    case 1:
                        Debug.Log("launcher End of Combo\n" + vibing);
                        combo = 0;
                        break;
                    default:
                        Debug.Log("not a combo\n" + vibing);
                        combo = 0;
                        break;
                }
                oldVibes = (int)vibing;
                return combo;
            }
            else
            {
                Debug.Log("You have failed the Vibecheck");
                combo = 0;
                return combo;
            }
        }
        else 
            return combo;
    }

    public int CheckForDrop(int combo)
    {
        if(vibes.songPosInBeats - oldVibes > 1f && combo != 0) //if the recorded beat is not n-1, reset combo
        {
            Debug.Log("Dropped Combo");
            combo = 0;
            animator.SetInteger("Combo", combo);
            return combo;
        }
        else
            return combo;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
