using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ondaToro : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Piso") || collision.transform.CompareTag("Player")){
            //Debug.Log("todo bien;");
        }
        else
        {
            Destroy(gameObject); //activar aniamcion que se rompe
        }
    }
}
 