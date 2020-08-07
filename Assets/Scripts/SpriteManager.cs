using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{

    public Transform playerTransform;
    //public string sheetname;
    public Sprite[] sprites;
    private SpriteRenderer sr;

    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //sprites = (Sprite[])Resources.LoadAll(sheetname);
        sr = GetComponent<SpriteRenderer>();

        //SHADOW 'SHADER'
        //SOLO FUNCIONA SI SE LE ASIGNA AL SPRITE RENDERER EL MATERIAL ADECUADO
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            Debug.Log("Renderer is empty");
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        GetComponent<Renderer>().receiveShadows = true;

    }

    void ChangeSprite(int index)
    {
        Sprite sprite = sprites[index];
         sr.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnWillRenderObject()
    {
        Vector3 targetVector = Camera.main.transform.position - transform.position; //Hallamos el vector que va desde el sprite a la cámara

        float newYAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg; //Hallamos la rotación que debe realizar el sprite para mirar hacia la cámara.

        //Y por último aplicamos esa rotación de manera negativa (ya que los grados van en el sentido horario y queremos que vayan al revés) al sprite, para que se oriente hacia la cámara
        transform.rotation = Quaternion.Euler(0, -newYAngle - 90, 0);
        //transform.rotation = Quaternion.LookRotation(targetVector,Vector3.up);

        //EXTRA: Aplica este método alternativo para cosas que quieras que SIEMPRE miren hacia la cámara, independientemente de su coordenada Y o Z.
        //Por ejemplo, un buen uso pueden ser los números indicadores de daño, texto, elementos de UI, etc.
        //transform.rotation = Quaternion.LookRotation(targetVector, Camera.main.transform.rotation * Vector3.up);


        ////NEXT: Dibujar el sprite correspondiente a la orientacion del personaje comparando el vector direccion del cilindro 3D invisible y el vector de la cámara

        Vector3 dir = (Camera.main.transform.position - playerTransform.position).normalized; //Vector que une al jugador y a la cámara
        float direction = Vector3.Dot(dir, playerTransform.forward); //Relación entre el vector que une al jugador con la cámara, y la dirección en la que está mirando el jugador
        //^Esto nos dice si la cámara mira al jugador fijamente (valor positivo) o desde atrás (valor negativo)

        //Una vez tenemos esa relación numérica, se la envíamos al Animator para que la interprete dentro de su animation tree.
        animator.SetFloat("CamOrientation", direction);
        animator.SetFloat("PositiveCrossY", Vector3.Cross(playerTransform.forward, dir).y);
        //^El resultado de ese producto de vectores nos dirá si la cámara mira al jugador desde su derecha (valor positivo) o desde su izquierda (valor negativo)

        ////En funcion de esa relación, dada por el .dot de esos dos vectores, decidimos que sprite se debe mostrar:
        //switch (direction)
        //{

        //    case float d when (d > 0.75f && d <= 1):
        //        //print("FRONT");
        //        ChangeSprite(0);
        //        break;

        //    case float d when (d < 0.75f && d >= 0.25f):
        //        //print("3/4 DOWN");

        //        //EN CASOS DONDE NO BASTA "direction" PORQUE SU VALOR ES IGUAL PARA LA IZQUIERDA Y LA DERECHA:
        //        //El cruce del vector que une al jugador con la cámara, y el de la dirección a la que mira el jugador, nos da información importante.
        //        //Ese vector será perpendicular al plano que forman los dos anteriores, y dependiendo de su valor "y" podemos establecer si la cámara está
        //        //a la derecha o a la izquierda del jugador. Por ejemplo:
        //        if(Vector3.Cross(playerTransform.forward,dir).y < 0)
        //        {
        //            //Como "y" es negativo, la cámara queda a la derecha del jugador, por lo que debemos mostrar la versión izquierda del sprite.
        //            ChangeSprite(0); //1
        //        }
        //        else //La cámara está a la derecha del jugador
        //        {
        //            ChangeSprite(0); //7
        //        }
        //        break;

        //    case float d when (d > -0.25f && d < 0.25F):
        //        //print("LEFT/RIGHT");

        //        if (Vector3.Cross(playerTransform.forward, dir).y < 0)
        //        {
        //            //Como "y" es negativo, la cámara queda a la derecha del jugador, por lo que debemos mostrar la versión izquierda del sprite.
        //            ChangeSprite(2);
        //        }
        //        else //La cámara está a la derecha del jugador
        //        {
        //            ChangeSprite(6);
        //        }
        //        break;

        //    case float d when (d < -0.25f && d >= -0.75f):
        //        //print("3/4 UP");

        //        if (Vector3.Cross(playerTransform.forward, dir).y < 0)
        //        {
        //            //Como "y" es negativo, la cámara queda a la derecha del jugador, por lo que debemos mostrar la versión izquierda del sprite.
        //            ChangeSprite(4); //3
        //        }
        //        else //La cámara está a la derecha del jugador
        //        {
        //            ChangeSprite(4); //5
        //        }
        //        break;

        //    case float d when (d < -0.75f && d >= -1):
        //        //print("BACK");
        //        ChangeSprite(4);
        //        break;
        //}

        //if (playerTransform.rotation.y > -0.5f && playerTransform.rotation.y < 0.5f) //Si el jugador va en la direccion derecha..     
        //{
        //    //ToDo: Detectar estos casos de una forma más segura, y sobre todo, menos circunstancial al transform del jugador y la escena en la que se encuentre.
        //    ChangeSprite(6);

        //}
        //else //En cualquier otro caso, irá en la dirección izquierda. Esto lo sabemos gracias a la condición para entrar en este case.
        //{
        //    ChangeSprite(1);
        //}
    }
}
