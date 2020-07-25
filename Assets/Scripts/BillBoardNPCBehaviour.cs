using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardNPCBehaviour : MonoBehaviour
{

    [Tooltip("Objeto 3D invisible que servirá para obtener información acerca de en qué lugar y en qué dirección está mirando el NPC")] [SerializeField] Transform NPCTransform;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnWillRenderObject()
    {
        //------------------------------------------------------------------------
        //RENDERIZACIÓN BILLBOARD - ORIENTAR UN SPRITE 2D SIEMPRE HACIA LA CÁMARA
        //------------------------------------------------------------------------

        Vector3 targetVector = Camera.main.transform.position - transform.position; //Hallamos el vector que va desde el sprite a la cámara

        float newYAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg; //Hallamos la rotación que debe realizar el sprite para mirar hacia la cámara.


        //Y por último aplicamos esa rotación de manera negativa (ya que los grados van en el sentido horario y queremos que vayan al revés) al sprite, para que se oriente hacia la cámara
        transform.rotation = Quaternion.Euler(0, -newYAngle - 90, 0);
        //transform.rotation = Quaternion.LookRotation(targetVector,Vector3.up);


        Vector3 dir = (Camera.main.transform.position - NPCTransform.position).normalized; //Vector que une al NPC y a la cámara.
        float direction = Vector3.Dot(dir, NPCTransform.forward); //Relación entre el vector que une al NPC con la cámara, y la dirección en la que está mirando el NPC.
        //^Esto nos dice si la cámara mira al jugador fijamente (valor positivo) o desde atrás (valor negativo)

        //Una vez tenemos esa relación numérica, se la envíamos al Animator para que la interprete dentro de su animation tree.
        animator.SetFloat("CameraVertical", direction);
        animator.SetFloat("CameraHorizontal", Vector3.Cross(NPCTransform.forward, dir).y);
        //^El resultado de ese producto de vectores nos dirá si la cámara mira al jugador desde su derecha (valor positivo) o desde su izquierda (valor negativo)

    }
}
