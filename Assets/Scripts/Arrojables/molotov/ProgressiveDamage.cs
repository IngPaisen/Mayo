using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProgressiveDamage : MonoBehaviour
{
    //public float damagePerSecond=0.2f;


    //private void Update()
    //{

    //}
    //public float damagePerSeconds() {
    //    return damagePerSecond;
    //}


    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        other.GetComponent<BaseEnemy>().danarEnemigo(damagePerSecond * Time.deltaTime);

    //        Debug.Log("Detectado");
    //        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
    //        if (enemy != null)
    //        {
    //            Debug.Log("dañado");

    //            enemy.danarEnemigo(damagePerSecond/* * Time.deltaTime);
    //        }
    //    }
    //}

    public float danioPorSegundo = 0.2f;
    private List<BaseEnemy> enemigosDentro;

    private void Start()
    {
        enemigosDentro = new List<BaseEnemy>();
    }

    private void Update()
    {
        foreach (BaseEnemy enemy in enemigosDentro)
        {
            enemy.danarEnemigo(danioPorSegundo * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemigosDentro.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemigosDentro.Remove(enemy);
            }
        }
    }

}
