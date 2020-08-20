using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
///Script encargado de gestionar el input y el movimiento del jugador.
public class MovementController : MonoBehaviour
{
    [Header("References", order = 0)]
    [SerializeField] Transform cam;
    [SerializeField] Animator animator;
    [SerializeField] CinemachineFreeLook cinemaCam;
    [Tooltip("Layer que contiene los objetos que se identificarán como 'suelo'")] [SerializeField] LayerMask groundMask;
    [Tooltip("Punto en el espacio donde se proyecta la esfera que detectará si el jugador está tocando el suelo")] [SerializeField] Transform groundCheck;
    private CharacterController controller;
    private PlayerInput playerInput;


    [Header("Movement variables", order = 1)]
    [Tooltip("Velocidad de movimiento del jugador")] [SerializeField] float speed = 6f;
    [Tooltip("Fuerza de la gravedad aplicada al jugador")] [SerializeField] float gravity = -9.81f;
    [Tooltip("Altura del salto del jugador")] [SerializeField] float jumpHeight = 3f;
    [Tooltip("Radio de la esfera proyectada en los pies del jugador para detectar si está tocando el suelo")] [SerializeField] float groundDistance = 0.4f;
    [Tooltip("Distancia recorrida en el dash")] [SerializeField] float dashDistance = 4f;
    [Tooltip("Cooldown del dash")] [SerializeField] float dashCooldownTime = 0.4f;
    [Tooltip("Simulación de fuerzas (aire, fricción) que ralentizan y detienen al jugador al moverse, saltar, dashear, etc")] 
    [SerializeField] Vector3 drag =  new Vector3(10f,1f,10f);
    private Vector3 direction;
    private Vector3 moveDir;
    private Vector3 velocity;
    private bool isGrounded;
    [HideInInspector] public bool canDash;
    [HideInInspector] public bool canMove;


    [Header("Camera settings", order = 2)]
    [Tooltip("Tiempo de cooldown para el snap de la cámara")] [SerializeField] float snapCooldown = 0.5f;
    [Tooltip("Duración de la animación de cambio de ángulo de la cámara")] [SerializeField] float snapDuration = 0.2f;
    private bool canSnapCamera;

    //todo: Gestionar mejor los casos en los que el jugador se detenga en seco para atacar. Ahora mismo siempre que canMove se pone a false el jugador
    //se detiene instantaneamente, pero probablemente no querremos que esto sea así para todos los casos.
    //¿Como gestionarlo? O bien por eventos, o bien fusionando este script con el CombatController.


    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        //Esta linea cambia el estado del cursor cuando se está jugando, escondiéndolo y bloqueándolo en el centro de la ventana, para que no se vea y no estorbe.
        Cursor.lockState = CursorLockMode.Confined;
        canSnapCamera = true;
        canMove = true;
        canDash = true;
    }

    #region INPUT MANAGEMENT

    public void OnMovement(InputValue value)
    {
        var v = value.Get<Vector2>(); //El valor leído siempre debe corresponder con el asignado a la acción dentro del Action Map

        if(canMove) //Recoge el input sólo si el jugador puede moverse.
        {
            direction = new Vector3(v.x, 0f, v.y); //Normalmente este vector deberia normalizarse, pero los valores vienen ya normalizados del sistema de Input.
        } 
        else //Si no puede, resetea el vector a cero para que no queden 'direcciones residuales' recogidas de inputs anteriores. 
        {
            direction = Vector3.zero;
        }
        
    }
    public void OnJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //Formula de salto 'realista' basada en físicas y en la elevación deseada del salto
            animator.SetBool("isJumping", true); //reproduce la animación de salto
        }

    }

    public void OnDash()
    {
        if (canDash)
        {
            //Añade a la velocidad un vector resultante de la multiplicación de la dirección del jugador (transform.forward) por un vector compuesto a partir del
            //dashDistance y el drag, basado en fórmulas físicas 'realistas'.
            velocity += Vector3.Scale(transform.forward,
                                       dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime),
                                                                  0,//^No entiendo muy bien por qué, pero a mayor el drag, mayor el dash en este caso.
                                                                  (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
            animator.SetBool("isDashing", true); //Reproduce la animacion de dash. El booleano se pondrá automaticamente a false cuando acabe la animación gracias al 'DashBehaviour' (consultar animator para + info)
            StartCoroutine(dashCooldown(dashCooldownTime)); //Ponemos en cooldown el dash.
        }
    }

    //CONTROL DE CÁMARA
    //En relación a la estética 2.5D con sprites de 4 direcciones, se ha decidido que la cámara sólo pueda manipularse en esas 4 direcciones.
    //Para conseguir esto, cuando el jugador desee cambiar el ángulo de cámara, podrá cambiar el ángulo de ésta en un rápido "snap", rotando entre los ángulos 0,90,180 y 270.
    //De esta manera se realiza una rotación entorno al jugador que dura "snapDuration" segundos. La cámara quedará siempre clampeada a uno de los valores anteriormente mencionados.
    //Además, sólo se podrá realizar el cambio de cámara cuando la variable canSnapCamera lo permita. El "snapCooldown" controla cuantos segundos tarda en volverse a poner a true tras utilizarla.

    public void OnCameraSnapLeft()
    {
        if(canSnapCamera)
        {
            //Se inicia el cooldown de la mecánica. canSnapCamera pasa a ser false, y no será true hasta que transcurran "snapCooldown" segundos.
            StartCoroutine(snapCameraCooldown(snapCooldown));

            float targetAngle = Mathf.Round((cinemaCam.m_XAxis.Value + 90) / 90) * 90;
            //^Obtiene el ángulo objetivo de la cámara, sumando 90 al actual 
            //y redondeandolo al valor de 0,90,180,270 más cercano para evitar derivaciones indeseadas
            float oldAngle = cinemaCam.m_XAxis.Value; //Guarda el anterior valor. Si se utilizase el actual en el Lerp, conduciría a una evolución inadecuada de la interpolación.

            //Una vez se conoce el ángulo original y el objetivo, se envian a esta corrutina junto al snapDuration, que se encargará de transformar el ángulo de la cámara en "snapDuration" segundos.
            StartCoroutine(snapCamera(oldAngle, targetAngle, snapDuration));
        }

    }

    public void OnCameraSnapRight()
    {
        if (canSnapCamera)
        {
            //Se inicia el cooldown de la mecánica. canSnapCamera pasa a ser false, y no será true hasta que transcurran "snapCooldown" segundos.
            StartCoroutine(snapCameraCooldown(snapCooldown));

            float targetAngle = Mathf.Round((cinemaCam.m_XAxis.Value - 90) / 90) * 90;
            //^Obtiene el ángulo objetivo de la cámara, restando 90 al actual 
            //y redondeandolo al valor de 0,90,180,270 más cercano para evitar derivaciones indeseadas
            float oldAngle = cinemaCam.m_XAxis.Value; //Guarda el anterior valor. Si se utilizase el actual en el Lerp, conduciría a una evolución inadecuada de la interpolación.

            //Una vez se conoce el ángulo original y el objetivo, se envian a esta corrutina junto al snapDuration, que se encargará de transformar el ángulo de la cámara en "snapDuration" segundos.
            StartCoroutine(snapCamera(oldAngle, targetAngle, snapDuration));
        }

    }

    /// <summary>
    /// Interpola el eje X de la cámara entre oldAngle y targetAngle en "sec" segundos.
    /// </summary>
    IEnumerator snapCamera(float oldAngle, float targetAngle, float sec)
    {
        float elapsedTime = 0; //Variable que determina el tiempo transcurrido desde que se comenzó la interpolación

        while (elapsedTime < sec) //Mientras no haya pasado el tiempo determinado para la interpolación...
        {
            float fraction = elapsedTime / sec; //Fracción del tiempo trascurrido que dictamina la velocidad a la que debe evolucionar el Lerp

            cinemaCam.m_XAxis.Value = Mathf.Lerp(oldAngle, targetAngle, fraction); //Transforma la variable del eje X desde oldAngle a targetAngle utilizando fraction como paso de suma.
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Como el Lerp sólo da resultados aproximados, es conveniente corregir el valor final al target exacto deseado, para no causar variaciones indeseadas.
        cinemaCam.m_XAxis.Value = targetAngle;
    }

    //FUNCIONES AUXILIARES

    /// <summary>
    /// Desactiva el booleano "canSnapCamera", y no lo vuelve a poner a true hasta que transcurran "sec" segundos.
    /// </summary>
    IEnumerator snapCameraCooldown(float sec)
    {
        canSnapCamera = false;
        yield return new WaitForSeconds(sec);
        canSnapCamera = true;
    }
    /// <summary>
    /// Desactiva el booleano "canDash", y no lo vuelve a poner a true hasta que transcurran "sec" segundos.
    /// </summary>
    IEnumerator dashCooldown(float sec)
    {
        canDash = false;
        yield return new WaitForSeconds(sec);
        canDash = true;
    }

    #endregion

    void FixedUpdate()
    {
        //DETECCION DE COLISIONES
        //Proyecta una esfera en el objeto groundcheck de radio groundDistance, devolviendo true si esa esfera colisiona con un objeto que esté en la capa "groundMask"
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) //Si está en el suelo y su velocidad es menor a 0,
        {
            velocity.y = -2f; //Resetea la velocidad a -2 para que no se incremente indefinidamente.
            //No se coloca a 0 porque pueden darse casos en los que isGrounded sea true antes de que el jugador esté verdaderamente en el suelo.
            animator.SetBool("JumpDown", false); //Sirve para detener la animación de salto
        }
        else if ((velocity.y < -6 && !isGrounded) || (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpUp") && velocity.y < 0)) //Siempre que la velocidad sea inferior a -1, reproduce la animación de 'caida'. Sirve para saltos y para dejarse caer
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("JumpDown",true); //En el momento en que la animación pasa de subir a bajar, cambia los booleanos para producir un cambio de estado en el animator.
        }

        //MOVIMIENTO
        if (direction.magnitude >= 0.1f)
        {
            //MOVERSE EN FUNCION DE LA DIRECCIÓN A LA QUE APUNTA LA CAMARA.
            //La funcion Atan2 nos da la rotación en el eje Y que necesitamos a partir de la dirección en el x y en el z, lo pasamos a grados y le añadimos la coordenada y de la cámara,
            //para que el jugador se mueva en dirección a donde apunta la cámara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //Convierte la rotación de la cámara en una direccion con el vector3.forward

            if (canMove) //Si puede moverse, aplica el movimimiento en función del moveDir calculado.
            {
                //Mueve el personaje en dicha direccion, tomando en cuenta su velocidad y haciendolo independiente del framerate
                if (playerInput.currentControlScheme == "Controller") //Si se está usando mando,
                {
                    //La velocidad de movimiento dependerá de cuánto se haya movido el joystick
                    controller.Move(moveDir.normalized * direction.magnitude * speed * Time.deltaTime); 
                } 
                else //En cualquier otro caso, la velocidad es fija.
                {
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);
                }

            }
            else //Si no, para en seco al jugador.
            {
                controller.Move(Vector3.zero);
            }
        }

        //Pasamos las variables al animator para que se actualicen a tiempo real y funcionen adecuadamente.
        if (direction != Vector3.zero) //Pasamos los valores horizontal y vertical solo si no son 0. Esto se hace así para poder aplicar animaciones "idle" en función del movimiento realizado
        {
            animator.SetFloat("Horizontal", direction.x); //Movimiento horizontal en X
            animator.SetFloat("Vertical", direction.z); //Movimiento horizontal en Z. Esto se debe a que estamos trabajando en 3D.
        }
        animator.SetFloat("Speed", direction.magnitude); //Para lo que necesita el animator, basta con pasarle la magnitud (longitud al cuadrado en este caso).
        //^Cuando sea positiva, aplicará animaciones de movimiento. Cuando no, animaciones "idle".

        //GRAVEDAD Y FÍSICAS
        velocity.y += gravity * Time.deltaTime; //GRAVEDAD

        //Drag
        //Como queremos tener control del drag individual en cada eje, lo aplicamos individualmente de esta manera, cogiendo un Vector3 como drag.
        velocity.x /= 1 + drag.x * Time.deltaTime;
        velocity.y /= 1 + drag.y * Time.deltaTime;
        velocity.z /= 1 + drag.z * Time.deltaTime;

        if (canMove)
            controller.Move(velocity * Time.deltaTime); //Finalmente, tras todas las simulaciones necesarias, movemos al jugador en función de la velocidad calculada

    }
}
