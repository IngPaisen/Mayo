using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks.Sources;
using UnityEngine;

public class PlayerStatsE : MonoBehaviour
{
    /*                       ESTE SCRIPT CONTROLA BASICAMENTE QUE EL JUGADOR PUEDA MOVERSE (ESTE VIVO), SU SALUD, SU municion                       */
    MovimientoP playerMove;
    [Header("Vida")]
    [SerializeField] bool botasEquipadas;

    [SerializeField] float vidaActual;
    [SerializeField] float vidaTotal;
    [SerializeField] bool playerVivo = true;
    [SerializeField] float mayonesaActual;
    //[Header("Daño Recibido")]
    bool isInvulnerable;


    // Start is called before the first frame update
    private void Awake()
    {
        playerMove = GetComponent<MovimientoP>();
        MandarDatosPlayerMovemnet();
    }


    void Start()
    {
        if (vidaActual > 0) playerVivo = true; else playerVivo = false;
    }

    // Update is called once per frame
    void Update()
    {
        chequeoVida();

        if (playerVivo)
        {
            MandarDatosPlayerMovemnet();
        }

    }

    //=============================================== Checar Vida ===============================================

    private void chequeoVida()
    {
        if (vidaActual <= 0)
        {
            playerVivo = false;
        }
        else if (vidaActual > vidaTotal)
        {
            vidaActual = vidaTotal;
        }
        else
        {
            playerVivo = true;
        }
    }

    //==============================================================================================================


    public void PlayerInvulnerable(float tiempo)
    {
        StartCoroutine(EnableInvulnerability(tiempo));

    }
    IEnumerator EnableInvulnerability(float tiempoInvulnerable)
    {
        isInvulnerable = true;
        bool originalCollisionState = Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
        bool originalCollisionState2 = Physics2D.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("AtaquesEnemy"));
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("AtaquesEnemy"), true);
        yield return new WaitForSeconds(tiempoInvulnerable);
        // Restaurar el Layer del jugador para que pueda ser dañado nuevamente por los enemigos
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), originalCollisionState);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("AtaquesEnemy"), originalCollisionState2);
        isInvulnerable = false;
    }

    //void perderControlPlayer()
    //{
    //    //si al recibir daño no se puede mover
    //}
    //public void revivir()
    //{
    //    vidaActual = vidaTotal;
    //    playerVivo = true;
    //}

    //==================================================== Retornar Datos ====================================================
    #region return Datos
    public float getVida()
    {
        return vidaActual;
    }
    public float getVidaTotal()
    {
        return vidaTotal;
    }
    public bool getEstadoVivo()
    {
        return playerVivo;
    }

    public float getMayoActual()
    {
        return mayonesaActual;
    }

    #endregion
    //=====================================================================================================================


    //==================================================== Mandar Datos ====================================================
    #region para que se pueda Mover
    private void MandarDatosPlayerMovemnet()
    {
        playerMove.setEstadoPlayer(playerVivo);
    }
    #endregion
    //=====================================================================================================================

    //================================================ Modificar Valores ====================================================

    #region MODIFICAR VALORES

    public void playerDanado(float danioRecibido)
    {

        vidaActual -= danioRecibido;
    }
    public void RestarVidaRebote(float danioRecibido, Vector2 posicion)//falta ver si si o si no
    {
        //vidaActual -= danioRecibido;
        ////anoiacion Golpe
        //playerMove.Rebote(posicion);
        //perderControlPlayer();

    }


    public void enPisoDanino(float danioParaPlayer) {
        if (!botasEquipadas) { 
            vidaActual -= Time.deltaTime * danioParaPlayer;

        }
    }

    //daño si esta cerca de algun enemigoq ue emane algo malo
    public void enAreaDañina(float danioParaPlayer)
    {
    
        vidaActual -= Time.deltaTime * danioParaPlayer;

        
    }


    public void restaMayo(float costoDisparo)
    {
        mayonesaActual -= costoDisparo;
    }

    #endregion
    //=====================================================================================================================



    //===================================================================================================
    #region cachar Botas Equipadas

    public void cacharBotasMejoradas(bool botas)
    {
        botasEquipadas = botas;
    }



    #endregion
    //===================================================================================================

}
