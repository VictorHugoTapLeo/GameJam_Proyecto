using UnityEngine;

public class GravedadCero2D : MonoBehaviour
{
    public float speed = 5f; // Velocidad inicial del objeto
    private Vector2 movementDirection; // Dirección de movimiento
    private Rigidbody2D rb; // Referencia al Rigidbody2D para manipular la física
    private BoxCollider2D campoGeneracion; // Referencia al campo de generación
    private bool isDragging = false; // Para verificar si el objeto está siendo arrastrado
    private Vector3 initialPosition; // Posición inicial del objeto antes de ser arrastrado

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Desactivar la gravedad

        // Buscar el campo de generación por tag
        campoGeneracion = GameObject.FindGameObjectWithTag("CampoGeneracion").GetComponent<BoxCollider2D>();

        // Iniciar con un impulso aleatorio en cualquier dirección
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.velocity = movementDirection * speed;
    }

    void Update()
    {
        if (!isDragging)
        {
            // Continuar moviendo el objeto en la dirección actual si no está siendo arrastrado
            rb.velocity = movementDirection * speed;
        }
        else
        {
            // Si el objeto está siendo arrastrado, seguir la posición del mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ajustar la posición en Z
            transform.position = mousePosition;
        }
    }

    // Detecta cuando el mouse empieza a arrastrar el objeto
    private void OnMouseDown()
    {
        isDragging = true; // Iniciar el arrastre
        rb.velocity = Vector2.zero; // Detener el movimiento mientras se arrastra
        initialPosition = transform.position; // Guardar la posición inicial
    }

    // Detecta cuando el mouse deja de arrastrar el objeto
    private void OnMouseUp()
    {
        isDragging = false; // Terminar el arrastre

        // Verificar si el objeto está fuera del área del campo de generación
        if (!campoGeneracion.bounds.Contains(transform.position))
        {
            // Si el objeto está fuera, regresa a la posición inicial
            transform.position = initialPosition; // Regresar a la posición inicial
            Debug.Log("El objeto ha sido soltado fuera del área, regresando a la posición inicial.");
        }
        else
        {
            // Si el objeto está dentro, continuar movimiento normal
            movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.velocity = movementDirection * speed;
        }
    }

    void FixedUpdate()
    {
        if (!isDragging)
        {
            // Mantener el objeto dentro de los límites del campo de generación
            KeepObjectWithinBounds();
        }
    }

    // Mantiene el objeto dentro de los límites del BoxCollider2D del campo de generación
    void KeepObjectWithinBounds()
    {
        Bounds bounds = campoGeneracion.bounds; // Obtener los límites del campo de generación

        // Si el objeto está fuera del límite por la izquierda o derecha
        if (transform.position.x < bounds.min.x)
        {
            transform.position = new Vector3(bounds.min.x, transform.position.y, transform.position.z);
            ChangeMovementDirection(); // Cambiar dirección
        }
        else if (transform.position.x > bounds.max.x)
        {
            transform.position = new Vector3(bounds.max.x, transform.position.y, transform.position.z);
            ChangeMovementDirection(); // Cambiar dirección
        }

        // Si el objeto está fuera del límite por arriba o abajo
        if (transform.position.y < bounds.min.y)
        {
            transform.position = new Vector3(transform.position.x, bounds.min.y, transform.position.z);
            ChangeMovementDirection(); // Cambiar dirección
        }
        else if (transform.position.y > bounds.max.y)
        {
            transform.position = new Vector3(transform.position.x, bounds.max.y, transform.position.z);
            ChangeMovementDirection(); // Cambiar dirección
        }
    }

    // Cambia la dirección de movimiento del objeto
    void ChangeMovementDirection()
    {
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.velocity = movementDirection * speed; // Reiniciar velocidad
    }
}
