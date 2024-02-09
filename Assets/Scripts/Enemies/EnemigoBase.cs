using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemigoBase : MonoBehaviour
{
    [Header("Stats Enemy")]
    [SerializeField] public float danio;
    [SerializeField] public float vidaEnemy;

    private void Update()
    {
        checarVida();
    }

    public void checarVida() {

        if (vidaEnemy <= 0)
        {
            //hacer animacion de muerte
            Destroy(gameObject);
        }
    }

    public void recibirDaño(float d) {
        vidaEnemy -= d;
    }
}
