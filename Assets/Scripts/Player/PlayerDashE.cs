using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Progress;

public class PlayerDashE : MonoBehaviour
{
    PlayerInput playerinput;
    PlayerStatsE playerStats;
    MovimientoP playerMovement;

    [Header("Dash")]
    [SerializeField] TrailRenderer trail;
    [SerializeField] bool dashDesbloqueado;
    [SerializeField] bool danioDashEquipado;
    [SerializeField] float dashCooldown; // Duración del dash
    [SerializeField] float dashVelocidad; // Duración del dash
    [SerializeField] float dashDuracion; // Duración del cooldown del dash
    [SerializeField] Collider2D colliderDanio;
    [SerializeField] Collider2D colliderPlayer;
    private bool canDash = true; // Bandera para controlar si el jugador está realizando un dash
    float gravedadNormal; //para que a la hora de dashear no tenga gravedad, este numero sirve para volverlo a como estaba
    bool puedeMoverse = true;
    [SerializeField]bool isdashing;
    Rigidbody2D rbplayer;
    [SerializeField] bool enPisoMalo;
    //[Header("Animations")]
    //Animator anim;
    private void Start()
    {
        //anim=GetComponent<Animator>();
       
        playerMovement = GetComponent<MovimientoP>();
        rbplayer = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStatsE>();
        playerinput = GetComponent<PlayerInput>();
        //colisionDanio=GetComponent<Collider2D>();
        gravedadNormal = rbplayer.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerStats.getVida() > 0 && dashDesbloqueado)
        {
            DetectarDash();
        }

    }



    void DetectarDash()
    {
        if (playerinput.actions["Dash"].IsPressed() &&canDash &&!enPisoMalo)
        {
            canDash = false;
            StartCoroutine(HacerDash());
            StartCoroutine(cooldownParaDash());
            playerStats.PlayerInvulnerable(dashDuracion);
        }


    }



    private IEnumerator HacerDash()
    {
        trail.emitting=true;
        //anim.SetBool("Dashing", true);
        isdashing = true;
        Debug.Log("dash");
        if (danioDashEquipado) { 
            colliderDanio.enabled = true;
        }
        puedeMoverse = false;
        rbplayer.gravityScale = 0;
        puedeMoversePlayerMove();//para restringir el movimiento en el scirp de MovimietnoP
        rbplayer.velocity = new Vector2(dashVelocidad * transform.localScale.x, 0);
        //colliderPlayer.enabled = false;

        yield return new WaitForSeconds(dashDuracion);
        //colliderPlayer.enabled = true;
        trail.emitting = false;

        puedeMoverse = true;
        rbplayer.gravityScale = gravedadNormal;
        if (danioDashEquipado)
        {
            colliderDanio.enabled = false;
        }
        isdashing = false;

        //anim.SetBool("Dashing", false);
        puedeMoversePlayerMove();//para regresar el movimiento en el scirp de MovimietnoP

    }

    private IEnumerator cooldownParaDash()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    //========================================================= PARA STATS ================================================================
    #region  STATS

    void puedeMoversePlayerMove()
    {
        playerMovement.SePuedeMover(puedeMoverse);
    }

    #endregion
    //=====================================================================================================================================

    //================================================= RETORNAR VALORES ==================================================================
    #region RETORNAR VALORES
 
    public float getVelocidadDash()
    {
        return dashVelocidad;
    }
    public bool isDashingReturn() {
        return isdashing;
    }
    #endregion
    //=====================================================================================================================================


    //================================================= SET VALORES ==================================================================
    //cargar datos
    #region SET VALORES 

    //public void SetVelocidadDash(float fuerzaDash)
    //{
    //    dashVelocidad = fuerzaDash;
    //}

    public void cacharDashDesbloqueado(bool DD)
    {
        dashDesbloqueado = DD;
    }
    public void cacharDashDanioDesbloqueado(bool DD)
    {
        danioDashEquipado = DD;
    }
    public void actualEnPisoMalo(bool enPiso) {
        enPisoMalo = enPiso;
    }
    #endregion
    //=====================================================================================================================================
}
