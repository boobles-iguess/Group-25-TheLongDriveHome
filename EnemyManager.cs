using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] enemies; 
    
    public int GetEnemyIndex(GameObject enemyObject)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemyObject.name == enemies[i].enemyName)
            {
                return i; //return the matching index
            }
        }
        return -1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject enemy1 = new GameObject("ShadowMan");
        Enemy shadowMan = enemy1.AddComponent<Enemy>();
        shadowMan.Initialize("ShadowMan", 100f, 5f, 10);
    }

   

    public void EnemyTakeDamage(int index, float amount)
    {
        if (index < 0 || index >= enemies.Length) return;

        enemies[index].enemyHealth -= amount;

        if (enemies[index].enemyHealth <= 0f)
        {
            Destroy(gameObject); //destroys enemy if enemy health depletes
        }
    }

}
