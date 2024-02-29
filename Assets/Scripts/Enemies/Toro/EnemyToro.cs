using System.Collections;
using System.Net;
using UnityEngine;

public class EnemyToro : EnemigoBase 
{
    [Header("Embestidas Toro")]
    Rigidbody2D rb;
    public float velocidadMaxima = 10f;
    [SerializeField] float velocidadActual;
    public float aceleracion = 2f;
    [SerializeField] float esperaAntesEmbestida;
    public Transform puntoA;
    public Transform puntoB;
    [SerializeField] int embestidasMaximas = 3;
    [SerializeField] private int embestidasRealizadas = 0;
    bool llego;

    [Header("Escombro")]
    [SerializeField] float esperaEscombroIncial;
    [SerializeField] float esperaEscombroFinal;
    [SerializeField] GameObject escombro;
    [SerializeField] Transform lugarSpawnEscombro;
    [SerializeField] Vector2 posicionJugador;
    [SerializeField] float fuerzaLanzamiento;
    [SerializeField] float chanfle;
    private Transform player;

    [Header("Quieto Onda")]
    [SerializeField] GameObject onda;
    [SerializeField] float fuerzaOnda;
    //[SerializeField] float tiempoVidaOnda;
    [SerializeField] Transform[] spawnOndas;
    [SerializeField] float esperaOndaTiempoInicial;
    [SerializeField] float esperaOndaTiempoFinal;
    [SerializeField] int ondasALanzar;
    [SerializeField] float tiemposEntreOndas;

    [Header("Temblores")]
    [SerializeField] GameObject escombroTecho;
    [SerializeField] Transform temblorA;
    [SerializeField] Transform temblorB;
    [SerializeField] int cantidaEscombrosCielo;

    [SerializeField] float esperaTemblorTiempoInicial;
    [SerializeField] float esperaTemblorTiempoFinal;
    [SerializeField] float tiemposEntreTrmbloresCielo;

    [Header("General")]
    [SerializeField] float velocidadEnemigo;
    [SerializeField] float cooldownEntreAtaques = 5;
    [SerializeField] float ccEntreAtaques;
    bool atacando;
    Transform target;
    bool embistiendo = false;
    [SerializeField]float tiempoQuieto;


    void Start()
    {


        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        target = puntoA; // Comienza moviéndose hacia el punto A
    }

    void Update()
    {
        if (cooldownEntreAtaques <= 0)
        {
            //rb.velocity = Vector2.zero;
            int ataqueProximo = Random.Range(0,6);
            atacando = true;


            Debug.Log(ataqueProximo);
            switch (ataqueProximo)
            {

                case 0:
                    Debug.Log("0");
                    //Embestida
                    //StartCoroutine(Embestir());
                    StartCoroutine(esperaAntesDeembestidas());
                    break;

                case 1:
                    Debug.Log("1");
                    rb.velocity = Vector2.zero;

                    ArrojarEscombro();


                    break;
                case 2:
                    Debug.Log("2");
                    rb.velocity = Vector2.zero;

                    ArrojarEscombro();


                    break;

                case 3:
                    rb.velocity = Vector2.zero;
                    Debug.Log("3");

                    //quietoOnda();
                    quietoOnda2();

                    break;

                case 4:
                    Debug.Log("4");

                    rb.velocity = Vector2.zero;

                    quietoOnda2();
                    break;


                case 5:
                    Debug.Log("5");

                    temblor();

                    break;

            }

            cooldownEntreAtaques = ccEntreAtaques; 

        }
        else if (!atacando)
        {

            cooldownEntreAtaques -= Time.deltaTime;
            //atacando = false;
        }
    }


    private void FixedUpdate()
    {
        if (embistiendo && embestidasRealizadas < embestidasMaximas)
        {

            Vector2 direction = (target.position - transform.position).normalized;
            Debug.Log("llego-1");

            if (Vector2.Distance(transform.position, target.position) < 1f &&!llego)
            {
                rb.velocity = Vector2.Lerp(Vector2.zero, direction * velocidadMaxima, Time.deltaTime);
                Debug.Log("llego");
                llego = true;


            }
            else if (llego)
            {

                if (rb.velocity.magnitude < 0.1f)
                {
                    Debug.Log("llego2");

                    CambiarObjetivo();
                    velocidadActual = 0;
                    embestidasRealizadas++;
                    if (embestidasRealizadas >= embestidasMaximas)
                    {
                        desembestir();

                    }
                    else
                    {
                        StartCoroutine(ReanudarEmbestida()); 

                    }
                }
            
                
            }
            else if (!llego)
            {
                Debug.Log("llego3");

                // Movimiento normal hacia el objetivo
                if (velocidadActual > velocidadMaxima)
                {
                    velocidadActual = velocidadMaxima;
                }
                else
                {
                    velocidadActual += aceleracion;//modificar valores para las etapas
                }
                rb.velocity = direction * velocidadActual;
            }
        }
        else if (!embistiendo && !atacando)
        {

            MoverHaciaJugadorLentamente();

            //velocidadDisminucion = velocidadEmbestida / 2;
            //Vector2 nuevaVelocidad = Vector2.MoveTowards(rb.velocity, Vector2.zero, velocidadDisminucion * Time.fixedDeltaTime);

            //rb.velocity = nuevaVelocidad;
        }

    }

    void MoverHaciaJugadorLentamente()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        direction.y = 0;


        // Movemos al enemigo hacia el jugador con una velocidad más lenta
        rb.velocity = direction * velocidadEnemigo;
    }

    IEnumerator ReanudarEmbestida()
    {
        embistiendo = false;
        yield return new WaitForSeconds(tiempoQuieto); 
        embistiendo = true; 
    }
    void embestir()
    {
        embistiendo = true;

    }



    void desembestir()
    {
        embistiendo = false;
        embestidasRealizadas = 0;
        atacando = false;
    }

    void CambiarObjetivo()
    {
        llego = false;

        //cambiar direccion
        if (target == puntoA)
        {
            target = puntoB;
        }
        else
        {
            target = puntoA;

        }
    }

    IEnumerator esperaAntesDeembestidas() {
        //rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(esperaAntesEmbestida);
        embestir();

    }

    //==========================================escombros===============================================

    #region escombros

    void ArrojarEscombro()
    {
        rb.velocity = Vector2.zero;
        atacando = true;
        StartCoroutine(esperaEscombroInicial());
        //cada uno es para que sea con chanfle
        LanzarEscombro();
        LanzarEscombro2();
        LanzarEscombro3();

        StartCoroutine(esperaEscombroFinals());

        
    }
    void LanzarEscombro()
    {


        GameObject escombroSpaneado = Instantiate(escombro, lugarSpawnEscombro.position, Quaternion.identity);
        Vector2 direccion = ((Vector2)player.position - (Vector2)transform.position).normalized;
        direccion.y += chanfle;

        Rigidbody2D rbEscombro = escombroSpaneado.GetComponent<Rigidbody2D>();
        rbEscombro.velocity = direccion * fuerzaLanzamiento;



     
    }
    void LanzarEscombro2()
    {


        GameObject escombroSpaneado2 = Instantiate(escombro, lugarSpawnEscombro.position, Quaternion.identity);
        Vector2 direccion2 = ((Vector2)player.position - (Vector2)transform.position).normalized;
        direccion2.y += (chanfle * 1.3f);
        //direccion2. += (chanfle *1.2f);


        Rigidbody2D rbEscombro2 = escombroSpaneado2.GetComponent<Rigidbody2D>();
        rbEscombro2.velocity = direccion2 * (fuerzaLanzamiento * .9f);



    
    }
    void LanzarEscombro3()
    {


        GameObject escombroSpaneado2 = Instantiate(escombro, lugarSpawnEscombro.position, Quaternion.identity);
        Vector2 direccion2 = ((Vector2)player.position - (Vector2)transform.position).normalized;
        direccion2.y += (chanfle * 1.6f);
        //direccion2. += (chanfle *1.2f);


        Rigidbody2D rbEscombro2 = escombroSpaneado2.GetComponent<Rigidbody2D>();
        rbEscombro2.velocity = direccion2 * (fuerzaLanzamiento * .95f);


    }

    IEnumerator esperaEscombroInicial()
    {

        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(esperaEscombroIncial);
   
    }
    IEnumerator esperaEscombroFinals()
    {

        yield return new WaitForSeconds(esperaEscombroFinal);
        atacando = false;
    }
    #endregion
    //=================================================================================================


    //=================================================================================================

    #region QUIETO ONDA


    void quietoOnda()
    {
        rb.velocity = Vector2.zero;
        atacando = true;
        StartCoroutine(esperaAntesOnda());
       
        StartCoroutine(esperaOnda());
    }

    void quietoOnda2()
    {
        rb.velocity = Vector2.zero;
        atacando = true;
        StartCoroutine(esperaAntesOnda2());

    }
    IEnumerator esperaAntesOnda()
    {
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(esperaOndaTiempoInicial);

        GameObject on1 = Instantiate(onda, spawnOndas[0].position, Quaternion.identity);
        GameObject ond2 = Instantiate(onda, spawnOndas[1].position, Quaternion.identity);

        Rigidbody2D rbOn1 = on1.GetComponent<Rigidbody2D>();
        Rigidbody2D rbOnd2 = ond2.GetComponent<Rigidbody2D>();

        rbOn1.velocity = new Vector2(fuerzaOnda, 0);
        rbOnd2.velocity = new Vector2(-fuerzaOnda, 0);
    }


    IEnumerator esperaAntesOnda2()
    {
        Debug.Log("Onda 2");
        yield return new WaitForSeconds(esperaOndaTiempoInicial);
        rb.velocity = Vector2.zero;

        int contador = 0; // Inicializamos un contador para llevar el control de las iteraciones

        while (contador < ondasALanzar) // Mientras el contador sea menor que la cantidad deseada de ondas a lanzar
        {
            GameObject on1 = Instantiate(onda, spawnOndas[0].position, Quaternion.identity);
            GameObject on2 = Instantiate(onda, spawnOndas[1].position, Quaternion.identity);

            Rigidbody2D rbOn1 = on1.GetComponent<Rigidbody2D>();
            Rigidbody2D rbOn2 = on2.GetComponent<Rigidbody2D>();

            rbOn1.velocity = new Vector2(fuerzaOnda, 0);
            rbOn2.velocity = new Vector2(-fuerzaOnda, 0);

            contador++; // Incrementamos el contador

            yield return new WaitForSeconds(tiemposEntreOndas); // Esperamos 2 segundos antes de continuar con la siguiente iteración
        }
        StartCoroutine(esperaOnda());

    }

    IEnumerator esperaOnda()
    {
        yield return new WaitForSeconds(esperaOndaTiempoFinal);
        atacando = false;
    }

    #endregion
    //=================================================================================================


    //=================================================================================================
    #region temblor

    void temblor() {

        atacando = true;
        rb.velocity = Vector2.zero;
        StartCoroutine(temblores());
    }

    IEnumerator temblores()
    {
        Debug.Log("Espera Temblor");
        yield return new WaitForSeconds(esperaTemblorTiempoInicial);
        rb.velocity = Vector2.zero;

        int contador = 0; // Inicializamos un contador para llevar el control de las iteraciones

        while (contador < cantidaEscombrosCielo) // Mientras el contador sea menor que la cantidad deseada de ondas a lanzar
        {
            Vector3 spawnPosition = Vector3.Lerp(temblorA.position, temblorB.position, Random.value);
            Instantiate(escombroTecho, spawnPosition, Quaternion.identity);
            contador++; // Incrementamos el contador

            yield return new WaitForSeconds(tiemposEntreTrmbloresCielo); // Esperamos 2 segundos antes de continuar con la siguiente iteración
        }

        StartCoroutine(esperaTemblores());

    }

    IEnumerator esperaTemblores()
    {
        yield return new WaitForSeconds(esperaTemblorTiempoFinal);
        atacando = false;
    }
    #endregion
    //=================================================================================================

}

