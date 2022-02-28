/*
 *  Shooter
 *  The shooter class makes any objs that holds it shot a projectile
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // Consts
    const string PROJECTILE_PARENT_NAME = "Projectiles";

    // Config params
    [SerializeField] GameObject gun = null;
    [SerializeField] Projectile projectile = null;

    // Cached component references
    GameObject projectileParent;

    private void Start()
    {
        CreateProjectileParent();
    }

    // Create a projectile parent
    private void CreateProjectileParent()
    {
        projectileParent = GameObject.Find(PROJECTILE_PARENT_NAME);
        if (!projectileParent)
        {
            projectileParent = new GameObject(PROJECTILE_PARENT_NAME);
        }
    }

    // Shoot the projectile
    public void Fire()
    {
        Projectile newProjectile = Instantiate(projectile, gun.transform.position, transform.rotation) as Projectile;
        newProjectile.transform.parent = projectileParent.transform;
    }

}
