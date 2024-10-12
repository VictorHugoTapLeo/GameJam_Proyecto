using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs; // Lista de prefabs de obstáculos
    public Transform leftLane; // Posición del carril izquierdo
    public Transform rightLane; // Posición del carril derecho
    public float spawnInterval = 2f; // Intervalo entre cada generación de obstáculos
    public int maxObstacles = 5; // Máximo número de obstáculos simultáneos en la escena
    public float moveSpeed = 3f; // Velocidad con la que los obstáculos subirán
    public float obstacleLifetime = 5f; // Tiempo en segundos que durará el obstáculo antes de destruirse

    private List<GameObject> activeObstacles = new List<GameObject>(); // Lista de obstáculos activos
    private int obstacleCount = 0; // Contador de obstáculos generados

    void Start()
    {
        // Iniciar la rutina de generación de obstáculos con un retraso de 3 segundos
        StartCoroutine(SpawnObstacles());
    }

    void Update()
    {
        // Hacer que los obstáculos activos suban de abajo hacia arriba
        foreach (GameObject obstacle in activeObstacles)
        {
            if (obstacle != null)
            {
                obstacle.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            }
        }
    }

    IEnumerator SpawnObstacles()
    {
        // Esperar 3 segundos antes de comenzar a generar obstáculos
        yield return new WaitForSeconds(3f);

        // Luego comenzar la generación de obstáculos
        while (true)
        {
            if (obstacleCount < maxObstacles)
            {
                // Elegir aleatoriamente un prefab de la lista de obstáculos
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

                // Elegir aleatoriamente el carril (izquierdo o derecho)
                Transform spawnPosition = Random.Range(0, 2) == 0 ? leftLane : rightLane;

                // Generar el obstáculo en el carril elegido
                GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition.position, Quaternion.identity);
                activeObstacles.Add(newObstacle);
                obstacleCount++;

                // Cambiar el Order in Layer a 0 después de 3 segundos
                StartCoroutine(ChangeOrderInLayerAfterTime(newObstacle, 2.8f));

                // Iniciar la destrucción del obstáculo después de "obstacleLifetime" segundos
                StartCoroutine(DestroyObstacleAfterTime(newObstacle, obstacleLifetime));
            }

            // Esperar el intervalo antes de generar el siguiente obstáculo
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator ChangeOrderInLayerAfterTime(GameObject obstacle, float delay)
    {
        // Esperar el tiempo indicado por "delay" (en este caso 3 segundos)
        yield return new WaitForSeconds(delay);

        // Cambiar el Order in Layer si el obstáculo aún existe
        if (obstacle != null)
        {
            SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 0; // Cambiar Order in Layer a 0
            }
        }
    }

    IEnumerator DestroyObstacleAfterTime(GameObject obstacle, float delay)
    {
        // Esperar el tiempo indicado por "delay"
        yield return new WaitForSeconds(delay);

        // Si el obstáculo aún existe, destruirlo y reducir el contador
        if (obstacle != null)
        {
            activeObstacles.Remove(obstacle); // Remover de la lista
            Destroy(obstacle); // Destruir el obstáculo
            obstacleCount--; // Reducir el número de obstáculos activos
        }
    }
}
