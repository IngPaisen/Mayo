using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImpulsoEscopeta : MonoBehaviour
{
    [SerializeField] PlayerInput playerI;
    [SerializeField] PlayerSaltoE playerSalto;
    [SerializeField] Rigidbody2D rbPlayer;
    [SerializeField] float fuerzasalto;
    // Start is called before the first frame update
    void Start()
    {
        playerSalto = GetComponentInParent<PlayerSaltoE>();
        rbPlayer = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void impusloEscopeta() {

        // Obtener la posición del mouse en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(playerI.actions["Look"].ReadValue<Vector2>());

        // Calcular la dirección desde el jugador al mouse
        Vector2 directionToMouse = (mousePosition - transform.position).normalized;

        // Detectar si la dirección es hacia abajo
        if (directionToMouse.y < 0&& playerI.actions["Aim"].IsPressed()&&!playerSalto.retornarEnPiso())
        {
            Debug.Log("Apuntando hacia abajo");
            Jump(fuerzasalto);
            // Resto de tu lógica aquí...
        }
    }

    void Jump(float force)
    {
        // Aplicar fuerza hacia arriba
        rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, 0); // Reiniciar la velocidad vertical para evitar acumulación
        rbPlayer.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

}
