using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public VibeCheck vibes;
    public PlayerStats ps;

    public int maxHealth = 33;
    public int spawnOffset;
    public int divisionOffset;
    int currHealth;

    Vector3 startValue;
    Vector3 endValue;

    enum EnemyState { MOVING, ATTACKING, LAUNCHED };
    EnemyState state;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        vibes = GameObject.FindGameObjectWithTag("Music").GetComponent<VibeCheck>();
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        startValue = this.transform.parent.transform.position;
        spawnOffset = (int)vibes.songPosInBeats + 6;
        divisionOffset = 6;
        endValue = new Vector3(-3.6f, .8f, 0);
        state = EnemyState.MOVING;

    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;

        if(damage == 1)
        {
            animator.SetTrigger("Launch");
            state = EnemyState.LAUNCHED;
        }
        else if(damage == 33)
        {
            animator.SetTrigger("Spiked");
        }
        else
            animator.SetTrigger("Hurt");

        if (currHealth <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.03f);
        Debug.Log("Enemy Died");
        this.transform.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }

    void LerpLeft()
    {
        
        this.transform.parent.transform.position = Vector3.Lerp(startValue, endValue, 1f - ((spawnOffset + 0.2f - vibes.songPosInBeats)/divisionOffset));
    }

    void HurtPlayer()
    {
        Debug.Log("You missed");
        ps.currentHealth -= 10;
        ps.healthBar.setHealth(ps.currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //enemy movement 
        if (this.transform.parent.transform.position != endValue)
            LerpLeft();

        //Despawn if too late
        if(spawnOffset + 0.2 < vibes.songPosInBeats && currHealth > 0)
        {
            if (state == EnemyState.MOVING)
            {
                state = EnemyState.ATTACKING;
                HurtPlayer();
                StartCoroutine(Die());
            }

            if (state == EnemyState.LAUNCHED)
            {
                spawnOffset+=1;
                state = EnemyState.MOVING;
            }
        }
        
    }
}
