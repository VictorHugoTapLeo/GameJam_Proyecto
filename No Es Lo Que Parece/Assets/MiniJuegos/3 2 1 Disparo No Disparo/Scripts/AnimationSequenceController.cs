using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class AnimationSequenceController : MonoBehaviour
{
    int count = 0;
    // Referencias a los Animators
    public Animator contrincanteAnimator;  // Primer Animator
    public Animator companeroAnimator;     // Segundo Animator
    public Animator atacanteAnimator;      // Tercer Animator

    // Nombre de los triggers en cada Animator
    public string muerteContrincanteTrigger = "MuerteContrincante";
    public string companeroTrigger = "DisparoCompañero";
    public string muerteAtacanteTrigger = "MuerteAtacante";

    // Tiempo de espera entre la ejecución de las animaciones (en segundos)
    public float companeroDelay = 1f;      // Retraso antes de la animación del compañero
    public float atacanteDelay = 2f;       // Retraso antes de la animación del atacante

    private bool isAnimating = false; // Variable para controlar si se está ejecutando la secuencia de animaciones

    private void Update()
    {
        // Detectar clic en la pantalla
        if (Input.GetMouseButtonDown(0) && !isAnimating) // Solo inicia si no se está animando
        {
            // Comenzar la secuencia de animaciones
            StartCoroutine(StartAnimationSequence());
        }
    }

    // Secuencia de animaciones
    private IEnumerator StartAnimationSequence()
    {
        isAnimating = true; // Indica que la secuencia está en ejecución

        // Activar el trigger del contrincante
        contrincanteAnimator.SetTrigger(muerteContrincanteTrigger);

        // Esperar a que termine la animación del contrincante (esperamos el tiempo de la animación)
        yield return new WaitForSeconds(contrincanteAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Después de la animación del contrincante, esperar un poco y luego activar el trigger del compañero
        yield return new WaitForSeconds(companeroDelay);
        companeroAnimator.SetTrigger(companeroTrigger);

        // Esperar a que termine la animación del compañero
        yield return new WaitForSeconds(companeroAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Después de la animación del compañero, esperar unos segundos más y activar el trigger del atacante
        yield return new WaitForSeconds(atacanteDelay);
        atacanteAnimator.SetTrigger(muerteAtacanteTrigger);

        // Esperar 10 segundos antes de cambiar de escena
        yield return new WaitForSeconds(4.5f);

        // Cambiar a la escena "MenuPrincipal"
        SceneManager.LoadScene("MenuPrincipal");
    }
}
