using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InterfaProvisional : MonoBehaviour
{
    [SerializeField] MovimientoP playerMovimiento;
    [SerializeField] GameObject[] armas;
    [SerializeField] Armas armasPlayer;
    [SerializeField] Slider vida;
    [SerializeField] TextMeshProUGUI mayonesaActual;
    [SerializeField] TextMeshProUGUI costoMayonesa;
    [SerializeField] PlayerSaltoE playerSalto;
    [SerializeField] PlayerDashE playerDash;

    private void Start()
    {
        playerDash=FindObjectOfType<PlayerDashE>();
        playerSalto=FindObjectOfType<PlayerSaltoE>();
        playerMovimiento = FindObjectOfType<MovimientoP>();
    }
    public void EquiparPistolita()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            armas[i].gameObject.SetActive(false);
        }
        armas[0].SetActive(true);
        equiparArmaAlPlayer(0);
    }
    public void EquiparEscopeta()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            armas[i].gameObject.SetActive(false);
        }
        armas[1].SetActive(true);
        equiparArmaAlPlayer(1);

    }
    public void EquiparSubfusil()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            armas[i].gameObject.SetActive(false);
        }
        armas[2].SetActive(true);
        equiparArmaAlPlayer(2);

    }
    public void EquiparSniper()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            armas[i].gameObject.SetActive(false);
        }
        armas[3].SetActive(true);
        equiparArmaAlPlayer(3);

    }
    public void EquiparGranada()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            armas[i].gameObject.SetActive(false);
        }
        armas[4].SetActive(true);
        equiparArmaAlPlayer(4);

    }
    public void Equiparmolotv()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            armas[i].gameObject.SetActive(false);
        }
        armas[5].SetActive(true);
        equiparArmaAlPlayer(5);

    }


    void equiparArmaAlPlayer(int arma) {
        armasPlayer.EquiparArma(arma);
    }

    public void EquiparBotasNormales()
    {
        playerMovimiento.cacharBotas(true);
        playerMovimiento.cacharBotasMejoradas(false);
        playerSalto.cacharBotasMejoradas(false);
        playerDash.cacharBotas(false);
    }
    public void EquiparBotasMejoradas()
    {
        playerMovimiento.cacharBotas(true);
        playerMovimiento.cacharBotasMejoradas(true);
        playerSalto.cacharBotasMejoradas(true);
        playerDash.cacharBotas(true);
    }

    public void DesequiparBotas() {
        playerMovimiento.cacharBotas(false);
        playerMovimiento.cacharBotasMejoradas(false);
        playerSalto.cacharBotasMejoradas(false);
        playerDash.cacharBotas(false);
    }


    //vida
    public void actualizarVidaMaxima(float vidaRecibida)
    {
        Debug.Log("VidaActualizadaMaxima");
        vida.maxValue = vidaRecibida;
    }
    public void actualizarVida(float vidaRecibida)
    {
        Debug.Log("VidaActualizada");
        vida.value = vidaRecibida;
    }


    //mayonesa
    public void actualizarMayonesaActual(float mayo) {
        Debug.Log("MayoActualActualizada");

        mayonesaActual.text = ""+mayo;
    }

    public void actualizarCostoMayonesa(float mayo) {
        Debug.Log("CostoMayoActualizada");

        costoMayonesa.text = "" + mayo;
    }

}
