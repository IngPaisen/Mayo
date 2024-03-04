using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemigoSigueSiempreAlDetectar : EnemigoBase
{

    [Header("Lado a Lado")]
    //[SerializeField] Transform[] posiciones;
    [SerializeField] Transform controladorSuelo;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float tiempoEspera;
    [SerializeField] float velocidadPatrullaje;
    [SerializeField] float distancia;
    [SerializeField] float distanciaDeteccionMuro;
    [SerializeField] private bool moviendoDer;
    [SerializeField] private bool caminando;
    [SerializeField] private bool moviendoDerecha = true; // Variable para mantener la dirección actual del movimiento del enemigo
    [SerializeField] LayerMask capaMuro;

    [Header("Detectar Player")]
    [SerializeField] float velocidadChase;
    [SerializeField] private bool playerDetectado;
    [SerializeField] private bool playerAlejado;
    [SerializeField] private bool detectado;
    [SerializeField] Transform player;
    [SerializeField] Transform puntoOrigenDetector;
    [SerializeField] LayerMask layerplauer;
    [SerializeField] float radioDeteccion;
    [SerializeField] float radioDeteccionAlejado;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //playerDetectado = Physics2D.OverlapCircle(puntoOrigenDetector.position, radioDeteccion, layerplauer);

        //if (playerDetectado) {
        //    detectado = true;
        //    playerAlejado = Physics2D.OverlapCircle(puntoOrigenDetector.position, radioDeteccionAlejado, layerplauer);

        //    if (!playerAlejado) {
        //        detectado = false;
        //    }
        //}

        playerDetectado = Physics2D.OverlapCircle(puntoOrigenDetector.position, radioDeteccion, layerplauer);

        if (playerDetectado)
        {
            detectado = true;
           
        }


        playerAlejado = Physics2D.OverlapCircle(puntoOrigenDetector.position, radioDeteccionAlejado, layerplauer);
        if (playerAlejado != true)
        {
            detectado = false;
            Debug.Log("salio");
        }

    }
    private void FixedUpdate()
    {
        if (caminando && !detectado)
        {
            saberADondeSeQuedoViendo();
            // Lógica para patrullar
            RaycastHit2D imfoSuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down, distancia);
            RaycastHit2D infoMuro = Physics2D.Raycast(controladorSuelo.position, Vector2.right, distanciaDeteccionMuro, capaMuro);
            
            //tal vez cambiarlo para que sea por tag, en luga de capamMuro, pero idk

            rb.velocity = new Vector2(velocidadPatrullaje, rb.velocity.y);

            if (infoMuro)
            {
                Girar();
            }
                
            if (imfoSuelo == false)
            {
                //gira el personaje
                Girar();
            }
        }
        else if (detectado)
        {

            // Encuentra la posición del jugador
            player = GameObject.FindGameObjectWithTag("Player").transform;

            // Calcula la dirección hacia el jugador
            Vector2 direccion = (player.position - transform.position).normalized;

            //rigidbody2.velocity = direccion * velocidad;
            rb.velocity = new Vector2(direccion.x * velocidadChase, rb.velocity.y);

            if (direccion.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }
        //else
        //{
        //    // Si el jugador no está detectado, la velocidad se establece en cero
        //    rb.velocity = Vector2.zero;
        //}
    
    }

    void Girar()
    {
        caminando = false;
        StartCoroutine(esperaLado());

    }

    IEnumerator esperaLado()
    {
        yield return new WaitForSeconds(tiempoEspera);
        caminando = true;

        moviendoDer = !moviendoDer;
        transform.localScale = new Vector3(transform.localScale.x * -1,1, 1);
        velocidadPatrullaje *= -1;
    }

    void saberADondeSeQuedoViendo() {

        if (moviendoDer) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);


        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(controladorSuelo.transform.position, controladorSuelo.transform.position + Vector3.down * distancia);
        Gizmos.DrawLine(controladorSuelo.transform.position, controladorSuelo.transform.position + Vector3.right * distanciaDeteccionMuro);
        Gizmos.DrawWireSphere(puntoOrigenDetector.transform.position, radioDeteccion);
        Gizmos.DrawWireSphere(puntoOrigenDetector.transform.position, radioDeteccionAlejado);

    }
}
