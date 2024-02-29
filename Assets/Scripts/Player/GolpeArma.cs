using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GolpeArma : MonoBehaviour
{
    [SerializeField] float danio;


    public float retornarDanio()
    {
        return danio;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy")) {
            collision.transform.GetComponent<EnemigoBase>().recibirDaño(danio);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        { 
            collision.transform.GetComponent<EnemigoBase>().recibirDaño(danio);
        }
    }
}
