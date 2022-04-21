using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_EnemyHealth : MonoBehaviour
{

    public int enemyHealth;
    public Slider healthBar;
    public GameObject Enemy;
    [SerializeField] GameObject vfxEnemyExplosion;

    private void Awake()
    {
        healthBar = GetComponent<Slider>();
        healthBar.value = enemyHealth;
        //Hero = GetComponent<GameObject>();
    }

    public void Func_UpdateHealthBar()
    {
        healthBar.value = enemyHealth;

    }

}

