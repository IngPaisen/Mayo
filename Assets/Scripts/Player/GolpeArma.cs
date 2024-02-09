using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GolpeArma : MonoBehaviour
{
    PlayerInput inputManager;
    [SerializeField] float cooldownGolpe;
    [SerializeField] float tiempoEnAtaque;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponentInParent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.actions["Golpe"].WasPressedThisFrame()) { 
        
        }
    }
}
