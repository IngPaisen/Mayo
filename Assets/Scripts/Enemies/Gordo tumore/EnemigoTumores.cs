using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoTumores : EnemigoBase
{
    [Header("Embestida")]
    //[SerializeField] float radioEmbestida;
    [SerializeField] float velocidadEmbestida;
    [SerializeField] float duracionEmbestida;
    [SerializeField] TrailRenderer trailEmbestida;
    [SerializeField] float tiempoQuieto;

    [Header("Area Dañina")]
    [SerializeField] bool playerDentroDeRAngoDañino = false;
    [SerializeField] float dañoArea=5;

    [Header("Lanzar Espora")]
    [SerializeField] GameObject espora;
    [SerializeField] float fuerzaLanzamiento;
    [SerializeField] Transform posicionLanzamientoEspora;
    [SerializeField] float tiempoQuietoEspora;
    [SerializeField] float chanfleLanzamiento;

    [Header("General")]
    [SerializeField] float velocidadenemigo;
    [SerializeField] float cooldownEntreAtaques;
    private Transform player;
    //[SerializeField] LayerMask layerPlayer;
    //[SerializeField] bool enRangoEmbestida;
    //[SerializeField] bool enRangoGolpe;
    Rigidbody2D rb;

    bool atacando;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        trailEmbestida = GetComponent<TrailRenderer>();
        trailEmbestida.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checarColisiones(); //-> de momento inutil

        checarVida();
        if (cooldownEntreAtaques <= 0)
        {
            int ataqueProximo = Random.Range(0,2);
            atacando = true;


            Debug.Log(ataqueProximo);
            switch (ataqueProximo)
            {

                case 0:
                    Embestida(); 

                    break;

                case 1:
                    //activar anuimacion para lazanr la espora
                    StartCoroutine(QuedarseQuietoEspora()); // -> tal vez no sea necesario, en un futuri
                   
                    break;
            }

            cooldownEntreAtaques = Random.Range(4f, 5.5f); // reiniciar el cooldown entre ataques

        }
        else if (!atacando)
        {

            cooldownEntreAtaques -= Time.deltaTime; // reinicar el cooldown
            //atacando = false;
        }

        dañarAreaPlayer();
    }

    private void FixedUpdate()
    {
        if (!atacando)
        {
            MoverHaciaJugadorLentamente();

        }

    }


    void checarColisiones()
    {
        //Physics2D.OverlapCircleAll(transform.position, radioEmbestida, layerPlayer);
    }

    //=============================================== ATAQUES ===========================================================
    void Embestida()
    {

        if (vidaEnemy > 600)
        {
            StartCoroutine(Embestir());

        }
        else if (vidaEnemy < 600 && vidaEnemy > 300)
        {
            StartCoroutine(Embestir2());

        }
        else if (vidaEnemy < 300) { 
        
            StartCoroutine(Embestir3());
        }
        //StartCoroutine(QuedarseQuieto());

    }

    //private IEnumerator Embestir()
    //{
    //    atacando = true;
    //    trailEmbestida.emitting = true;
    //    Vector2 direction = (player.position - transform.position).normalized;
    //    direction.y = 0;

    //    float velocidadInicial = velocidadEmbestida;

    //    rb.velocity = direction * velocidadInicial;

    //    velocidadInicial -= Time.deltaTime;

    //    if (velocidadInicial < 0)
    //    {
    //        velocidadInicial = 0;
    //    }
    //        yield return new WaitForSeconds(duracionEmbestida);
    //        trailEmbestida.emitting = false;

    //        StartCoroutine(QuedarseQuieto());

    //}

    #region Embestidas
    private IEnumerator Embestir()
    {
        Debug.Log("embstida 1");
        atacando = true;
        trailEmbestida.emitting = true;
        Vector2 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        float velocidadInicial = velocidadEmbestida;

        // Ajusta la velocidad inicial para que la embestida dure exactamente la duración especificada
        float tiempoTotal = duracionEmbestida;
        float tiempoActual = 0;
        while (tiempoActual < duracionEmbestida)
        {
            rb.velocity = direction * velocidadInicial;

            tiempoActual += Time.deltaTime;
            velocidadInicial = Mathf.Lerp(velocidadEmbestida, 0, tiempoActual / tiempoTotal);

            yield return null;
        }


        trailEmbestida.emitting = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(tiempoQuieto); // Espera 1 segundo

        atacando = false;
    }


    private IEnumerator Embestir2()
    {
        Debug.Log("embstida 2");

        atacando = true;
        trailEmbestida.emitting = true;
        for (int i = 0; i <= 1; i++)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            float velocidadInicial = velocidadEmbestida;

            // Ajusta la velocidad inicial para que la embestida dure exactamente la duración especificada
            float tiempoTotal = duracionEmbestida;
            float tiempoActual = 0;
            while (tiempoActual < duracionEmbestida)
            {
                rb.velocity = direction * velocidadInicial;

                tiempoActual += Time.deltaTime;
                velocidadInicial = Mathf.Lerp(velocidadEmbestida, 0, tiempoActual / tiempoTotal);

                yield return null;
            }


        }
            trailEmbestida.emitting = false;
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(tiempoQuieto); // Espera 1 segundo

            atacando = false;
        
    
    }
    private IEnumerator Embestir3()
    {
        Debug.Log("embstida 3");

        atacando = true;
        trailEmbestida.emitting = true;
        for (int i = 0; i <= 2; i++)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            float velocidadInicial = velocidadEmbestida;

            // Ajusta la velocidad inicial para que la embestida dure exactamente la duración especificada
            float tiempoTotal = duracionEmbestida;
            float tiempoActual = 0;
            while (tiempoActual < duracionEmbestida)
            {
                rb.velocity = direction * velocidadInicial;

                tiempoActual += Time.deltaTime;
                velocidadInicial = Mathf.Lerp(velocidadEmbestida, 0, tiempoActual / tiempoTotal);

                yield return null;
            }

        }

            trailEmbestida.emitting = false;
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(tiempoQuieto); // Espera 1 segundo

            atacando = false;
        
    }

#endregion


    #region esporas 
    void LanzarEspora()
    {
        GameObject nuevaEspora = Instantiate(espora, posicionLanzamientoEspora.position, Quaternion.identity);
        Vector3 direccion = (player.position - transform.position).normalized;

        Rigidbody2D rbEspora = nuevaEspora.GetComponent<Rigidbody2D>();
        rbEspora.velocity = direccion * fuerzaLanzamiento;
        atacando = false;
    }
    void LanzarEsporaChanfle()
    {
        GameObject nuevaEspora = Instantiate(espora, posicionLanzamientoEspora.position, Quaternion.identity);
        Vector3 direccion = (player.position - transform.position).normalized;
        direccion.y += chanfleLanzamiento;

        Rigidbody2D rbEspora = nuevaEspora.GetComponent<Rigidbody2D>();
        rbEspora.velocity = direccion * fuerzaLanzamiento;
        atacando = false;
    }
    void LanzarEsporaChanfleX2()
    {
        Debug.Log("3");
        GameObject nuevaEspora = Instantiate(espora, posicionLanzamientoEspora.position, Quaternion.identity);
        Vector3 direccion = (player.position - transform.position).normalized;
        direccion.y += ((chanfleLanzamiento*1.5f));

        Rigidbody2D rbEspora = nuevaEspora.GetComponent<Rigidbody2D>();
        rbEspora.velocity = direccion * fuerzaLanzamiento;
        atacando = false;
    }

    #endregion
    void MoverHaciaJugadorLentamente()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        direction.y = 0;


        // Movemos al enemigo hacia el jugador con una velocidad más lenta
        rb.velocity = direction * velocidadenemigo;
    }

    void dañarAreaPlayer() {

        if (playerDentroDeRAngoDañino) {
            player.GetComponent<PlayerStatsE>().enAreaDañina(dañoArea);
        }
    }

    //===================================================================================================================


    //===================================================================================================================

    IEnumerator QuedarseQuieto()
    {
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(tiempoQuieto);
        rb.velocity = Vector2.zero;

        atacando = false;


    }
    IEnumerator QuedarseQuietoEspora()
    {

        yield return new WaitForSeconds(tiempoQuietoEspora);
        rb.velocity = Vector2.zero;
        LanzarEspora();
        if (vidaEnemy < 600 && vidaEnemy>300)
        {
            LanzarEsporaChanfle();

        }
        else if (vidaEnemy < 300) {
            LanzarEsporaChanfle();

            LanzarEsporaChanfleX2();
        }

    }

    //===================================================================================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("PlayerAttack"))
        {
            recibirDaño(collision.transform.GetComponent<PlayerAttacks>().getDanioAtaque());
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, radioEmbestida);

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerDentroDeRAngoDañino = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerDentroDeRAngoDañino = false;
        }
    }

}

