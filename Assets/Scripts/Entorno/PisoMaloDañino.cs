using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisoMaloDañino : MonoBehaviour
{
    [SerializeField] float danoPlayerEnPiso;
    [SerializeField] bool playerPisando;
    [SerializeField]MovimientoP playerMove;
    [SerializeField]PlayerStatsE playerStats;
    [SerializeField] PlayerSaltoE playerSalto;
    [SerializeField] PlayerDashE playerDash;

    private void Start()
    {
        playerDash = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerDashE>();
    }
    private void Update()
    {
        if (playerPisando) {
            playerMove.slowPlayer();
            playerStats.enPisoDanino(danoPlayerEnPiso);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")) {
            playerMove = collision.GetComponent<MovimientoP>();
            playerSalto= collision.GetComponent<PlayerSaltoE>();
            playerStats= collision.GetComponent<PlayerStatsE>();
            playerSalto.estadoPlayerAtascado(true);
            playerDash.actualEnPisoMalo(true);
            playerPisando = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")) {
            playerPisando = false;
            //playerMove.RegresarVelocidad();
            playerSalto.estadoPlayerAtascado(false);
            playerDash.actualEnPisoMalo(false);

            playerMove.NOslowPlayer();
        }
    }
}
