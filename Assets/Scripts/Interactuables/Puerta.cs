//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Puerta : MonoBehaviour
//{
//    [SerializeField] bool abierta = false;
//    [SerializeField] bool playerDentro;
//    [SerializeField] float cdEntreInteraccion;
//    [SerializeField] bool puedeInteactuar;
//    [SerializeField] BoxCollider2D bc;
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (playerDentro && puedeInteactuar)
//        {
//            if (Input.GetKeyDown(KeyCode.E))
//            {
//                TogglePuerta(); // Llama a la función para abrir/cerrar la puerta
//            }
//        }
//    }



//    void TogglePuerta()
//    {
//        abierta = !abierta;
//        // Aquí deberia ir la animacion... SI TUVIERA UNA
//        if (bc != null)
//        {
//            bc.enabled = !abierta;
//        }

//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (collision.transform.CompareTag("Player")) {
//            playerDentro = true;
//        }
//    }
//}

using UnityEngine;
using UnityEngine.InputSystem;

public class Puerta : MonoBehaviour
{
    [SerializeField] MovimientoP player;
    [SerializeField] bool abierta = false;
    [SerializeField] bool playerDentro;
    [SerializeField] float cdEntreInteraccion = 0.5f; // Tiempo de espera entre interacciones
    [SerializeField] bool puedeInteractuar = true;
    [SerializeField] BoxCollider2D bc;

    private float tiempoUltimaInteraccion;

    // Update is called once per frame
    void Update()
    {
        if (playerDentro && puedeInteractuar)
        {
            if (player.playerInput.actions["Interactuar"].WasPerformedThisFrame())
            {
                TogglePuerta(); // Llama a la función para abrir/cerrar la puerta
            }
        }

        if (Time.time - tiempoUltimaInteraccion > cdEntreInteraccion)
        {
            puedeInteractuar = true;
        }
    }

    void TogglePuerta()
    {
        abierta = !abierta;
        // Aquí debería ir la animación... SI TUVIERA UNA
        if (bc != null)
        {
            bc.enabled = !abierta;
        }

        // Establecer el tiempo de la última interacción y desactivar la capacidad de interactuar durante el tiempo de espera, esto es para evitar que el player abra y cierre por accidente
        tiempoUltimaInteraccion = Time.time;
        puedeInteractuar = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<MovimientoP>();
            playerDentro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDentro = false;
        }
    }
}

