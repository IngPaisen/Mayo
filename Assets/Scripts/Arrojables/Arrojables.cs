using UnityEngine;
using UnityEngine.InputSystem;

public class Arrojables : MonoBehaviour
{

    //[SerializeField] Transform destino;
    [Header(("Arrojadizos"))]
    [SerializeField] float costoMayo;
    [SerializeField] float tiempo = 2;//el tiempo que tarda e llegar, controla por la rueda del raton 
    [SerializeField] int totalSegmentos = 10;//determina que tan preciso es la linea
    [SerializeField] float distanciaMaxima; // el modo maximo, osea la curva
    [SerializeField] GameObject Arrojable; //el prefab, de momento es prefab, se tiene que hacer una poolObject
    [SerializeField] float multiArrojar; //para que el arrojamiento sea similar y no se vea raro, es necesario

    LineRenderer lineRenderer;
    Vector2 puntoParabolaReferencia;
    Vector2 velocidadInicial;
    bool estoyDisparando = false;
    PlayerInput playerinput;
    PlayerStatsE playerStats;
    SpriteRenderer sp;

    //============================================== METODOS UNITY =========================================================
    #region METODOS UNITY

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        playerStats = GetComponentInParent<PlayerStatsE>();
        playerinput = GetComponentInParent<PlayerInput>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
    }
    void Start()
    {
            //tal vez llamar el recibirmayo en playerstats
    }

    void Update()
    {
        //if (playerinput.actions["MouseScroll"].IsInProgress()) {

        //    Debug.Log("Rueda");
        //} 
        if (playerStats.getMayoActual()>=costoMayo)
        {
            sp.enabled = true;
            playerStats.recibirCostoDeMAyonesa(costoMayo);


            if (!estoyDisparando && playerinput.actions["Aim"].IsPressed())
            {

                lineRenderer.enabled = true;
                lineRenderer.positionCount = Mathf.RoundToInt(totalSegmentos * tiempo);
                velocidadInicial = CalcularVelocidadInicial(/*Input.mousePosition*/Camera.main.ScreenToWorldPoint(Input.mousePosition), this.transform.position, tiempo);
                DibujarLinea(velocidadInicial, tiempo);
                //RotarCañonHaciaDireccionDisparo();
                CecharRuedaMouse(Input.mouseScrollDelta);//no logro hacer que funcione con playerinput
            }
            else
            {
                lineRenderer.enabled = false;

            }
        }
        else {
            //ocultar Granada, como si no tuviera para lanzar
            sp.enabled = false;
            lineRenderer.enabled = false;

        }
        if (playerinput.actions["Atack"].WasPressedThisFrame() && !estoyDisparando )
        {

            if (playerStats.getMayoActual() >= costoMayo)
            {
                estoyDisparando = true;
                playerStats.restaMayo(costoMayo);
                GameObject bola = Instantiate(Arrojable, this.transform.position, Quaternion.identity);
                bola.GetComponent<Rigidbody2D>().velocity = velocidadInicial * multiArrojar;
                Invoke("ResetearDisparo", tiempo + 1); // -> cooldown entre granadas

            }
            else {
                Debug.Log("Sin Mayonesa McCormick");
            }

        }
     
        
    }
    #endregion
    //======================================================================================================================

    //cooldown Temporarl
    private void ResetearDisparo(){
        estoyDisparando = false;
    }


    //=================================================== ARROJAR ================================================================
    #region ARROJAR
    private void DibujarLinea(Vector2 velocidadInicial, float tiempo)
    {
        for (int i = 0; i < totalSegmentos * tiempo; i++)
        {
            Vector2 posicionTemporal = CalcularPosicionEnElTiempo(velocidadInicial, (i / (totalSegmentos * tiempo)) * tiempo);
            lineRenderer.SetPosition(i, posicionTemporal);

            if (i == 5)
            {
                puntoParabolaReferencia = posicionTemporal;
            }

        }
    }

    //saber que modo tiene en ese momento
    private void CecharRuedaMouse (Vector2 scrollDelta){
        
        if(scrollDelta.Equals(Vector2.down) && tiempo > 1){
            tiempo--;
        } 

         if(scrollDelta.Equals(Vector2.up) && tiempo < 2){
            tiempo++;
        }

        
    }

    //se calcula la velocidad para el arrojale
    private Vector2 CalcularVelocidadInicial(Vector2 destino, Vector2 origen, float tiempo)
    {

        Vector2 distancia = destino - origen;
        float distanciaReal = distancia.magnitude;

        // Si la distancia excede el límite, ajusta la posición del destino
        if (distanciaReal > distanciaMaxima)
        {
            Vector2 direccionLimitada = distancia.normalized * distanciaMaxima;
            destino = origen + direccionLimitada;
        }

        distancia = destino - origen;

        float velocidadInicialX = distancia.x / tiempo;
        float velocidadInicialY = distancia.y / tiempo + 0.5f * Mathf.Abs(Physics2D.gravity.y) * tiempo;

        Vector2 velocidadInicial = new Vector2(velocidadInicialX, velocidadInicialY);
        return velocidadInicial;
    }

    //saber la posicion de donde va a caer
    private Vector2 CalcularPosicionEnElTiempo (Vector2 velocidadInicial, float tiempo)
    {
        Vector2 posicionEnElTiempo = new Vector2 (this.transform.position.x + velocidadInicial.x * tiempo, (-0.5f * Mathf.Abs(Physics2D.gravity.y) * (tiempo * tiempo)) + (velocidadInicial.y * tiempo) + this.transform.position.y);
        return posicionEnElTiempo;

    }


    #endregion
    //============================================================================================================================

}
