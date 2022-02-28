using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Config
    [SerializeField] float health = 1f;
    [SerializeField] int pointsForKill = 100;
    [SerializeField] AnimationClip DeathAnimation = null;

    // State
    bool isAlive = true;

    // Cached component references
    Animator myAnimator;

    // gets
    public bool IsAlive() { return isAlive; }

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        // play the hit animation
        myAnimator.SetTrigger("gotHit");

        // dec the enemy health by 1, if it reaches 0 kill the enemy
        health--;
        if(health <= 0)
        {
            isAlive = false;
            FindObjectOfType<GameSession>().addToScore(pointsForKill);
            Destroy(gameObject, DeathAnimation.length - 0.025f);
        }
    }

    public virtual void Attack()
    {
        //Debug.Log("Regular Attack");

        // play the attack animation
        myAnimator.SetTrigger("Attack");
    }

}

