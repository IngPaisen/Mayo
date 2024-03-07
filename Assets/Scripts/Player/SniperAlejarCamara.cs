using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SniperAlejarCamara : MonoBehaviour
{
    [SerializeField] float camaraInicial;
    [SerializeField] float camaraSniper;
    [SerializeField] CinemachineVirtualCamera camaraVirtualCamera;
    [SerializeField] float velocidadTransicion;
    PlayerInput playerinput;
    MovimientoP movePlayer;
    private bool estabaPresionado = false;

    // Start is called before the first frame update

    private void Start()
    {
        movePlayer = GetComponentInParent<MovimientoP>();
        playerinput = GetComponentInParent<PlayerInput>();
    }
    private void Awake()
    {
        camaraVirtualCamera=FindAnyObjectByType<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        mantenerCamara();
    }
  

    public void mantenerCamara()
    {
        bool estaPresionado = playerinput.actions["Aim"].IsPressed();

        if (estaPresionado != estabaPresionado)
        {
            // Cambio de estado, se presinó o se soltó el botón "Aim" en este caso el clck dercho
            if (estaPresionado)
            {
                float tamañoOrtograficoActual = camaraVirtualCamera.m_Lens.OrthographicSize;
                float nuevoTamañoOrtografico = Mathf.Lerp(tamañoOrtograficoActual, camaraSniper, Time.deltaTime * velocidadTransicion);
                camaraVirtualCamera.m_Lens.OrthographicSize = nuevoTamañoOrtografico; movePlayer.cacharApuntadoRifle(true); // True indica que está pulsado
            }
            else
            {
                //acá ya no esta pulsado
                float tamañoOrtograficoActual = camaraVirtualCamera.m_Lens.OrthographicSize;
                float nuevoTamañoOrtografico = Mathf.Lerp(tamañoOrtograficoActual, camaraInicial, Time.deltaTime * velocidadTransicion);
                camaraVirtualCamera.m_Lens.OrthographicSize = nuevoTamañoOrtografico;
                movePlayer.cacharApuntadoRifle(false); // False indc que no está pulsao 
            }
        }
        else
        {
            float objetivoSize = estaPresionado ? camaraSniper : camaraInicial;
            float tamañoOrtograficoActual = camaraVirtualCamera.m_Lens.OrthographicSize;

            // Interpolar suavemente entre el tamaño ortogfico actual y el objetivo
            float nuevoTamañoOrtografico = Mathf.Lerp(tamañoOrtograficoActual, objetivoSize, Time.deltaTime * velocidadTransicion);

            // Asignar el nuevo tamaño ortográfico al compoente de la cámara
            camaraVirtualCamera.m_Lens.OrthographicSize = nuevoTamañoOrtografico;
        }

        estabaPresionado = estaPresionado; // Actualizar el estado anterior
    }



    private void OnDisable()
    {
        camaraVirtualCamera.m_Lens.OrthographicSize = camaraInicial;


    }
}
