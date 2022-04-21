using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ShootController : MonoBehaviour
{
    public Rigidbody bulletRigidody;

    [SerializeField] GameObject vfxHitGreen;
    [SerializeField] GameObject vfxHitRed;

    private void Awake()
    {
        bulletRigidody = GetComponent<Rigidbody>(); // Initialize Rigitbody
    }

    private void Start()
    {
        float speed = 35f;
        bulletRigidody.velocity = transform.forward * speed; // Bullet Movement
    }

    public void OnTriggerEnter(Collider other) // On Touch with other Ojecty = On Trigger Enter 
    {
        GameObject vfxHit; // Create vfx Game Object || Temporary Object to Assign Spawned Particle System to Destroy it after Done with Animation

        //Check if Hit Object have Enemy Tag
        if (other.CompareTag("Enemy")) // If HAVE
        {
            vfxHit = Instantiate(vfxHitGreen, transform.position, Quaternion.identity); // Play Particle System vfxHitGreen and Assign it to vfxHit Object
        }
        else // IF DON'T HAVE
        {
            vfxHit = Instantiate(vfxHitRed, transform.position, Quaternion.identity); // Play Particle System vfxHitRed and Assign it to vfxHit Object
        }

        Destroy(vfxHit, 1f); // Destroy vfxHit Object (Spawned Particle) after 1 second waiting

        Destroy(gameObject); // Destroy the Bullet
    }

}
