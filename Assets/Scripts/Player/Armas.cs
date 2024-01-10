using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Armas : MonoBehaviour
{
    Vector2 direccion;
    PlayerStatsE playerStats;
    PlayerInput playerInput;
    [SerializeField] GameObject[] armas; // ->todas las armas del juego
    [SerializeField] float VelocidadRegresoPos;
    [SerializeField] int armaEquipada;


    //[SerializeField]Transform puntoDeApoyoMano;
    [SerializeField] float limiteDown;
    [SerializeField] float limiteUp;

    bool recargando;

    void Start()
    {
        //armas = GameObject.FindGameObjectsWithTag("ArmasPlayer"); // Buscar todos los objetos con la etiqueta "ArmasPlayer"
        //armasEquipadasR  y queArmaTieneEquipada SE SACA DE LO QUE TENGA GUARDADO
        playerInput = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStatsE>();


    }

    // Update is called once per frame
    void Update()
    {

        if (playerStats.getEstadoVivo())
        {
            if (armas != null) // Verificar si hay armas
            {
                checarApuntado();

            }
            else
            {
                Debug.LogError("El objeto 'arma' no está asignado en el inspector!");
            }
            CambioDeArmas();
        }
    }

    //*****************************************************************************apuntado**********************************************************************************
    #region  apuntado

    void checarApuntado()
    {
        //saber si esta apuntado
        if (playerInput.actions["Aim"].IsPressed())
        {
            verAlMouse();

        }
        else if (!playerInput.actions["Aim"].IsPressed())
        {
            // Gradualmente vuelve a cero las rotaciones locales, no se si sea necesario quitar esto si tiene animacion la cabeza, pero creo que no, si es asi, se elimina y se coloca en el Late

            armas[armaEquipada].transform.localRotation = Quaternion.Slerp(armas[armaEquipada].transform.localRotation, Quaternion.identity, Time.deltaTime * VelocidadRegresoPos);
        }

    }


    void verAlMouse()
    {
        armas[armaEquipada].transform.right = direccion * MathF.Sign(transform.localScale.x);

        var eulerDir1 = armas[armaEquipada].transform.localEulerAngles;
        // Colocar los límites de la cabeza
        eulerDir1.z = Mathf.Clamp(
            eulerDir1.z - (eulerDir1.z > 180 ? 360 : 0),
            limiteDown,
            limiteUp);
        armas[armaEquipada].transform.localEulerAngles = eulerDir1;

    }

    public void cacharRotacion(Vector2 dir)
    {
        direccion = dir;


    }


    #endregion
    //*********************************************************************************************************************************************************************************



    //********************************************************************Cambio de arma***************************************************************************************************
    //saber si cambia de arma
    private void CambioDeArmas()
    {
        if (playerInput.actions["Chage_Weapon_1"].WasPerformedThisFrame())
        {
            armaEquipada = 0;
            foreach (var arma in armas)
            {
                arma.SetActive(false);
            }
            armas[armaEquipada].SetActive(true);
        }
        else if (playerInput.actions["Chage_Weapon_2"].WasPerformedThisFrame())
        {
            armaEquipada = 1;
            foreach (var arma in armas)
            {
                arma.SetActive(false);
            }
            armas[armaEquipada].SetActive(true);
        }
    }

    //********************************************** PLAYER DATA ******************************************************
    #region PLAYER DATA
    
    #endregion
    //*****************************************************************************************************************
}
