using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GeneradorObjetos : MonoBehaviour
{
    public GameObject[] reciclajes; // Lista de objetos reciclables a generar
    public GameObject[] combustibles; // Lista de objetos combustibles a generar
    public int dificultad; // Escala de dificultad (1 a 4)

    public int maxReciclajesPorDificultad = 5; // Máximo de reciclajes generados
    public int maxCombustiblesPorDificultad = 5; // Máximo de combustibles generados

    void Start()
    {
        // Llamar a GenerarObjetos al inicio
        GenerarObjetos();
    }

    // Método para iniciar la generación de objetos
    public void GenerarObjetos()
    {
        // Validar la dificultad
        if (dificultad < 1 || dificultad > 4)
        {
            Debug.LogWarning("La dificultad debe estar entre 1 y 4.");
            return;
        }

        // Calcular la cantidad de objetos a generar basándose en la dificultad
        int cantidadReciclajes = Random.Range(1, maxReciclajesPorDificultad * dificultad + 1);
        int cantidadCombustibles = Random.Range(1, maxCombustiblesPorDificultad * dificultad + 1);

        // Generar reciclajes
        for (int i = 0; i < cantidadReciclajes; i++)
        {
            GenerarObjeto(reciclajes);
        }

        // Generar combustibles
        for (int i = 0; i < cantidadCombustibles; i++)
        {
            GenerarObjeto(combustibles);
        }
    }

    // Método para instanciar un objeto aleatorio de la lista proporcionada
    private void GenerarObjeto(GameObject[] objetos)
    {
        int indiceAleatorio = Random.Range(0, objetos.Length);
        GameObject objetoGenerado = Instantiate(objetos[indiceAleatorio], ObtenerPosicionAleatoria(), Quaternion.identity);
        Debug.Log("Generado: " + objetoGenerado.name);
    }

    // Método para obtener una posición aleatoria dentro de un rango
    private Vector3 ObtenerPosicionAleatoria()
    {
        // Aquí puedes personalizar el rango de generación
        float x = Random.Range(-10f, 10f); // Cambiar según tus límites
        float y = Random.Range(-10f, 10f); // Cambiar según tus límites
        return new Vector3(x, y, 0);
    }
}
