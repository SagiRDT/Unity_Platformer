using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSight : MonoBehaviour
{
    // Config
    [SerializeField] Enemy enemy = null;
    [SerializeField] [Range(0, 2)] float attackIntervals = 0;

    // Cached component references
    private BoxCollider2D mySightCollider2D;

    // Variables
    bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        mySightCollider2D = GetComponent<BoxCollider2D>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    private void Update()
    {
        bool isSpotingAPlayer = mySightCollider2D.IsTouchingLayers(LayerMask.GetMask("Player"));

        if (isSpotingAPlayer)
        {
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(AttackOnce());
            }
        }
    }

    private IEnumerator AttackOnce()
    {
        //if (!canAttack) yield return null;

        enemy.Attack();

        // if theres an attack intevals delay wait for it
        yield return new WaitForSeconds(attackIntervals);

        canAttack = true;
    }

}
