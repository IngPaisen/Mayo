using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TipoDisparoConMelee : TipoDisparo
{
    void detectarAtaque()
    {

        if (playerInput.actions["Atack"].IsPressed() && playerStats.getMayoActual() >= costoDisparoMayo)
        {
            Atacar();
        }
        else if (playerInput.actions["AtackMelee"].IsPressed() && armaConMelee) 
        {
            AtacarMelee();
        }

    }
}
