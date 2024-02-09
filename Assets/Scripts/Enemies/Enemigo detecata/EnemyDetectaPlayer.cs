using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetectaPlayer : MonoBehaviour
{
    [SerializeField] Transform puntoOrigenDetector;
    [SerializeField] float radioDeteccion;
    [SerializeField] LayerMask layerplauer; 
    [SerializeField] Transform player; 
    [SerializeField] float velocidad;
    [SerializeField] bool detectado;
    Rigidbody2D rigidbody2;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        detectado = Physics2D.OverlapCircle(puntoOrigenDetector.position, radioDeteccion, layerplauer);


    }
    private void FixedUpdate()
    {
        if (detectado)
        {
            // Encuentra la posición del jugador
            player = GameObject.FindGameObjectWithTag("Player").transform;

            // Calcula la dirección hacia el jugador
            Vector2 direccion = (player.position - transform.position).normalized;
        
            //rigidbody2.velocity = direccion * velocidad;
            rigidbody2.velocity = new Vector2(direccion.x * velocidad, rigidbody2.velocity.y);

            if (direccion.x > 0)
            {   
                transform.localScale = new Vector3(1, 1, 1);
            }
            else { 
                transform.localScale = new Vector3(-1,1,1);

            }
        }
        else
        {
            // Si el jugador no está detectado, la velocidad se establece en cero
            rigidbody2.velocity = Vector2.zero;
        }
    }
    // Update is called once per frame
  


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(puntoOrigenDetector.transform.position, radioDeteccion);

    }

}
