using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestruirProyectil : MonoBehaviour
{
    [SerializeField] float tiempoVida;
    /*
        CAMBIAR A POOLOBJECT
     */
   
    void Start()
    {
        // Llama a la funci�n DESACTIVA el objeto despu�s de X segundos
        Invoke("EliminarObjeto", tiempoVida);
        //StartCoroutine(ReducirEscaladeObjeto());

    }

    void EliminarObjeto()
    {
        Destroy(gameObject);

    }

    //IEnumerator ReducirEscaladeObjeto()// no es necesario, se puede colocar unas particlas de impacto, pero de momento coloque este metodo para que el objeto se haga peque�o
    //{
    //    float tiempoPasado = 0f;
    //    Vector3 escalaInicial = transform.localScale;
    //    Vector3 escalaFinal = Vector3.zero; // Escala final ser� (0, 0, 0)

    //    while (tiempoPasado < tiempoVida)
    //    {
    //        tiempoPasado += Time.deltaTime;
    //        transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tiempoPasado / tiempoVida);
    //        yield return null;
    //    }

    //    // Aseg�rate de establecer la escala final expl�citamente para evitar errores de aproximaci�n
    //    transform.localScale = escalaFinal;
    //}
}

    