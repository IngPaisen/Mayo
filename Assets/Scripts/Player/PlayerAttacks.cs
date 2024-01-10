using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttacks : MonoBehaviour
{
    /*                       ESTE SCRIPT CONTROLA BASICAMENTE LAS BALAS DEL PLAYER                       */

    [SerializeField] float danioAtaque;//NO ES DEL TODO NECESARIO QUE SEA MODIFICACBLE DESDE EL INSPECTOR, YA QUE SE CACHAN DESDE OTRO SCRIPT

    public void DestruirBala() {
        Destroy(gameObject);
        //gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        //transform.localScale = new Vector3(1, 1, 1);//tal vez sea necesario de modificar esta linea, es para la poolobject
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestruirBala();
    }


    //=========================================== GETERS & SETTERS =========================================
    #region GETERS & SETTERS
    public float getDanioAtaque()
    {
        return danioAtaque;
    }
    public void setDanioAtaque(float dA)
    {
        danioAtaque = dA;
    }
    //public void setPotenciaDisparo(float Pd)
    //{
    //    potenciaDisparo = Pd;
    //}

    #endregion
    //=======================================================================================================

}


