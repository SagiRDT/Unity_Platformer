using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Config
    [SerializeField] float moveSpeed = 1f;

    // Cached component references
    Rigidbody2D myRigidbody;
    BoxCollider2D myBodyCollider2D;

    // Variables
    bool canBeFlipped = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider2D = gameObject.transform.Find("Body").GetComponent<BoxCollider2D>();
    }

    //------------------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //------------------------------------------------------------------------------------------------------------
   /*    
    private void HittingTheWall()
    {
        bool isTouchingTheWall = myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if( isTouchingTheWall )
        {
            FlipSprite();
            moveSpeed = myRigidbody.velocity.x * -1f;
        }
    }
    */
    //------------------------------------------------------------------------------------------------------------
    
    // handle the flipping of the enemy sprite horizontally (left or right)
    private IEnumerator FlipSprite()
    {
        transform.localScale = new Vector3(-(Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y, transform.localScale.y);

        // if theres an attack intevals delay wait for it
        yield return new WaitForSeconds(0.5f);

        canBeFlipped = true;
    }

    //------------------------------------------------------------------------------------------------------------

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool isEnemyAlive = GetComponent<Enemy>().IsAlive();
        if (!isEnemyAlive) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (canBeFlipped)
            {
                canBeFlipped = false;
                StartCoroutine(FlipSprite());
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------

    // return true if the enemy is facing right, else return false
    private bool IsFacingRight()
    {
        return (transform.localScale.x > 0);
    }

    //------------------------------------------------------------------------------------------------------------

    // making the enemy move
    private void Move()
    {
        if (IsFacingRight())
        {
            myRigidbody.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    //------------------------------------------------------------------------------------------------------------

}
