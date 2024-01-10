using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProgressiveDamage : MonoBehaviour
{
    public float damagePerSecond=0.2f;

    public float damagePerSeconds() {
        return damagePerSecond;
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().danarEnemigo(damagePerSecond /** Time.deltaTime*/);

            //Debug.Log("Detectado");
            //BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            //if (enemy != null)
            //{
            //    Debug.Log("dañado");

            //    enemy.danarEnemigo(damagePerSecond/* * Time.deltaTime);
            //}
        }
    }

}
