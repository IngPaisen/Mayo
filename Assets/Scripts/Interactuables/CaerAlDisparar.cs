using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaerAlDisparar : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float danioAEnemigos;
    [SerializeField] float gravedadAlCaer;
    [SerializeField] int layerAttacks=9;
    [SerializeField] int golpesParaDestruirse; //cantidad de golpes que puede impactar para que se rompa
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("PlayerAttack")) {
            rb.gravityScale = gravedadAlCaer;
            Physics2D.IgnoreLayerCollision(gameObject.layer,/* LayerMask.NameToLayer("PlayerAttack")*/layerAttacks, true);



        }

        if (collision.transform.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<BaseEnemy>().danarEnemigo(danioAEnemigos);//dañar al enemigo
            checarImpactos();
        }
        else if(collision.transform.CompareTag("piso"))
        {//si impacta con otra cosa
            //checarImpactos();
            Destroy(gameObject); //animacion de destuiccion
        }
    }

    private void checarImpactos()
    {
        golpesParaDestruirse-=1;
        //golpesParaDestruirse--;
        if (golpesParaDestruirse <= 0)
        {
            Destroy(gameObject);

        }
    }
}
