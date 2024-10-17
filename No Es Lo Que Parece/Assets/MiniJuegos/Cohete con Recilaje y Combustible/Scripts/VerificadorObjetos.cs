using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class VerificadorObjetos : MonoBehaviour
{
    public string tagReciclaje = "Reciclaje"; // Tag de reciclaje
    public string tagCombustible = "Combustible"; // Tag de combustible

    public GameObject[] objetosDesactivar; // Lista de objetos a desactivar
    public GameObject[] objetosActivar; // Lista de objetos a activar
    public Animator animator; // Referencia al Animator

    // Lista de escenas para cargar aleatoriamente
    public List<string> escenasParaCargar; // Asegúrate de agregar las escenas en el inspector

    // Método para iniciar la verificación
    void Start()
    {
        // Iniciar la coroutine para verificar objetos
        StartCoroutine(EsperarYVerificarObjetos());
    }

    // Coroutine para esperar y luego verificar objetos
    private IEnumerator EsperarYVerificarObjetos()
    {
        while (true) // Bucle infinito para verificar continuamente
        {
            yield return new WaitForSeconds(1f); // Espera 5 segundos
            VerificarObjetos(); // Llama al método para verificar objetos
        }
    }

    // Método para verificar la existencia de objetos con los tags especificados
    public void VerificarObjetos()
    {
        // Comprobar si existen objetos con los tags especificados
        bool hayReciclaje = GameObject.FindGameObjectWithTag(tagReciclaje) != null;
        bool hayCombustible = GameObject.FindGameObjectWithTag(tagCombustible) != null;

        if (!hayReciclaje && !hayCombustible)
        {
            // Desactivar objetos
            DesactivarObjetos(objetosDesactivar);

            // Activar objetos
            ActivarObjetos(objetosActivar);

            // Activar el trigger del animator
            animator.SetTrigger("Fin");

            Debug.Log("No hay objetos de reciclaje ni de combustible. Se han activado nuevos objetos y se ha activado el trigger 'Fin'.");

            // Iniciar la coroutine para esperar unos segundos y luego cargar una escena
            StartCoroutine(EsperarYCargarEscena());
        }
        else
        {
            Debug.Log("Existen objetos de reciclaje o combustible.");
        }
    }

    // Coroutine para esperar y luego cargar una escena aleatoria
    private IEnumerator EsperarYCargarEscena()
    {
        // Esperar un tiempo para permitir que la animación se complete
        yield return new WaitForSeconds(7f); // Ajusta este tiempo según sea necesario

        // Cargar una escena aleatoria de la lista
        int randomIndex = Random.Range(0, escenasParaCargar.Count); // Genera un índice aleatorio
        string sceneToLoad = escenasParaCargar[randomIndex]; // Obtiene la escena aleatoria
        SceneManager.LoadScene(sceneToLoad); // Carga la escena seleccionada
    }

    // Método para desactivar una lista de objetos
    private void DesactivarObjetos(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            obj.SetActive(false);
        }
    }

    // Método para activar una lista de objetos
    private void ActivarObjetos(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            obj.SetActive(true);
        }
    }
}
