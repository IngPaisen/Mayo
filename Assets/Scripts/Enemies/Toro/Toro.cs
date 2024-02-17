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
    [SerializeField] TrailRenderer trailEmbestida;
    [SerializeField] float tiempoQuieto;
    [SerializeField] Transform puntoA;
    [SerializeField] Transform puntoB;
    public int embestidasMaximas = 3;
    [SerializeField] private int embestidasRealizadas = 0;
    private Transform target; // Punto hacia el que se está moviendo actualmente
    [SerializeField] bool embistiendo;
    [SerializeField] float esperaEntreEmbestida = 2;

    [Header("General")]
    [SerializeField] float velocidadenemigo;
    [SerializeField] float cooldownEntreAtaques;
    private Transform player;
    [SerializeField] bool atacando;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
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
            int ataqueProximo = Random.Range(0, 1);
            atacando = true;


            Debug.Log(ataqueProximo);
            switch (ataqueProximo)
            {

                case 0:
                    //Embestida
                    embestir();
                    //StartCoroutine(Embestir());
                    break;

                case 1:
                    //activar anuimacion para lazanr la espora


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

        if (embistiendo && embestidasRealizadas < embestidasMaximas)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            rb.velocity = direction * velocidadEmbestida;

            // Si el boss se ha acercado lo suficiente al trget se cambiar el objetivo
            if (Vector2.Distance(transform.position, target.position) < 1f)
            {
                //StartCoroutine(aguatalaEmebetida());

                Debug.Log("Si cambio");
                CambiarObjetivo();
                embestidasRealizadas++;
                if (embestidasRealizadas >= embestidasMaximas)
                {
                    desembestir();

                }
            }
        }
        else if (!embistiendo)
        {

        }


    }

    IEnumerator aguatalaEmebetida()
    {
        yield return new WaitForSeconds(esperaEntreEmbestida);

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
}
