using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class granada : MonoBehaviour
{
    [SerializeField] float radioExplosion;
    [SerializeField] float tiempoExplosion;
    [SerializeField] GameObject particulasDropHumo;//pendiente
    [SerializeField] GameObject particulasDropFragmentos;//pendiente
    [SerializeField] GameObject particulasDropHumoGrande;//pendiente
    void Start()
    {
        Invoke("explosion", tiempoExplosion);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void explosion() {
        Explode();


        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radioExplosion);

        // Iterar a trav�s de los objetos que colisionan con el �rea de explosi�n
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy")) {
                GameObject objeto = collider.gameObject;


                Destroy(objeto);
            }

            if (collider.CompareTag("BrakeableWall")) {
                collider.GetComponent<MurosRompibles>().destruirMuro();   
            }
            // Aqu� puedes realizar acciones con cada objeto colisionado dentro del radio de explosi�n
            // Por ejemplo: destruir el objeto, aplicar fuerzas, etc.
            // Ejemplo: objeto.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse);
        }
        //gameObject.SetActive(false);//-> para hacer el objet pool
        //Destroy(gameObject);
        Destroy(gameObject);

    }


    void Explode()
    {
        // Crea el efecto de part�culas
        GameObject particles = Instantiate(particulasDropHumo, transform.position, Quaternion.identity);
        GameObject box = Instantiate(particulasDropFragmentos, transform.position, Quaternion.identity);
        GameObject humos = Instantiate(particulasDropHumoGrande, transform.position, Quaternion.identity);
        //box.transform.parent = particles.transform;
        //humos.transform.parent = particles.transform;
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        Destroy(particles, ps.main.duration); 
        Destroy(box, ps.main.duration); 
        Destroy(humos, ps.main.duration); 


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
