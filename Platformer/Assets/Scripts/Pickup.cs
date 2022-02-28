using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Config
    [SerializeField] AudioClip pickupSFX = null;
    [SerializeField] AnimationClip CollectedAnimation = null;
    [SerializeField] int pointsForPickup = 100;
    [SerializeField] int livesForPickup = 0;

    // Cached component references
    Animator myAnimator;
    bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Handle a collision with a pickup object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // the bool isColliding makes sure that a pickup object can be triggered only once)
        if (isColliding) return;

        isColliding = true;

        // play the pickup collecting animation and sfx
        myAnimator.SetTrigger("IsCollected");
        AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);

        // add the score and/or lives that the curr pickup holds
        FindObjectOfType<GameSession>().addToScore(pointsForPickup);
        FindObjectOfType<GameSession>().addToLives(livesForPickup);

        // destroy the curr pickup obj
        Destroy(gameObject, CollectedAnimation.length - 0.05f);
    }
}
