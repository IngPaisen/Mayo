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
            // Cambio de estado, se presin� o se solt� el bot�n "Aim" en este caso el clck dercho
            if (estaPresionado)
            {
                float tama�oOrtograficoActual = camaraVirtualCamera.m_Lens.OrthographicSize;
                float nuevoTama�oOrtografico = Mathf.Lerp(tama�oOrtograficoActual, camaraSniper, Time.deltaTime * velocidadTransicion);
                camaraVirtualCamera.m_Lens.OrthographicSize = nuevoTama�oOrtografico; movePlayer.cacharApuntadoRifle(true); // True indica que est� pulsado
            }
            else
            {
                //ac� ya no esta pulsado
                float tama�oOrtograficoActual = camaraVirtualCamera.m_Lens.OrthographicSize;
                float nuevoTama�oOrtografico = Mathf.Lerp(tama�oOrtograficoActual, camaraInicial, Time.deltaTime * velocidadTransicion);
                camaraVirtualCamera.m_Lens.OrthographicSize = nuevoTama�oOrtografico;
                movePlayer.cacharApuntadoRifle(false); // False indc que no est� pulsao 
            }
        }
        else
        {
            float objetivoSize = estaPresionado ? camaraSniper : camaraInicial;
            float tama�oOrtograficoActual = camaraVirtualCamera.m_Lens.OrthographicSize;

            // Interpolar suavemente entre el tama�o ortogfico actual y el objetivo
            float nuevoTama�oOrtografico = Mathf.Lerp(tama�oOrtograficoActual, objetivoSize, Time.deltaTime * velocidadTransicion);

            // Asignar el nuevo tama�o ortogr�fico al compoente de la c�mara
            camaraVirtualCamera.m_Lens.OrthographicSize = nuevoTama�oOrtografico;
        }

        estabaPresionado = estaPresionado; // Actualizar el estado anterior
    }



    private void OnDisable()
    {
        camaraVirtualCamera.m_Lens.OrthographicSize = camaraInicial;


    }
}
