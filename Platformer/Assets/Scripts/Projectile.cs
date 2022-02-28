/*
 *  Projectile
 *  Handle the projectiles functionality, its speed and damage
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Config params
    [SerializeField] float speed = 1f;
    //[SerializeField] float damage = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    // Makes the projectile deal damage to the obj it collide with and than destroy it (the projectile)
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Destroy(gameObject, 1);
    }
}
