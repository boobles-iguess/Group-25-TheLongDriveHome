using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float enemySpeed;
    public float enemyHealth;
    public int damage;
    
    public void Initialize(string name, float spd, float hp, int dmg)
    {
        enemyName = name;
        enemySpeed = spd;
        enemyHealth = hp;
        damage = dmg;
    }
}
