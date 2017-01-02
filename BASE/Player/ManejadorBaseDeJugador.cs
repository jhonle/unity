using UnityEngine;
using System.Collections;

public class ManejadorBaseDeJugador : MonoBehaviour 
{
   	public bool didInit;   
   	public manejadorBaseUsuario manegadorDeDatos; 
   
   	public virtual void Awake()
   	{
       didInit=false;
       // rather than clutter up the start() func, we call Init to
       // do any startup specifics
       Init();
    }
	public virtual void Init ()
	{
	    manegadorDeDatos = gameObject.GetComponent<manejadorBaseUsuario>();
        if(manegadorDeDatos==null)
	       manegadorDeDatos = gameObject.AddComponent<manejadorBaseUsuario>();
	    didInit= true;
	}
	public virtual void JuegoFinalizado()
	{
		manegadorDeDatos.SetFinalizado(true);
	}
	public virtual void IniciarJuego()
	{
		manegadorDeDatos.SetFinalizado(false);
	}
}
