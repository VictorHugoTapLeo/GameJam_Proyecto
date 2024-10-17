using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Variables públicas para establecer los tags desde el inspector
    public string combustibleTag = "Combustible";
    public string reciclajeTag = "Reciclaje";
    public Animator animator; // Referencia al Animator

    // Método que se llama al colisionar con otro objeto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprobar si colisiona con un objeto que tiene el tag de combustible
        if (collision.CompareTag(combustibleTag))
        {
            Destroy(collision.gameObject); // Destruir el objeto
            Debug.Log("Destruido: " + collision.gameObject.name);
        }

        // Comprobar si colisiona con un objeto que tiene el tag de reciclaje
        if (collision.CompareTag(reciclajeTag))
        {
            // Activar el trigger del animator
            animator.SetTrigger("DestruccionNave");
            Debug.Log("Activado trigger de destrucción para: " + collision.gameObject.name);
            // Iniciar la coroutine para esperar a que la animación termine antes de cargar el menú
            StartCoroutine(WaitAndLoadMenu());
        }
    }

    // Coroutine para esperar un tiempo y luego cargar el menú principal
    private IEnumerator WaitAndLoadMenu()
    {
        // Esperar un tiempo para que la animación se reproduzca (ajusta este tiempo según la duración de tu animación)
        yield return new WaitForSeconds(3f); // Cambia el valor si es necesario para que coincida con la duración de tu animación

        // Cargar la escena MenuPrincipal
        SceneManager.LoadScene("MenuPrincipal");
    }
}

