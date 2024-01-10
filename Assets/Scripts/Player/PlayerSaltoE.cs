using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSaltoE : MonoBehaviour
{
    PlayerInput playerInput;
    MovimientoP moviemientoP;
    Rigidbody2D rbPlayer;
    //PlayerStatsE playerStats;
    PlayerDashE playerDash;
    [Header("Salto")]
    [SerializeField] float fuerzaSalto;
    [SerializeField] Transform refPie;
    [SerializeField] float radioDetectarPuedeSalto;
    [SerializeField] LayerMask QueEsPiso;
 
    [Range(0, 1)][SerializeField] float multiplicadorCancelarSalto;
    [SerializeField] float multiplicadorGravedad;
    bool saltar;
    bool enPiso;
    float escalaGravedad;
    bool botonSaltoArriba = true;
    //Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        //anim=GetComponent<Animator>();
        playerDash=GetComponent<PlayerDashE>();
        //playerStats = GetComponent<PlayerStats>();
        moviemientoP = GetComponent<MovimientoP>();
        playerInput = GetComponent<PlayerInput>();
        rbPlayer = GetComponent<Rigidbody2D>();
        escalaGravedad = rbPlayer.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (moviemientoP.getSePuedeMover())
        {
            detectarSalto();
            detectarCaesPlataforma();
        }

        //if (enPiso) {
        //    anim.SetBool("Jumping", false);
        //}

    }


    void detectarCaesPlataforma()
    {
        if (playerInput.actions["Down"].WasReleasedThisFrame())
        {

            Collider2D[] objetos = Physics2D.OverlapCircleAll(refPie.position, radioDetectarPuedeSalto, QueEsPiso);
            foreach (Collider2D item in objetos)
            {
                PlatformEffector2D platF = item.GetComponent<PlatformEffector2D>();
                if (platF != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>(), true);//->descativa la coliscion con esa plataforma

                    /*
                     SE COLCOA EN UN SCRIPT PARA LAS PLATAFORMASEN EL ONTRGIGEREXTI
                                                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>(), false);//->Activa la coliscion con esa plataforma

                        
                     */
                }
            }

        }
    }

    void detectarSalto()
    {

        if (playerInput.actions["Jump"].IsInProgress() )
        {
            saltar = true;

        };
        //if (playerInput.actions["Jump"].WasPressedThisFrame()) {

        //    anim.SetBool("Jumping", true);
        //}

        if (playerInput.actions["Jump"].WasReleasedThisFrame() ) { BotonSaltoSoltado(); };

        enPiso = Physics2D.OverlapCircle(refPie.position, radioDetectarPuedeSalto, QueEsPiso);
        //anim.SetBool("EnSuelo", enPiso);

        //animacion si esta en suelo
    }


    private void FixedUpdate()
    {
        if (saltar && botonSaltoArriba && enPiso)
        {
            Saltar();
        }

        if (rbPlayer.velocity.y < 0 && !enPiso)
        {
            rbPlayer.gravityScale = escalaGravedad * multiplicadorGravedad;
        } else if (playerDash.isDashingReturn()) 
        {
            rbPlayer.gravityScale = 0;
        }
        else
        {
            rbPlayer.gravityScale = escalaGravedad;
        }
        saltar = false;
    }

    public void Saltar()
    {
        rbPlayer.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

        enPiso = false;
        saltar = false;
        botonSaltoArriba = false;
    }


    void BotonSaltoSoltado()
    {
        if (rbPlayer.velocity.y > 0 && !playerDash.isDashingReturn())
        {
            rbPlayer.AddForce(Vector2.down * rbPlayer.velocity.y * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);

        }
        botonSaltoArriba = true;
        saltar = false;
    }


    private void OnDrawGizmosSelected()
    {
        // Establecer el color del gizmo
        Gizmos.color = Color.red;

        // Dibujar el círculo en la posición del objeto y con el radio especificado
        Gizmos.DrawWireSphere(refPie.position, radioDetectarPuedeSalto);
    }
}
