using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mayotov : MonoBehaviour
{
    public GameObject explosionParticles; // Asigna aqu� tu efecto de part�culas para la explosi�n
    public GameObject boxCollider; // Asigna aqu� tu efecto de part�culas para la explosi�n
    public GameObject humo; // Asigna aqu� tu efecto de part�culas para la explosi�n
    public float explosionRadius = 3f;
    public float damagePerSecond = 5f;
    //public float multiplicadorDa�o;
    bool exploded = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!exploded)
        {
            exploded = true;
            Explode();
        }
    }

    void Explode()
    {
        // Crea el efecto de part�culas
        GameObject particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        GameObject box = Instantiate(boxCollider, transform.position, Quaternion.identity);
        GameObject humos = Instantiate(humo, transform.position, Quaternion.identity);
        //box.transform.parent = particles.transform;
        //humos.transform.parent = particles.transform;
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        Destroy(particles, ps.main.duration + 1f); // Destruye el efecto de part�culas despu�s de su duraci�n +  1 segundo parq qu eno desaparezca de golpe
        Destroy(box, ps.main.duration); // Destruye el efecto de part�culas despu�s de su duraci�n +  1 segundo parq qu eno desaparezca de golpe
        Destroy(humos, ps.main.duration + 1f); // Destruye el efecto de part�culas despu�s de su duraci�n +  1 segundo parq qu eno desaparezca de golpe

        Destroy(gameObject);

    }
}




