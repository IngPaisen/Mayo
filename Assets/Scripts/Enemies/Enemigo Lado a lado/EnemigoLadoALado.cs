using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemigoLadoALado : EnemigoBase
{
    [Header("Lado a Lado")]
    //[SerializeField] Transform[] posiciones;
    [SerializeField] Transform controladorSuelo;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float tiempoEspera;
    [SerializeField] float velocidad;
    [SerializeField] float distancia;
    [SerializeField] private bool moviendoDer;
    [SerializeField] private bool caminando;

    void Start()
    {
                rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (caminando) {
            RaycastHit2D imfoSuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down, distancia);//crea la lina y choca con algo, si deja de haber se voltea
            rb.velocity = new Vector2(velocidad, rb.velocity.y);
            if (imfoSuelo == false)
            {
                //gira el persoanje
                Girar();
            }
        }
    }

    void Girar() {
        caminando = false;
        StartCoroutine(esperaLado());
      
    }

    IEnumerator esperaLado() {
        yield return new WaitForSeconds(tiempoEspera);
        caminando = true;

        moviendoDer = !moviendoDer;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        velocidad *= -1;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(controladorSuelo.transform.position, controladorSuelo.transform.position + Vector3.down * distancia);
    }

}
