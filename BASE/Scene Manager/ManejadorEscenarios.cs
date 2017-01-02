using UnityEngine;
using System.Collections;

public class ManejadorEscenarios : MonoBehaviour 
{
   public string[] nombreNiveles = {"nivel 1"};
   public int nivelActual;
   
   public void Iniciar ()
   {
       // keep this object alive
       DontDestroyOnLoad (this.gameObject);
   }
   public void CargarNivel( string escenario)
   {
       Application.LoadLevel(escenario);
   }
   public void SiguienteNivel()
   {      
      if( nivelActual >= nombreNiveles.Length )
           nivelActual = 0;
           // load the level (the array index starts at 0, but we start
           // counting game levels at 1 for clarity’s sake)
           CargarNivel( nivelActual );
           // increase our game level index counter
           nivelActual++;
   }
   private void CargarNivel( int nivelActual )
   {
       // load the game level
       CargarNivel( nombreNiveles[nivelActual] );
   }
   public void ResetGame()
   {
       // reset the level index counter
       nivelActual = 0;
   }
}