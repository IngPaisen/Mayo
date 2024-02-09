using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoDebilEnemigo : MonoBehaviour
{

    EnemigoBase baseEnemy;
    [SerializeField] float danioPorHacer;
    void Start()
    {
        baseEnemy=  GetComponentInParent<EnemigoBase>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("PlayerAttack")) {
            baseEnemy.recibirDaño(danioPorHacer);
        }
    }
}
