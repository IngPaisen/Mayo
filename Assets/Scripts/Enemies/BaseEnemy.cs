using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseEnemy : MonoBehaviour
{
    [Header("Stats Enemy")]
    [SerializeField] float danio;
    [SerializeField] float vidaEnemy;
    [Header("Seguimiento Player")]
    [SerializeField] Transform contenedroCabez;
    PlayerStatsE player;
    Transform playerTransform;
    [SerializeField] float VelocidadRegresoCabeza;
    [SerializeField] float limiteUp = 10;
    [SerializeField] float limiteDown = -15;
    bool playerVivo;
    Rigidbody2D rbenemy;
    // Start is called before the first frame update
    void Start()
    {
        rbenemy = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsE>();
        //playerTransform = GameObject.Find("CabezaPlayer").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        //playerVivo = player.getEstadoVivo();

        checarPlayer();
        MuerteEnemy();
    }



    //************************************************** Voltear Ver Al Player**********************************************************
    #region VerPlayer
    void checarPlayer()
    {
        if (playerVivo)
        {
            verAlPlayer();
        }
        else {
            if (contenedroCabez.localEulerAngles != Vector3.zero)
            {
                contenedroCabez.localRotation = Quaternion.Slerp(contenedroCabez.localRotation, Quaternion.identity, Time.deltaTime * VelocidadRegresoCabeza);
            }
        }

        //saber si esta apuntado

    }

    private void verAlPlayer()
    {
        //var direccion = ((Vector2)player.transform.position - (Vector2)contenedroCabez.position).normalized;
        var direccion = ((Vector2)playerTransform.position - (Vector2)contenedroCabez.position).normalized;

        // Aplicar rotación al arma

        //mandar rotaciones a las armas
        //pistola.cacharRotacion(direccion);

        flipX(direccion.x);

        contenedroCabez.right = direccion * MathF.Sign(transform.localScale.x);
        var eulerDir = contenedroCabez.localEulerAngles;
        //colcoar los limites de la cabeza
        eulerDir.z = Mathf.Clamp(
            eulerDir.z - (eulerDir.z > 180 ? 360 : 0),
            limiteDown,
            limiteUp);
        contenedroCabez.localEulerAngles = eulerDir;

    }



    private void flipX(float x)
    {

        if (x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(x), 1, 1);
        }
    }



    public float getDanio() {
        return danio;
    }
    #endregion

    //***********************************************************************r**********************************************************

    //********************************************************** Colisiones ************************************************************
    #region COLISIONES
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("PlayerAttack"))
        {
            vidaEnemy -= collision.transform.GetComponent<PlayerAttacks>().getDanioAtaque();
            collision.transform.GetComponent<PlayerAttacks>().DestruirBala();
            Debug.Log("Enemigo dañado");

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDash"))
        {
            vidaEnemy -= 2;
        }
    }


    public void danarEnemigo(float danio) {
        vidaEnemy -= danio;
    }


    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Mayotov")) {
    //        danarEnemigo(collision.GetComponent<ProgressiveDamage>().damagePerSeconds());
    //    }
    //}
    #endregion
    //**********************************************************************************************************************************


    //**************************************************** ACCIONES ENEMY **************************************************************

    #region ACCIONES    
    private void MuerteEnemy()
    {
        if (vidaEnemy <= 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    //**********************************************************************************************************************************


    public float getDanioPlayer() {
        return danio;
    }

}
