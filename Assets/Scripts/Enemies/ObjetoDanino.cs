using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoDanino : MonoBehaviour
{
    [SerializeField] float danioPlayer;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")) {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

            // Obtener el componente PlayerController y llamar a la función TakeDamage

            collision.gameObject.GetComponent<MovimientoP>().Rebote(knockbackDirection);
            Debug.Log("Rebote");
        }
    }

    public float returnarDaño() {
        return danioPlayer;
    }
}
