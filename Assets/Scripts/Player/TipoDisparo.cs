using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class TipoDisparo : MonoBehaviour
{
    [SerializeField] Transform transforPadre;
    [SerializeField] Rigidbody2D rbPlayer;
    [Header("Configurar Arma")]
    
    [SerializeField] float cooldownAtaques;
    [SerializeField] float danioAtaque;
    [SerializeField] private bool puedeAtacar = true;
    [Header("Rafaga?")]
    [SerializeField] bool rafagas;
    [SerializeField] int balasPorRafaga;
    [SerializeField] float tiempoEntreDisparo;
    private bool puedeDisparar = true;

    PlayerInput playerInput;
    PlayerStatsE playerStats;

    [SerializeField] GameObject ataque;
    [SerializeField] float costoDisparoMayo;
    [SerializeField] float retroceso;
    [SerializeField] float potenciaDeDisparo;
    [SerializeField] Transform[] origenAtaque;
    [SerializeField] Transform contArma;


    [Header("Si tiene impulso")]
    [SerializeField] bool impulso;
    [SerializeField] ImpulsoEscopeta escoepta;
    //Vector2 mousePos; //->    //INNECESARIO
    //[Header("Por Si es Melee")]

    //[SerializeField] float costoStamina;

    //public bool canReceiveInput;
    //public bool inputReceived;


    //============================================================= PROCESOS UNITY ===========================================
    #region PROCESOS UNITY

    private void Awake()
    {
    }
    void Start()
    {
        if (!rafagas) { balasPorRafaga = 0; tiempoEntreDisparo = 0; }
        if (impulso) { escoepta = GetComponent<ImpulsoEscopeta>(); }
        playerStats = GetComponentInParent<PlayerStatsE>();
        playerInput = GetComponentInParent<PlayerInput>();
        rbPlayer = GetComponentInParent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (this.enabled && playerStats.getEstadoVivo()) //si es necesario agregar que && PuedeMoverse
        {
            //detectarRecargaArma();
            detectarAtaque();
        }
    }

    #endregion
    //========================================================================================================================

    //====================================================== ATAQUE ==========================================================
    #region  ataque
    void detectarAtaque()
    {

        if (playerInput.actions["Atack"].IsPressed() && playerStats.getMayoActual() >= costoDisparoMayo)
        {
            Atacar();
        }

    }
    public void Atacar()
    {
        if (puedeAtacar)
        {
            puedeAtacar = false;
            //saber que tipo de disapro hace
            if (rafagas)
            {
                StartCoroutine(DispararRafaga());

            }
            else {
                DispararProyectiles();
            }
            //cargadorActual--;
            playerStats.restaMayo(costoDisparoMayo);
            Invoke("ActivarCooldownDisparo", cooldownAtaques);//COMO COORUTINA PERO SE SIGUE EJECUTNADO AUNQUE EL SCRIPT ESTE DESACTIVADO

        }
    }




    private void ActivarCooldownDisparo()
    {
        //yield return new WaitForSeconds(cooldownAtaques);
        puedeAtacar = true;
    }
    private void DispararProyectiles()
    {
        foreach (var origen in origenAtaque)
        {
            GameObject balaN = Instantiate(ataque, origen.position, origen.rotation);
            //HACER QUE CAMBIE DEL DAÑO SI TIENE MAYONESA DE OTROS TIPOS
            balaN.GetComponent<PlayerAttacks>().setDanioAtaque(danioAtaque);
            Rigidbody2D rbBalaN = balaN.GetComponent<Rigidbody2D>();

            // Calcular la dirección desde el objeto hacia el punto del mouse

            // Aplicar la fuerza en dirección contraria a esa dirección
            float anguloEnRadianes = contArma.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direccion = new Vector2(Mathf.Cos(anguloEnRadianes), Mathf.Sin(anguloEnRadianes));

            if (impulso) {
                escoepta.impusloEscopeta();
            }

            //retroceso Falta mejorarlo
            if (transforPadre.localScale.x > 0)
            {
                //rbPlayer.AddForce(-direccion * retroceso, ForceMode2D.Force);

                rbBalaN.AddForce(origen.right * potenciaDeDisparo);
            }
            else if (transforPadre.localScale.x < 0)
            {
                //rbPlayer.AddForce(direccion * retroceso, ForceMode2D.Force);   
                

                rbBalaN.AddForce(origen.right * -potenciaDeDisparo/*, ForceMode2D.Impulse*/);
            }
        }
    }

 
    IEnumerator DispararRafaga()
    {

        for (int i = 0; i < balasPorRafaga; i++) // Cambia el número 3 por la cantidad de disparos por ráfaga
        {
            DispararProyectiles();
            yield return new WaitForSeconds(tiempoEntreDisparo); // Intervalo entre balas de la ráfaga
        }

    }

    //public void detectarRecargaArma()
    //{

    //    if (/*tipoArma == TipoDeArma.Distancia && */cargadorActual < cargadorLleno)
    //    {

    //        if (playerInput.actions["Reload"].WasPressedThisFrame())
    //        {
    //            //activar Animacion para recargarar y llamar el metodo cuando se termine la animacion 
    //        }
    //    }
    //}


    #endregion
    //========================================================================================================================
}
