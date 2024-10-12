using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public int countdownTime = 10; // Tiempo inicial en segundos
    public Text countdownText; // Texto UI para mostrar el tiempo restante
    public ImageDegradeTimer imageDegradeTimer; // Referencia al script ImageDegradeTimer
    public GameObject canvas; // Referencia al canvas que deseas desactivar
    int x = 0;
    private void Start()
    {
        StartCoroutine(StartCountdown());
    }
    private IEnumerator StartCountdown()
    {
        int currentTime = countdownTime;

        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        // Cuando el temporizador llega a 0, desactiva el canvas
        countdownText.text = "0";
        Debug.Log("Countdown terminado, desactivando canvas e iniciando ImageDegradeTimer.");

        // Desactiva todo el canvas
        canvas.SetActive(false);

        // Iniciar el degradado de imagen cuando termine el countdown
        imageDegradeTimer.StartDegradeTimer();
    }
}
