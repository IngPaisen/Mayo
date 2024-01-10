using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulasMayotov : MonoBehaviour
{
    private ParticleSystem particleSystem;
    [SerializeField]
    [Range(0, 100)]
    private int cantidadPsPorArrojable;
    private int emittedParticles = 0;
    private bool isEmitting = false;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem == null)
        {
            Debug.LogError("No se encontró el componente ParticleSystem.");
        }
        else {
            StartEmission();
        }

    }

    void Update()
    {
        if (!isEmitting && emittedParticles < 100)
        {
            particleSystem.Emit(1); // Emite una partícula
            emittedParticles++;
        }
        else /*mittedParticles >= 100*/ {
            StopEmission();
            //particleSystem.enableEmission = false;
        }
    }

    public void StartEmission()
    {
        if (particleSystem != null)
        {
            isEmitting = true;
            particleSystem.Play();
        }
    }

    public void StopEmission()
    {
        if (particleSystem != null)
        {
            isEmitting = false;
            particleSystem.Stop();
        }
    }
}
