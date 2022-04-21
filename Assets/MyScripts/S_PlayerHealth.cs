using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_PlayerHealth : MonoBehaviour
{
    public int playerHealth;
    public Slider healthBar;
    public GameObject Hero;
    [SerializeField] GameObject vfxPlayerExplosion;

    private void Awake()
    {
        healthBar = GetComponent<Slider>();
        healthBar.value = playerHealth;
        //Hero = GetComponent<GameObject>();
    }

    public void Func_UpdateHealthBar()
    {
        healthBar.value = playerHealth;
        
    }

    public void Func_HeroDeath()
    {
        if(healthBar.value == 0)
        {
            Vector3 playerPosition = gameObject.transform.position;
            Quaternion playerRotation = Quaternion.identity;

            GameObject vfxExplosion; // Create vfx Game Object || Temporary Object to Assign Spawned Particle System to Destroy it after Done with Animation

            Destroy(Hero); // Destroy himself (Player)
            vfxExplosion = Instantiate(vfxPlayerExplosion, playerPosition, playerRotation);

            Destroy(vfxExplosion, 15f);
        }
    }

}
