using UnityEngine;
using System.Collections;

public class manejadorBaseUsuario : MonoBehaviour
{
// gameplay specific data
// we keep these private and provide methods to modify them
// instead, just to prevent any accidental corruption
// or invalid data coming in
    private int puntuacion;
    private int puntuacionMasAlta;
    private int nivel;
    private int vidas;
    private bool finalizado;
    public string nombre ="Anon";
    
    public virtual void GetDatosPorDefecto()
    {
       nombre="Anon";
       puntuacion=0;
       nivel=1;
       vidas=3;
       puntuacionMasAlta=0;
       finalizado=false;
    }
	public string GetNombre()
	{
 	   return nombre;
    }
    public void Setnombre(string aNombre)
    {
       nombre=aNombre;
	}
	public int GetNivel()
	{
 	    return nivel;
	}
    public void SetNivel(int num)
	{
 	    nivel=num;
	}
    public int GetPuntuacionMasAlta()
    {
       return puntuacionMasAlta;
    }
    public int GetPuntuacion()
	{
 	   return puntuacion;
	}
	public virtual void AniadirPuntaje(int puntaje)
	{
       puntuacion+=puntaje;
	}
	public void PerderPuntaje(int puntaje)
	{
 	    puntuacion-=puntaje;
	}
	public void SetPuntaje(int puntaje)
	{
 	    puntuacion=puntaje;
	}
	public int GetVidas()
	{
 	    return vidas;
	}	
    public void ReducirVidas(int vida)
    {
        vidas-=vida;
	}
	public void SetVidas(int num)
	{
 	    vidas=num;
	}
	public bool GetFinalizado()
	{
 		return finalizado;
	}
	public void SetFinalizado(bool fin)
	{
 		finalizado=fin;
	}
}