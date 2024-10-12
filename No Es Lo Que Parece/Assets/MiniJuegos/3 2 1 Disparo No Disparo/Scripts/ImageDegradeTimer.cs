using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena
using System.Collections.Generic; // Necesario para utilizar listas
using System.Collections; // Necesario para usar corutinas

public class ImageDegradeTimer : MonoBehaviour
{
    public Image timerImage; // Referencia a la imagen que representa el tiempo restante
    public float totalTime = 15f; // Tiempo total en segundos (15 segundos)
    private float currentTime;
    private bool isTimerRunning = false; // Controla si el temporizador está activo

    // Lista pública de objetos que se desactivarán cuando el tiempo llegue a 0
    public List<GameObject> objectsToDisable;

    // Lista pública de objetos que se activarán cuando el tiempo llegue a 0
    public List<GameObject> objectsToEnable;

    // Lista de nombres de escenas para cambiar aleatoriamente
    public List<string> scenesToLoad; // Asegúrate de que estas escenas estén añadidas en Build Settings
    // Referencia al Animator del otro GameObject
    public Animator deadAnimator; // Arrastra aquí el Animator del GameObject en el Inspector
    public string deadTrigger = "Dead"; // Nombre del trigger en el Animator
    // Función que será llamada para iniciar el temporizador de degradado
    public void StartDegradeTimer()
    {
        currentTime = totalTime;
        isTimerRunning = true; // Activamos el temporizador
    }

    void Update()
    {
        if (isTimerRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            // Actualizar la barra de imagen en función del tiempo restante
            timerImage.fillAmount = currentTime / totalTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                OnTimerEnd();
            }
        }
    }

    // Función que se llama cuando el tiempo llega a 0
    void OnTimerEnd()
    {
        Debug.Log("El tiempo del ImageDegradeTimer se ha agotado.");

        // Desactivar todos los objetos en la lista objectsToDisable
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null) // Verificar que no sea null
            {
                obj.SetActive(false);
            }
        }

        // Activar todos los objetos en la lista objectsToEnable
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null) // Verificar que no sea null
            {
                obj.SetActive(true);
            }
        }

        // Comenzar la espera de 3 segundos antes de cambiar de escena
        StartCoroutine(WaitAndChangeScene());
    }

    // Coroutine para esperar y luego cambiar de escena
    private IEnumerator WaitAndChangeScene()
    {
        deadAnimator.SetTrigger("Dead");
        yield return new WaitForSeconds(3f); // Esperar 3 segundos

        // Comprobar si estamos en la escena "3 2 1 Disparo No Disparo" o "EsquivarObstaculos"
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "3 2 1 Disparo No Disparo" || currentSceneName == "EsquivarObstaculos")
        {
            ChangeSceneRandomly();
        }
        else
        {
            // Si no estamos en las escenas especificadas, cargar "MenuPrincipal"


            LoadMenuPrincipal();
        }


    }

    // Método para cambiar a una escena aleatoria de la lista
    private void ChangeSceneRandomly()
    {
        if (scenesToLoad.Count > 0) // Asegurarse de que la lista no esté vacía
        {
            int randomIndex = Random.Range(0, scenesToLoad.Count);
            string sceneToLoad = scenesToLoad[randomIndex];
            Debug.Log("Cambiando a la escena: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No hay escenas disponibles para cargar.");
        }
    }

    // Método para cargar la escena MenuPrincipal
    private void LoadMenuPrincipal()
    {
        Debug.Log("Cambiando a la escena: MenuPrincipal");
        SceneManager.LoadScene("MenuPrincipal");
    }
}
