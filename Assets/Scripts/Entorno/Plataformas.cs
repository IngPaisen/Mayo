using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Plataformas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) { 
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.GetComponent<Collider2D>(), false);//->Activa la coliscion con esa plataforma

        }
    }
}
