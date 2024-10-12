using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PlayerMovement : MonoBehaviour
{
    int x = 0;
    public float moveSpeed = 10f; // Velocidad del jugador al moverse entre los carriles
    public float laneDistance = 3f; // Distancia entre los dos carriles
    private int currentLane = 0; // 0 es el carril izquierdo, 1 es el carril derecho
    private Vector3 targetPosition; // Posición a la que se moverá el jugador
    private Animator animator; // Referencia al Animator del jugador
    public GameObject enemySpawner; // Referencia al spawner de enemigos

    void Start()
    {
        // Inicializar la posición del jugador en el carril izquierdo
        targetPosition = transform.position;

        // Obtener el componente Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verificar la posición del jugador
        Debug.Log("Posición actual del jugador: " + transform.position);

        // Detectar la entrada del jugador para moverse a la derecha o izquierda
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane == 0)
        {
            // Mover al carril derecho
            currentLane = 1;
            targetPosition = new Vector3(transform.position.x + laneDistance, transform.position.y, transform.position.z);
            Debug.Log("Moviéndose a la derecha, nueva posición objetivo: " + targetPosition);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane == 1)
        {
            // Mover al carril izquierdo
            currentLane = 0;
            targetPosition = new Vector3(transform.position.x - laneDistance, transform.position.y, transform.position.z);
            Debug.Log("Moviéndose a la izquierda, nueva posición objetivo: " + targetPosition);
        }

        // Suavizar el movimiento del jugador hacia el nuevo carril
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    // Método para detectar triggers con el tag "Enemy"
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado con: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Trigger con un Enemy detectado");
            // Activar el trigger "Derrota" en el Animator
            if (animator != null)
            {
                animator.SetTrigger("Derrota");
            }

            // Desactivar los SpriteRenderer de todos los objetos con tag "Enemy"
            DisableEnemySprites();

            // Desactivar el spawner
            if (enemySpawner != null)
            {
                enemySpawner.SetActive(false);
                Debug.Log("Spawner desactivado.");
            }

            // Iniciar la coroutine para esperar 3 segundos y cambiar de escena
            StartCoroutine(WaitAndChangeScene());
        }
    }

    // Método para desactivar todos los SpriteRenderer de los objetos con tag "Enemy"
    private void DisableEnemySprites()
    {
        // Obtener todos los objetos en la escena con el tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Recorrer todos los objetos encontrados y desactivar sus SpriteRenderer
        foreach (GameObject enemy in enemies)
        {
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
                Debug.Log("Desactivando SpriteRenderer de: " + enemy.name);
            }
            else
            {
                Debug.Log("No se encontró SpriteRenderer en: " + enemy.name);
            }
        }
    }

    // Coroutine para esperar 3 segundos y cambiar a la escena "MenuPrincipal"
    private IEnumerator WaitAndChangeScene()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos
        SceneManager.LoadScene("MenuPrincipal"); // Cambia a la escena "MenuPrincipal"
    }
}
