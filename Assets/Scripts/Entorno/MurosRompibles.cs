using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurosRompibles : MonoBehaviour
{
    [SerializeField] GameObject psASpawnearHumo;
    [SerializeField] GameObject psASpawnearFragmentos;


    public void destruirMuro() {
        GameObject particles = Instantiate(psASpawnearHumo, transform.position, Quaternion.identity);
        GameObject frag = Instantiate(psASpawnearFragmentos, transform.position, Quaternion.identity);
        Destroy(particles, particles.GetComponent<ParticleSystem>().main.duration);
        Destroy(frag, frag.GetComponent<ParticleSystem>().main.duration);

        Destroy(gameObject);

    }
}
