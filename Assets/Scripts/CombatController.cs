using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementController))]
public class CombatController : MonoBehaviour
{
    [Header("References", order = 0)]
    [SerializeField] Animator animator;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] WeaponBehaviour weaponBehaviour;

    [Header("Attack Pointer variables", order = 1)]
    [Tooltip("Objeto que contiene al puntero y sobre el que éste pivota")] [SerializeField] Transform attackPointerContainer;
    [Tooltip("Puntero de ataque que se orientará en dirección de donde apunte el jugador")] [SerializeField] Transform attackPointer;
    [Tooltip("Capas en las que puede colisionar el RayCast para hallar el punto al que debe mirar el puntero de ataque")] [SerializeField] LayerMask hitLayers;
    private Vector2 mousePos;


    private Transform playerTransform; //Transform del objeto 3D invisible que representa la lógica del jugador en el mundo
    private ForceApplier forceApplier; //Script dedicado a aplicar 'impulsos' a character controllers
    private MovementController movementController;

    [Header("Combat variables", order = 2)]
    [Tooltip("Impulso añadido al jugador cuando éste ataca")] [SerializeField] float attackImpulse = 1f;
    private bool canAttack;
    private int noOfTaps; //Número de taps o clicks que ha realizado el jugador desde que comenzó su combo de ataque.

    private void Start()
    {
        movementController = GetComponent<MovementController>();
        playerTransform = GetComponent<Transform>();
        forceApplier = GetComponent<ForceApplier>();

        noOfTaps = 0;
        canAttack = true;
        
    }

    //EVENTS
    private void OnEnable()
    {
        weaponBehaviour.comboCheckEvent += comboAttack;
    }

    private void OnDisable()
    {
        weaponBehaviour.comboCheckEvent -= comboAttack;
    }


    public void OnAttack()
    {
        if (canAttack)
        { 
            noOfTaps++; //En cada paso del combo, si se puede atacar, acumula un "tap"
            playerTransform.LookAt(new Vector3(attackPointer.position.x, playerTransform.position.y, attackPointer.position.z));
        }

        if (noOfTaps == 1) //Si el número de  taps es exactamente 1,
        {
            //Activa el trigger del animador para que reproduzca la animación correspondiente.
            animator.SetTrigger("Attack");
            //Al comenzar el combo, bloqueamos el movimiento y el dash del jugador, que no podrá volver a moverse libremente hasta que decida terminar el combo.
            movementController.canMove = false;
            movementController.canDash = false;
            weaponAnimator.SetInteger("Animation", 1);
            forceApplier.AddImpact(playerTransform.forward, attackImpulse);//AÑADIR IMPULSO
        }
    }

    public void OnMouseAim(InputValue value)
    {
        mousePos = value.Get<Vector2>(); //Actualiza la variable local que utilizaremos en update con el valor dado por el sistema de Input.
    }

    // Update is called once per frame
    void Update()
    {
        RotatePointer();
    }

    /// <summary>
    /// Lanza un rayo desde el punto relativo del ratón hacia el suelo, hallando el punto de intersección al que mirará el Pointer.
    /// </summary>
    private void RotatePointer()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePos); //Transforma la posición del ratón en un rayo que parte desde ese punto de la pantalla.
        RaycastHit rayCollisionPoint; //Punto del rayo en el que éste colisiona con el 'groundPlane'


        if(Physics.Raycast(cameraRay,out rayCollisionPoint, hitLayers)) //Si al proyectar el rayo colisiona con algún punto de las 'hitLayer', devuelve el punto del rayo donde se produce la intersección
        {
            //Actualmente, se ha predispuesto un plano 'Ground Plane' dentro del objeto jugador al nivel de sus pies cuya layer es 'Ground'. Es el único objeto con el que puede chocar el rayo,
            //de forma que el puntero siempre mirará en la dirección correcta gracias a que este plano invisible está a los pies del jugador y cubre un amplio rango de espacio.
            Vector3 pointToLookAt = rayCollisionPoint.point; //Y transforma ese punto en un vector3 que nos diga a qué punto del despacio debe mirar el Pointer
            Debug.DrawLine(cameraRay.origin, pointToLookAt, Color.blue); //Debug line para corroborar todo lo anterior

            attackPointerContainer.LookAt(new Vector3 (pointToLookAt.x, transform.position.y, pointToLookAt.z)); //Finalmente, hacemos que el pointer mire hacia ese punto rotándolo únicamente en los ejes X y Z, manteniendo su altura Y
        }
    }

    IEnumerator attackCooldown(float sec)
    {
        canAttack = false;
        yield return new WaitForSeconds(sec);
        canAttack = true;
    }

    /// <summary>
    /// Normalmente, esta función será invocada por eventos desencadenados en las animaciones de ataque. 
    /// En ese momento esta función decide qué animación ejecutar a continuación.
    /// </summary>
    private void comboAttack()
    {
        //todo: buscar una implementación más cómoda y escalable que permita crear nuevos combos más fácilmente
        canAttack = false; //Ponemos "canAttack" a false durante la comprobación del estado del combo. Una vez lo sepamos, lo volveremos a poner a true. Así evitamos malas lecutras.

        if(weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash1") && noOfTaps == 1)
        {
            //Si no se han registrado nuevos "taps" antes de haber llegado a este punto en la animación, vuelve a "idle" y corta el combo.
            weaponAnimator.SetInteger("Animation", 0);
            movementController.canMove = true;
            movementController.canDash = true;
            StartCoroutine(attackCooldown(weaponBehaviour.getCooldown())); //El cooldown entre ataques viene definido por el arma del usuario.
            noOfTaps = 0; //Como el combo ha terminado, reseteamos esta variable.

        }
        else if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash1") && noOfTaps >= 1)
        {
            //Si se han registrado uno o más taps durante la primera animación, eso quiere decir que el jugador quiere continuar con el combo.
            animator.SetTrigger("Attack");
            weaponAnimator.SetInteger("Animation", 2);
            canAttack = true;
            //StartCoroutine(cooldownMovement()); //Detiene en seco al jugador al poner canMove a falso durante un corto lapso de tiempo.
            forceApplier.AddImpact(playerTransform.forward, attackImpulse);//AÑADIR IMPULSO
        } 
        else if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash2") && noOfTaps == 2)
        {
            //Si durante el segundo ataque no se han registrado nuevos taps, eso quiere decir que el jugador no quiere continuar el combo. Volvemos a idle.
            weaponAnimator.SetInteger("Animation", 0);
            movementController.canMove = true;
            movementController.canDash = true;
            StartCoroutine(attackCooldown(weaponBehaviour.getCooldown())); //El cooldown entre ataques viene definido por el arma del usuario.
            noOfTaps = 0; //Como el combo ha terminado, reseteamos esta variable.
        } 
        else if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash2") && noOfTaps >= 3)
        {
            //Si se han registrado nuevos taps durante el segundo ataque, eso quiere decir que el jugador quiere continuar con el combo.
            animator.SetTrigger("Attack");
            weaponAnimator.SetInteger("Animation", 3);
            canAttack = true;
            //StartCoroutine(cooldownMovement()); //Detiene en seco al jugador al poner canMove a falso durante un corto lapso de tiempo.
            weaponBehaviour.knockback = weaponBehaviour.knockback * 5; //Como el movimiento es un finisher, queremos que tenga mayor knockback.
            forceApplier.AddImpact(playerTransform.forward, attackImpulse);//AÑADIR IMPULSO
        }
        else if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash3"))
        {
            //Al ser el tercer ataque el último del combo, volveremos automáticamente a idle cuando termine.
            weaponAnimator.SetInteger("Animation", 0);
            movementController.canMove = true;
            movementController.canDash = true;
            weaponBehaviour.knockback = weaponBehaviour.knockback / 5; //Tras el finisher, devolvemos el valor del knockback al original.
            //IDEA: Asignar el knockback y el daño individual de cada ataque del combo mediante eventos en las propias animaciones!
            StartCoroutine(attackCooldown(weaponBehaviour.getCooldown())); //El cooldown entre ataques viene definido por el arma del usuario.
            noOfTaps = 0; //Como el combo ha terminado, reseteamos esta variable.
        } else
        {
            weaponAnimator.SetInteger("Animation", 0);
            movementController.canMove = true;
            movementController.canDash = true;
            StartCoroutine(attackCooldown(weaponBehaviour.getCooldown())); //El cooldown entre ataques viene definido por el arma del usuario.
            noOfTaps = 0; //Como el combo ha terminado, reseteamos esta variable.
        }
    }

    IEnumerator cooldownMovement()
    {
        movementController.canMove = false;
        yield return new WaitForSeconds(weaponBehaviour.getCooldown()); //parametrizar
        movementController.canMove = true;
    }

    /// <summary>
    /// Devuelve true si el animator pasado por referencia está ejecutando una animación.
    /// </summary>
    bool AnimatorIsPlaying(ref Animator anim)
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
