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


public class MovimientoP : MonoBehaviour
{
    public PlayerInput playerInput;
    PlayerSaltoE playerSalto;
    PlayerStatsE playerStats;
    PlayerDashE playerDash;
    [Header("Movimiento")]

    [SerializeField] float velocidadPlayer;
    [SerializeField] float velocidadPlayerApuntando;
    [SerializeField] float velocidadPlayerApuntandoRifle;
    [SerializeField] float velocidadPlayerSlowed;
    [SerializeField]float velocidadActual;
    [SerializeField] bool sePuedeMover = true;
    [SerializeField] bool slowed;

    [Header("Rebote")] 
    [SerializeField] Vector2 velocidadRebote;
    [SerializeField] float tiempoInvulnerable;

    Rigidbody2D rbPlayer;

    [SerializeField] bool PlayerVivo;
    [Header("Apuntado")]
    //[SerializeField] GameObject mira;
    [SerializeField] Transform contenedroCabez;
    [SerializeField] float limiteUp;
    [SerializeField] float limiteDown;
    Vector2 mousePos;
    bool rifleApuntando;
    bool apuntando = false;
    [SerializeField] float VelocidadRegresoCabeza;

    Armas armas;

    [Header("EQUIPABLES")]
    [SerializeField] bool botasEquiapdas;
    [SerializeField] bool botasMejoradas;
    

    //============================================= Start/Update ==============================================================
    #region Metodos (UNITY)


    void Start()
    {
        //recibir valore de dash, vida, velodaida, salto, etc. desde el los datos guardados
        //anim = GetComponent<Animator>();
        
        armas = GetComponent<Armas>();
        playerDash = GetComponent<PlayerDashE>();
        playerSalto=GetComponent<PlayerSaltoE>();
        playerStats=GetComponent<PlayerStatsE>();
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
        checarVelocidad();

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

        // Aplicar rotación al arma (cambiar si son arrojables o no /PENDIENTE/)

        //mandar rotaciones a las armas
        armas.cacharRotacion(direccion);

        if (apuntando)
        {

            flipX(direccion.x);

        }
        contenedroCabez.right = direccion * Mathf.Sign(transform.localScale.x);

        var eulerDir = contenedroCabez.localEulerAngles;

        // Colocar los límites de la cabeza
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

    public void recibirDano(float damage) {
        playerStats.playerDanado(damage);
    }
    public void Rebote(Vector2 knockbackDirection)
    {

        rbPlayer.velocity = new Vector2(-velocidadRebote.x * knockbackDirection.x, velocidadRebote.y);
        StartCoroutine(playerInvulnerable());
    }

    IEnumerator playerInvulnerable()
    {

        //Physics2D.IgnoreLayerCollision(8, 6, true);
        //Physics2D.IgnoreLayerCollision(8, 10, true);

        //yield return new WaitForSeconds(tiempoInvulnerable);

        //Physics2D.IgnoreLayerCollision(8, 6, false);
        ////Physics2D.IgnoreLayerCollision(8, 10, false);

        //bool originalCollisionState = Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        //bool originalCollisionState2 = Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("AtaquesEnemy"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("AtaquesEnemy"), true);
        yield return new WaitForSeconds(tiempoInvulnerable);
        // Restaurar el Layer del jugador para que pueda ser dañado nuevamente por los enemigos
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("AtaquesEnemy"), false);

    }
    #endregion
    //=========================================================================================================================


    //=================================================== Player "Velocidad" ==================================================
    #region player velocidades


    private void checarVelocidad()
    {
        if (slowed && !botasMejoradas)
        {
            velocidadActual = velocidadPlayerSlowed;
        }
        else if (slowed && botasMejoradas)
        {
            velocidadActual = velocidadPlayer;
        }
        else if (!slowed && rifleApuntando)
        {
            velocidadActual = velocidadPlayerApuntandoRifle;
        }
        else if (playerInput.actions["Aim"].IsPressed() && !slowed)
        {
            velocidadActual = velocidadPlayerApuntando;
        }
        else if (!slowed && !playerInput.actions["Aim"].IsPressed())
        {
            velocidadActual = velocidadPlayer;
        }
      
    }


    public void slowPlayer()
    {
        slowed = true;
    }
    public void NOslowPlayer()
    {
        slowed = false;
    }


    public void RegresarVelocidad() {
        velocidadActual = velocidadPlayer;
    }

    #endregion


    //=========================================================================================================================

    //============================================*colisiones==================================================================
    #region COLISIONES
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {

            Debug.Log("Player Dañado");//Restar vida en playerstats
        }
        else if (collision.transform.CompareTag("DamageAlPlayer")) {

            recibirDano(collision.transform.GetComponent<ObjetoDanino>().returnarDaño());
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

    public void cacharApuntadoRifle(bool ca)//si esta apuntado el rifle
    {
        rifleApuntando = ca;
    }
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



    //========================================= EQUIPABLES =============================================
    #region cachar Botas Equipadas
    //se andar a llamar con botones, o los equipables
    public void cacharBotas(bool botas)//se recibe desde un lado aqui
    {
        botasEquiapdas = botas;
        playerStats.cacharBotasMejoradas(botas);//caminar sin recibir daño
    }


    public void cacharBotasMejoradas(bool botas)
    {
        botasMejoradas = botas;
        playerStats.cacharBotasMejoradas(botas);//caminar sin recibir daño
        playerSalto.cacharBotasMejoradas(botas);//caminar y saltar con libertas
    }


    #endregion


    #region Dasheo
    public void EquiparDash(bool dashDes)
    {
        playerDash.cacharDashDesbloqueado(dashDes);

    }
    public void EquiparDashDanio(bool dashDanio)
    {
        playerDash.cacharDashDanioDesbloqueado(dashDanio);

    }

    #endregion


    //===================================================================================================
}


