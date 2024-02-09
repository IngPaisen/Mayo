using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsporaArrojadizo : MonoBehaviour
{

    [SerializeField] float tiempoVidaObjeto =5;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] Collider2D colliderEspora;
    [SerializeField] ParticleSystem particulas;
    [SerializeField] Rigidbody2D rb;


    [Header("colisionado")]
    [SerializeField] bool playerDentro;
    [SerializeField] PlayerStatsE player;
    [SerializeField] float danioArea;
    private void Update() {

        dañarAreaPlayer();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        circleCollider.enabled = true;
        particulas.Play();
        rb.velocity = Vector2.zero;
        colliderEspora.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        StartCoroutine(vidaEspora());
    }

    IEnumerator vidaEspora() {
        yield return new WaitForSeconds(tiempoVidaObjeto);
        Destroy(gameObject);
    }


    void dañarAreaPlayer()
    {

        if (playerDentro)
        {
            player.GetComponent<PlayerStatsE>().enAreaDañina(danioArea);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player = collision.transform.GetComponent<PlayerStatsE>();
            playerDentro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerDentro = false;
        }
    }


}
