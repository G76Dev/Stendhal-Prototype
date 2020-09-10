using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{

    [Tooltip("Daño causado por este arma")] [SerializeField] int damage = 30;
    [Tooltip("Retroceso causado a los enemigos que daña este arma")] public float knockback = 0.35f;
    [Tooltip("Cooldown entre ataque y ataque")] [SerializeField] float cooldown = 0.2f;
    [Tooltip("Cantidad de espacio que avanza el jugador al atacar en una direccion con este arma")] [SerializeField] float thrust = 1;
    [Tooltip("Punto en el espacio utilizado para calcular la dirección del knockback aplicado a enemigos")] [SerializeField] Transform impactVector;
    [Tooltip("Partículas generadas cada vez que este arma impacta a un enemigo")] [SerializeField] GameObject hitParticles;
    [Tooltip("Punto desde donde se lanzará el raycast para encontrar el punto de colisión del arma donde generar las hitParticles")] [SerializeField] GameObject RaycastOrigin;

    public delegate void ComboEvent();
    public event ComboEvent comboCheckEvent; //Evento lanzado en las animaciones de ataque para ejecutar la lógica del combo en el script.

    public float getCooldown()
    {
        return cooldown;
    }

    public float getThrust()
    {
        return thrust;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") //Sólo podrá hacer daño a objetos con el tag 'Enemy'
        {
            other.gameObject.GetComponent<Enemy>().takeDamage(damage, knockback, RaycastOrigin.transform.forward, this.gameObject); //Aplica el daño al enemigo
            //other.gameObject.GetComponent<ForceApplier>().AddImpact(impactVector.forward.normalized, knockback); //Aplica un knockback junto al golpe, en direcccion de la punta de la espada (impactVector)
            //^Ojo con esto que a veces se bugea y lanza al enemigo a la mierda.

            //Instanciación de partículas de golpe.
            RaycastHit hit;
            //Lanza un rayo desde el punto RaycastOrigin (que se encuentra en la base del arma) hacia el enemigo que ha sido golpeado. Si el raycast tiene éxito
            //y choca con algo dentro de la distancia máxima, devolverá la información y posición de la colisión en la variable hit.
            if(Physics.Raycast(RaycastOrigin.transform.position,other.transform.position - RaycastOrigin.transform.position,out hit,3f))
            {
                //Si el raycast tiene exito, instancia las particulas en el punto de colisión
                Instantiate(hitParticles, hit.point, Quaternion.identity);
            }
        }
    }


    /// <summary>
    /// Función llamada en selecitvamente en las animaciones de ataque para gestionar la lógica del combo desde los scripts que reaccionen al evento que lanza ésta.
    /// </summary>
    public void comboCheck()
    {
        if (comboCheckEvent != null)
            comboCheckEvent();
    }

}
