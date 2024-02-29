using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escombors : MonoBehaviour
{
    [SerializeField] float danioPlayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float tiempoVida;
    [SerializeField] bool desdeCielo = false;
    //[SerializeField] Material material;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        //material=GetComponent<Material>();
        Invoke("destruir",tiempoVida);
    }

    void destruir() {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale -= new Vector3(.005f,0.005f,0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //si impacta cambar de sprite para que se vea que se rompe
        rb.sharedMaterial.bounciness -= .2f;
        rb.gravityScale -= .2f;

        if (desdeCielo) { 
            Destroy(gameObject);
        }

    }

    public float returnarDaño() {
        return danioPlayer; 
    }


}
