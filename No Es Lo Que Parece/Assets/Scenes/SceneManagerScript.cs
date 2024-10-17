using UnityEngine;
using UnityEngine.SceneManagement; // Asegúrate de incluir esta librería

public class SceneManagerScript : MonoBehaviour
{
    // Método para cargar una escena por su nombre
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Método para cerrar el juego
    public void CerrarJuego()
    {
#if UNITY_EDITOR
        // Si estás en el editor, detiene el juego
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estás en una build, cierra la aplicación
        Application.Quit();
#endif
    }
}
