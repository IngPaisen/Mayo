using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Toro : EnemigoBase
{

    [Header("Embestidas")]
    //[SerializeField] float radioEmbestida;
    [SerializeField] float velocidadEmbestida;
    [SerializeField] float velocidadActual;    
    [SerializeField] TrailRenderer trailEmbestida;
    [SerializeField] float tiempoQuieto;
    [SerializeField] Transform puntoA;
    [SerializeField] Transform puntoB;
    public int embestidasMaximas = 3;
    [SerializeField] private int embestidasRealizadas = 0;
    [SerializeField] Transform target; // Punto hacia el que se está moviendo actualmente
    [SerializeField] bool embistiendo;
    //[SerializeField] float esperaEntreEmbestida = 2;
    [SerializeField] float velocidadDisminucion = 1;
    [SerializeField] float[] multiplicadorReduccionVelocidadTerminarEmbestida = new float[3];
    [SerializeField] bool llego;

    [Header("Escombro")]
    [SerializeField] GameObject escombro;
    [SerializeField] Transform lugarSpawnEscombro;
    [SerializeField] float tiempoVidaEscombro;
    [SerializeField] float tiempoAtaqueEscombros;
    [SerializeField] Vector2 posicionJugador;
    [SerializeField] float fuerzaLanzamiento;
    [SerializeField] float chanfle;
    [Header("General")]
    [SerializeField] float velocidadenemigo;
    [SerializeField] float cooldownEntreAtaques;
    private Transform player;
    [SerializeField] bool atacando;
    Rigidbody2D rb;

        // Start is called before the first frame update
    void Start()
    {

        Transform transformJugador = GameObject.FindGameObjectWithTag("Player").transform;
        posicionJugador = new Vector2(transformJugador.position.x, transformJugador.position.y);

        target = puntoA; // Empieza moviéndose hacia el punto A

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        trailEmbestida = GetComponent<TrailRenderer>();
        trailEmbestida.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownEntreAtaques <= 0)
        {
            int ataqueProximo = Random.Range(0, 2);
            atacando = true;


            Debug.Log(ataqueProximo);
            switch (ataqueProximo)
            {

                case 0:
                    //Embestida
                    ArrojarEscombro();

                    //embestir();
                    //StartCoroutine(Embestir());
                    break;

                case 1:
                    //activar anuimacion para lazanr la espora
                    ArrojarEscombro();

                    break;
            }

            cooldownEntreAtaques = Random.Range(4f, 5.5f); // reiniciar el cooldown entre ataques

        }
        else if (!atacando)
        {

            cooldownEntreAtaques -= Time.deltaTime; // reinicar el cooldown
            //atacando = false;
        }
    }

    private void FixedUpdate()
    {

        //if (embistiendo && embestidasRealizadas < embestidasMaximas)
        //{
        //    Vector2 direction = (target.position - transform.position).normalized;

        //    rb.velocity = direction * velocidadEmbestida;

        //    // Si el boss se ha acercado lo suficiente al trget se cambiar el objetivo
        //    if (Vector2.Distance(transform.position, target.position) < 1f)
        //    {
        //        //StartCoroutine(aguatalaEmebetida());

        //        Debug.Log("Si cambio");
        //        CambiarObjetivo();
        //        embestidasRealizadas++;
        //        if (embestidasRealizadas >= embestidasMaximas)
        //        {
        //            desembestir();

        //        }
        //    }
        //}
        if (embistiendo && embestidasRealizadas < embestidasMaximas)
        {

            Vector2 direction = (target.position - transform.position).normalized;
            Debug.Log("llego-1");

            if (Vector2.Distance(transform.position, target.position) < 1f&&!llego)
            {
                // Detener gradualmente el movimiento
                //rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * multiplicadorReduccionVelocidadTerminarEmbestida[0]);
                Debug.Log("llego");
                llego = true;
                velocidadActual = 0;

            }
            else if (llego)
            {

                if (rb.velocity.magnitude < 0.1f)
                {
                    Debug.Log("llego2");

                    CambiarObjetivo(); // Cambiar el objetivo al siguiente punto

                    embestidasRealizadas++;
                    if (embestidasRealizadas >= embestidasMaximas)
                    {
                        desembestir();

                    }
                    else
                    {
                        StartCoroutine(ReanudarEmbestida()); // Reanudar la embestida después de un breve retraso

                    }
                }
            }
            else if (!llego)
            {
                Debug.Log("llego3");

                // Movimiento normal hacia el objetivo
                if (velocidadActual > velocidadEmbestida) {
                    velocidadActual = velocidadEmbestida;
                }
                else
                {
                    velocidadActual += .15f;//modificar valores para las etapas
                }
                rb.velocity = direction * velocidadActual;
            }
        }
        else if (!embistiendo && rb.velocity.magnitude > 0)
        {

            Debug.Log("no");

            velocidadDisminucion = velocidadEmbestida / 2;
            Vector2 nuevaVelocidad = Vector2.MoveTowards(rb.velocity, Vector2.zero, velocidadDisminucion * Time.fixedDeltaTime);

            rb.velocity = nuevaVelocidad;
        }


    }

    //======================================= EMBESTIDA ==================================================
    #region EMBESTIDA?
    IEnumerator ReanudarEmbestida()
    {
        yield return new WaitForSeconds(tiempoQuieto); // Esperar un tiempo antes de reanudar la embestida, creo qu eno funciona 
        embistiendo = true; // Reanudar la embestida
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
    #endregion
    //=================================================================================================

    //==========================================escombros===============================================

    #region escombros

    void ArrojarEscombro()
    {
        atacando = true;
        //cada uno es para que sea con chanfle
        StartCoroutine(LanzarEscombro());
        StartCoroutine(LanzarEscombro2());
        StartCoroutine(LanzarEscombro3());
    }
    IEnumerator LanzarEscombro()
    {


        GameObject escombroSpaneado = Instantiate(escombro, lugarSpawnEscombro.position, Quaternion.identity);
        Vector2 direccion = ((Vector2)player.position - (Vector2)transform.position).normalized;
        direccion.y += chanfle;

        Rigidbody2D rbEscombro = escombroSpaneado.GetComponent<Rigidbody2D>();
        rbEscombro.velocity = direccion * fuerzaLanzamiento;



        yield return new WaitForSeconds(tiempoAtaqueEscombros);

        atacando = false;
    }
    IEnumerator LanzarEscombro2()
    {


        GameObject escombroSpaneado2 = Instantiate(escombro, lugarSpawnEscombro.position, Quaternion.identity);
        Vector2 direccion2 = ((Vector2)player.position - (Vector2)transform.position).normalized;
        direccion2.y += (chanfle * 1.3f);
        //direccion2. += (chanfle *1.2f);


        Rigidbody2D rbEscombro2 = escombroSpaneado2.GetComponent<Rigidbody2D>();
        rbEscombro2.velocity = direccion2 * (fuerzaLanzamiento * .9f);



        yield return new WaitForSeconds(tiempoAtaqueEscombros);

        atacando = false;
    }
    IEnumerator LanzarEscombro3()
    {


        GameObject escombroSpaneado2 = Instantiate(escombro, lugarSpawnEscombro.position, Quaternion.identity);
        Vector2 direccion2 = ((Vector2)player.position - (Vector2)transform.position).normalized;
        direccion2.y += (chanfle * 1.6f);
        //direccion2. += (chanfle *1.2f);


        Rigidbody2D rbEscombro2 = escombroSpaneado2.GetComponent<Rigidbody2D>();
        rbEscombro2.velocity = direccion2 * (fuerzaLanzamiento * .95f);



        yield return new WaitForSeconds(tiempoAtaqueEscombros);

        atacando = false;
    }

    #endregion
    //=================================================================================================

}


