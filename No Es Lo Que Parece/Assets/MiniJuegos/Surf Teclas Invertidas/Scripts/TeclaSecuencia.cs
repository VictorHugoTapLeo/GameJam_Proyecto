using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Agregar esto para manejar escenas

public class TeclaSecuencia : MonoBehaviour
{
    public GameObject[] prefabsTeclas; // Array para los prefabs de las teclas (flechitas)
    public float intervaloGeneracion = 1f; // Intervalo de tiempo entre la generación de teclas
    public List<string> secuenciaTeclas; // Almacena la secuencia generada
    public int cantidadBaseSecuencia = 5; // Cantidad base de teclas a generar
    public int nivelActual = 1; // Nivel actual del juego
    private int indiceActual = 0; // Índice de la tecla actual que se debe presionar
    private List<GameObject> teclasGeneradas; // Lista para almacenar las teclas generadas
    private int intentosActuales = 0; // Contador de intentos
    private bool secuenciaGenerada = false; // Indica si la secuencia ha sido generada completamente

    private Collider2D collider; // Referencia al collider donde se moverán las teclas
    private Animator animator; // Referencia al Animator

    // Lista de nombres de escenas a las que se puede redirigir
    public List<string> escenasPosibles;

    void Start()
    {
        secuenciaTeclas = new List<string>();
        teclasGeneradas = new List<GameObject>(); // Inicializa la lista de teclas generadas
        collider = GetComponent<Collider2D>(); // Obtener el Collider2D del GameObject
        animator = GetComponent<Animator>(); // Obtener el Animator del GameObject

        StartCoroutine(GenerarSecuencia());
        StartCoroutine(RestablecerColorTeclas()); // Iniciar coroutine para restablecer colores
    }

    IEnumerator GenerarSecuencia()
    {
        // Esperar 3 segundos antes de comenzar la generación
        yield return new WaitForSeconds(3f);

        // Calcula la cantidad total de teclas en la secuencia según el nivel
        int cantidadSecuencia = cantidadBaseSecuencia * nivelActual;

        for (int i = 0; i < cantidadSecuencia; i++)
        {
            // Seleccionar aleatoriamente un prefab y generar la tecla
            GameObject teclaPrefab = prefabsTeclas[Random.Range(0, prefabsTeclas.Length)];
            string tecla = teclaPrefab.name; // Asume que el nombre del prefab es el nombre de la tecla
            secuenciaTeclas.Add(tecla);
            GameObject nuevaTecla = CrearTecla(teclaPrefab); // Almacena la tecla generada
            teclasGeneradas.Add(nuevaTecla); // Agregar a la lista de teclas generadas
            yield return new WaitForSeconds(intervaloGeneracion);
        }

        secuenciaGenerada = true; // La secuencia ha sido generada
    }

    private GameObject CrearTecla(GameObject prefabTecla)
    {
        if (prefabTecla != null)
        {
            // Instanciar el prefab manteniendo su rotación original
            GameObject nuevaTecla = Instantiate(prefabTecla, Vector3.zero, prefabTecla.transform.rotation);
            MoverTeclaDentroDelCollider(nuevaTecla); // Mover la tecla al collider
            return nuevaTecla; // Retorna la tecla generada
        }
        else
        {
            Debug.LogWarning("Prefab de tecla es null. Asegúrate de que esté asignado correctamente.");
            return null;
        }
    }

    private void MoverTeclaDentroDelCollider(GameObject tecla)
    {
        if (collider == null)
        {
            Debug.LogWarning("No Collider2D found on this GameObject.");
            return;
        }

        // Genera una posición aleatoria dentro del collider y verifica si está ocupada
        Vector2 randomPosition;
        bool positionFound;

        do
        {
            randomPosition = GetRandomPositionInCollider(collider);
            positionFound = !IsPositionOccupied(randomPosition);
        } while (!positionFound);

        // Mueve la tecla a la nueva posición
        tecla.transform.position = randomPosition;
    }

    private Vector2 GetRandomPositionInCollider(Collider2D collider)
    {
        // Obtiene los límites del collider
        Bounds bounds = collider.bounds;

        // Genera una posición aleatoria dentro de los límites
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }

    private bool IsPositionOccupied(Vector2 position)
    {
        // Verifica si hay teclas generadas en la posición dada
        foreach (GameObject tecla in teclasGeneradas)
        {
            if (tecla != null && Vector2.Distance(tecla.transform.position, position) < 1f)
            {
                return true; // La posición está ocupada
            }
        }
        return false; // La posición está libre
    }

    void Update()
    {
        // Comprobar si hay teclas en la secuencia y si la secuencia ya fue generada
        if (secuenciaGenerada && indiceActual < secuenciaTeclas.Count)
        {
            // Comprobar la entrada del jugador según la tecla actual en la secuencia
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Tecla presionada: Derecha");
                ProcesarEntrada("Derecha");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("Tecla presionada: Izquierda");
                ProcesarEntrada("Izquierda");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("Tecla presionada: Arriba");
                ProcesarEntrada("Arriba");
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Debug.Log("Tecla presionada: Abajo");
                ProcesarEntrada("Abajo");
            }
        }
    }

    void ProcesarEntrada(string tecla)
    {
        // Verificar que la tecla procesada sea correcta
        if (secuenciaTeclas[indiceActual] == tecla)
        {
            Debug.Log($"Tecla correcta: {tecla}");

            // Desactivar el SpriteRenderer de la tecla generada
            GameObject teclaVisual = teclasGeneradas[indiceActual]; // Obtener la tecla generada correspondiente
            if (teclaVisual != null)
            {
                teclaVisual.GetComponent<SpriteRenderer>().enabled = false; // Desactivar el SpriteRenderer
                teclasGeneradas[indiceActual] = null; // Marcar como destruida para evitar errores
            }

            indiceActual++; // Avanzar al siguiente índice en la secuencia

            // Verifica si se completó la secuencia
            if (indiceActual >= secuenciaTeclas.Count)
            {
                Debug.Log("¡Secuencia completada!");
                animator.SetTrigger("Victoria"); // Activar el trigger 'Victoria'

                // Esperar 6 segundos antes de cargar una escena aleatoria
                StartCoroutine(CargarEscenaAleatoriaConDelay(6f)); // Cargar escena aleatoria
            }
        }
        else
        {
            Debug.Log("Orden incorrecto de la secuencia.");

            // Incrementar el contador de intentos
            intentosActuales++;

            if (intentosActuales >= 3)
            {
                Debug.Log("Demasiados intentos fallidos. Activando trigger de muerte.");
                animator.SetTrigger("Dead"); // Activar el trigger 'Dead'

                // Desactivar el SpriteRenderer de todas las teclas generadas
                DesactivarSpriteRenderersTeclas(); // Llamar a un método para desactivar los SpriteRenderers

                // Llamar al método para cargar el menú principal después de un delay
                StartCoroutine(CargarMenuPrincipalConDelay(3f)); // Esperar 3 segundos antes de cargar el menú
            }
            else
            {
                Debug.Log("Reiniciando intento.");
                // Cambiar el color de todas las teclas generadas a rojo
                CambiarColorTeclas(Color.gray); // Método para cambiar el color

                // Mover todas las teclas a nuevas posiciones
                MoverTeclas(); // Método para mover las teclas

                // Reiniciar el índice para permitir que el jugador intente nuevamente
                indiceActual = 0;
            }
        }
    }

    private IEnumerator CargarMenuPrincipalConDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Esperar el tiempo especificado
        SceneManager.LoadScene("MenuPrincipal"); // Cambiar a la escena del menú
    }

    private IEnumerator CargarEscenaAleatoriaConDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Esperar el tiempo especificado

        // Seleccionar una escena aleatoria de la lista de escenas posibles
        string escenaAleatoria = escenasPosibles[Random.Range(0, escenasPosibles.Count)];
        SceneManager.LoadScene(escenaAleatoria); // Cambiar a la escena aleatoria
    }

    private void CambiarColorTeclas(Color color)
    {
        // Cambiar el color de todas las teclas generadas
        foreach (GameObject tecla in teclasGeneradas)
        {
            if (tecla != null)
            {
                tecla.GetComponent<SpriteRenderer>().color = color; // Cambiar el color
            }
        }
    }

    private void MoverTeclas()
    {
        // Mover todas las teclas generadas a nuevas posiciones
        foreach (GameObject tecla in teclasGeneradas)
        {
            if (tecla != null)
            {
                MoverTeclaDentroDelCollider(tecla); // Mover la tecla al collider
            }
        }
    }

    private void DesactivarSpriteRenderersTeclas()
    {
        // Desactivar el SpriteRenderer de todas las teclas generadas
        foreach (GameObject tecla in teclasGeneradas)
        {
            if (tecla != null)
            {
                tecla.GetComponent<SpriteRenderer>().enabled = false; // Desactivar el SpriteRenderer
            }
        }
    }

    // Método para restablecer los colores de las teclas (opcional)
    private IEnumerator RestablecerColorTeclas()
    {
        yield return new WaitForSeconds(2f); // Esperar 2 segundos antes de restablecer colores

        while (true) // Bucle infinito para mantener el restablecimiento
        {
            CambiarColorTeclas(Color.white); // Restablecer el color a blanco
            yield return new WaitForSeconds(1f); // Esperar 1 segundo

        }
    }
}
