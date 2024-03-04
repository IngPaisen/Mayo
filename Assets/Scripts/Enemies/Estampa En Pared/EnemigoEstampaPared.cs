using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class EnemigoEstampaPared : EnemigoBase
{

    [SerializeField] float velocidadMovimiento;
    [SerializeField] float velocidadInicialEmbestida;
    [SerializeField] float velocidadMaximaEmbestida;
    [SerializeField] Transform player;
    [SerializeField] float aceleracion;
    [SerializeField] float tiempoEmbestida;
    [SerializeField] float ccEntreEmbestidaTotal;
    [SerializeField] float ccEntreEmbestidas;
    [SerializeField] float radioDeteccion;
    [SerializeField] float tiempoAturdido; 
    [SerializeField] LayerMask layerplayer;
    [SerializeField] bool embistiendo;
    [SerializeField] bool detectado;
    [SerializeField] bool estampado;
    Rigidbody2D rb;
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        detectado = Physics2D.OverlapCircle(transform.position, radioDeteccion, layerplayer);

        if (ccEntreEmbestidas > 0&& detectado)
        {
            ccEntreEmbestidas -= Time.deltaTime;
            embistiendo = false;
            estampado = false;
            velocidadInicialEmbestida = 0;
        }
        else if(ccEntreEmbestidas < 0 && detectado)
        { 
            embistiendo = true;
        }
    }

    private void FixedUpdate()
    {
        if (detectado && !embistiendo)
        {
            // Encuentra la posición del jugador
            player = GameObject.FindGameObjectWithTag("Player").transform;

            Vector2 direccion = (player.position - transform.position).normalized;

            //rigidbody2.velocity = direccion * velocidad;
            rb.velocity = new Vector2(direccion.x * velocidadMovimiento, rb.velocity.y);

            if (direccion.x > 0 && !embistiendo)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direccion.x < 0 && !embistiendo)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }
        else if ( embistiendo && !estampado) {

            if (velocidadInicialEmbestida > velocidadMaximaEmbestida)
            {
                velocidadInicialEmbestida = velocidadMaximaEmbestida;
            }
            else
            {
                velocidadInicialEmbestida += aceleracion;//modificar valores para las etapas
            }
            if (transform.localScale.x >=0)
            {
                rb.velocity = new Vector2(1 * velocidadInicialEmbestida, rb.velocity.y);
            }
            else { 
                rb.velocity = new Vector2(-1 * velocidadInicialEmbestida, rb.velocity.y);

            }
            StartCoroutine(terminarEmbestida());
        }
    }



    IEnumerator terminarEmbestida() { 
        yield return new WaitForSeconds(tiempoEmbestida);
        embistiendo = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall")) {
            rb.velocity = Vector2.zero;
            estampado = true;
            //activar animacion
            Debug.Log("Estampado");
            StartCoroutine(aturdido());
        }
    }

    IEnumerator aturdido() {
        yield return new WaitForSeconds(tiempoAturdido);
        embistiendo = false;
        ccEntreEmbestidas = ccEntreEmbestidaTotal;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

    }

}
