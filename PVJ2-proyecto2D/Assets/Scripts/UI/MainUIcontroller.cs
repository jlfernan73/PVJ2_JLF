using UnityEngine;
using UnityEngine.SceneManagement;


public class MainUIcontroller : MonoBehaviour
{
    public void CargarSiguienteEscena()
    {
        int indiceEscenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indiceEscenaActual+1);                       //Para pasar de nivel (no resetea variables globales)
        GameManager.Instance.SetVictoria(false);
        GameManager.Instance.EscalarExperiencia();
        Time.timeScale = 1;
    }
    public void CargarEscena(int escena)
    {
        SceneManager.LoadScene(escena, LoadSceneMode.Single);               //Usado cuando reinicia
        GameManager.Instance.ResetPuntaje();                                //Resetea variables globales
        GameManager.Instance.ResetVidas();
        GameManager.Instance.ResetExperiencia();
        GameManager.Instance.ResetExperienciaNivel();
        GameManager.Instance.SetGameOver(false);
        GameManager.Instance.SetVictoria(false);
        Time.timeScale = 1;
    }
    public void CargarEscenaAdicional(int escena)                           //Abre la escena sin cerrar la original
    {
        SceneManager.LoadScene(escena, LoadSceneMode.Additive);
    }
    public void CerrarEscenaAdicional(int escena)                           //Cierra la escena abierta en simultaneo
    {
        SceneManager.UnloadSceneAsync(escena);
    }
    public void RecargarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        GameManager.Instance.ResetPuntaje();                                //Resetea variables globales
        GameManager.Instance.ResetVidas();
        GameManager.Instance.ResetExperiencia();
        GameManager.Instance.SetGameOver(false);
        GameManager.Instance.SetVictoria(false);
        Time.timeScale = 1;
    }
    public void SalirJuego()                                                //Sale del juego
    {
        Application.Quit();
    }
}
