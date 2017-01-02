using UnityEngine;
using System.Collections;

public class ControladorBaseDelJuego : MonoBehaviour
{
   bool paused;
   public GameObject explosionPrefab;
   
   public virtual void JugadorPerdioVida ()
   {
     // deal with player life lost (update U.I. etc.)
   }
   public virtual void RevivirJugador ()
   {
    // the player needs to be spawned
   }
   public virtual void revivir()//revisar
   {
     // the player is respawning
   }
   public virtual void IniciarJuego()
   {
     // do start game functions
   }
   public void Explotar ( Vector3 posicion )
   {
     // instantiate an explosion at the position passed into this function
     Instantiate( explosionPrefab,posicion, Quaternion.identity );
   }
   public virtual void EnemigoDestruido(Vector3 posicion, int pointsValue,int hitByID )
   {
       // deal with enemy destroyed
   }
   public virtual void JefeDestruido()
   {
     // deal with the end of a boss battle
   }
   public virtual void BotonReiniciar()
   {
     // deal with restart button (default behavior re-loads the
     // currently loaded scene)
     Application.LoadLevel(Application.loadedLevelName);
    }
    public bool Paused
    {
       get
       {    
          return paused;
       }
       set
       {
          paused = value;
          if(paused)
          {
             Time.timeScale= 0f;
          }
          else
          {
             Time.timeScale = 1f;
          }
       }
    }
}