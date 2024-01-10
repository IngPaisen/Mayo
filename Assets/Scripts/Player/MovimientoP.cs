using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class MovimientoP : MonoBehaviour
{
    PlayerInput playerInput;
    [Header("Movimiento")]

    [SerializeField] float velocidadPlayer;
    [SerializeField] float velocidadPlayerApuntando;
    float velocidadActual;
    [SerializeField] bool sePuedeMover = true;
    [SerializeField] Vector2 velocidadRebote;
    Rigidbody2D rbPlayer;

    [SerializeField] bool PlayerVivo;

    [Header("Apuntado")]
    //[SerializeField] GameObject mira;
    [SerializeField] Transform contenedroCabez;
    [SerializeField] float limiteUp;
    [SerializeField] float limiteDown;
    Vector2 mousePos;

    bool apuntando = false;
    [SerializeField] float VelocidadRegresoCabeza;

    Armas armas;

    //============================================= Start/Update ==============================================================
    #region Metodos (UNITY)


    void Start()
    {
        //recibir valore de dash, vida, velodaida, salto, etc. desde el los datos guardados
        //anim = GetComponent<Animator>();
        armas = GetComponent<Armas>();
        playerInput = GetComponent<PlayerInput>();
        rbPlayer = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (PlayerVivo && sePuedeMover)
        {
            Move();
            //salto();
            checarApuntado();

        }
        else
        {

        }
    }

    private void FixedUpdate()
    {

    }
    private void LateUpdate()
    {
        if (apuntando) { verAlMouse(); }//ani
    }


    #endregion
    //=========================================================================================================================

    //================================================= Movimiento ============================================================
    #region Movimiento
    private void Move()
    {
        if (playerInput.actions["Aim"].IsPressed()) {
            velocidadActual = velocidadPlayerApuntando;
        }
        else
        {
            velocidadActual = velocidadPlayer;
        }
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        //anim.SetFloat("Movimiento", MathF.Abs(moveInput.x));
        if (!apuntando)
        {
            flipX(moveInput.x);
        }
        //rbPlayer.velocity = moveInput * velocidadPlayer;
        rbPlayer.velocity = new Vector2(moveInput.x * velocidadActual, rbPlayer.velocity.y);

    }

    #endregion
    //=========================================================================================================================

    //======================Apuntado, solo cabeza, las armas se voltean esn sus scripts correspodinetes========================
    #region Apuntar
    void checarApuntado()
    {
        //saber si esta apuntado
        if (playerInput.actions["Aim"].IsPressed())
        {
            apuntando = true;
            saberDondeEstaElMouse();
            verAlMouse();
        }
        else
        {
            apuntando = false;

            //Gradualmente vuelve a cero las rotaciones locales, no se si sea necesario quitar esto si tiene animacion la cabeza, pero creo que no, si es asi, se elimina y se coloca en el Late
            if (contenedroCabez.localEulerAngles !=/* Vector3.zero*/new Vector3(0, 0, 0))
            {
                Vector3 eulerAngle = new Vector3(0, 0, 0);
                Quaternion targetRotation = Quaternion.Euler(eulerAngle);
                contenedroCabez.localRotation = Quaternion.Slerp(contenedroCabez.localRotation, targetRotation, Time.deltaTime * VelocidadRegresoCabeza);
                //contenedroCabez.localRotation = Quaternion.Slerp(contenedroCabez.localRotation, quaternion.identity, Time.deltaTime * VelocidadRegresoCabeza);
            }


        }
    }


    private void saberDondeEstaElMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(playerInput.actions["Look"].ReadValue<Vector2>());
    }

    private void verAlMouse()
    {
        var direccion = (mousePos - (Vector2)contenedroCabez.position).normalized;

        // Aplicar rotaci�n al arma

        //mandar rotaciones a las armas
        armas.cacharRotacion(direccion);

        if (apuntando)
        {

            flipX(direccion.x);

        }
        contenedroCabez.right = direccion * Mathf.Sign(transform.localScale.x);

        var eulerDir = contenedroCabez.localEulerAngles;

        // Colocar los l�mites de la cabeza
        eulerDir.z = Mathf.Clamp(
            eulerDir.z - (eulerDir.z > 180 ? 360 : 0),
            limiteDown,
            limiteUp);

        //eulerDir.z += -90f; // Sumar 90 grados si es necesario

        contenedroCabez.localEulerAngles = eulerDir;


    }
    private void flipX(float x)
    {
        float umbral = 0.1f; // Establecer un umbral para evitar cambios constantes cerca del centro

        if (Mathf.Abs(x) > umbral)
        {
            transform.localScale = new Vector3(Mathf.Sign(x), 1, 1);
        }
    }
    #endregion
    //=========================================================================================================================

    //================================================== Player Golpeado ======================================================
    #region PLAYER GOLPEADO 
    public void Rebote(Vector2 puntoGolpe)
    {
        rbPlayer.velocity = new Vector2((-velocidadRebote.x * puntoGolpe.x), velocidadRebote.y);
    }
    #endregion
    //=========================================================================================================================


    //============================================*colisiones==================================================================
    #region COLISIONES
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {

            Debug.Log("Player Da�ado");
        }
    }
    #endregion
    //=========================================================================================================================


    //============================================* Retornar Datos ============================================================
    #region return datos
    public bool retornarApuntando()
    {
        return apuntando;
    }

 
    public bool getSePuedeMover()
    {
        return sePuedeMover;
    }

    #endregion
    //=========================================================================================================================

    //=============================================== Cachar Datos ============================================================
    #region CACHAR DATOS

  
    public void setEstadoPlayer(bool estado)
    {
        PlayerVivo = estado;
    }

    public void setVelocidadPlayer(float velocidadPlayer)
    {
        this.velocidadPlayer = velocidadPlayer;
    }

    public void SePuedeMover(bool puedeMoverse)
    {
        sePuedeMover = puedeMoverse;
    }

    #endregion      

    //=========================================================================================================================

}
