using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class GeneradorDiaNoche : MonoBehaviour
{
    // el color de la noche se aplica sobre los tilemaps de los GameObjects Fondo y Pista
    [SerializeField] private GameObject fondo;
    [SerializeField] private GameObject pista;
    
    [SerializeField] private Color nocheColor;
    [SerializeField] private Light2D luz2D;


    [SerializeField][Range(1, 128)] private int duracionDia;
    [SerializeField][Range(1, 24)] private int dias;

    private Color diaColor;
    // se modica la intensidad de la luz (no el color)
    private float luzIntensidad = 1.0f;
    // dirección de modificación de la intensidad de la luz (-1 baja, +1 sube)
    private int luzDireccion = -1;  


    void Start()
    {
        diaColor = fondo.GetComponent<Tilemap>().color; // componente Tilemap del GameObject fondo
        StartCoroutine(CambiarColor(duracionDia));
    }

    IEnumerator CambiarColor(float tiempo)
    {
        //se define el color a aplicar
        Color colorDestinoFondo = fondo.GetComponent<Tilemap>().color == diaColor ? nocheColor : diaColor;
        Color colorDestinoPista = pista.GetComponent<Tilemap>().color == diaColor ? nocheColor : diaColor;
        float duracionCiclo = tiempo * 0.8f;
        float duracionCambio = tiempo * 0.2f;

        for (int i = 0; i < dias; i++)
        {
            yield return new WaitForSeconds(duracionCiclo);

            float tiempoTranscurrido = 0;

            while (tiempoTranscurrido < duracionCambio)
            {
                tiempoTranscurrido += Time.deltaTime;
                luzIntensidad += luzDireccion * 0.8f * Time.deltaTime / duracionCambio; // se varía la intensidad de la luz    
                float t = tiempoTranscurrido / duracionCambio;

                float smoothT = Mathf.SmoothStep(0f, 0.01f, t);

                // se aplica el color a los tilemaps de los GameObjects Fondo y Pista
                fondo.GetComponent<Tilemap>().color = Color.Lerp(fondo.GetComponent<Tilemap>().color, colorDestinoFondo, smoothT);
                pista.GetComponent<Tilemap>().color = Color.Lerp(pista.GetComponent<Tilemap>().color, colorDestinoPista, smoothT);
                // se asigna el valor de intensidad calculado 
                luz2D.intensity = luzIntensidad;

                yield return null;
            }

            // se actualizan los valores
            colorDestinoFondo = fondo.GetComponent<Tilemap>().color == diaColor ? nocheColor : diaColor;
            colorDestinoPista = pista.GetComponent<Tilemap>().color == diaColor ? nocheColor : diaColor;
            luzDireccion *= -1;
        }
    }
}
