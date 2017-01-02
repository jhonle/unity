using UnityEngine;
using System.Collections;

public class Proyectil : MonoBehaviour
{	
    private float maxTiempoVida = 2f;
    private float radioExplosion = 2f;
    public ParticleSystem explosion;  
    public AudioSource explosionS;
    private Rigidbody proyectil;
    private Renderer render;
    private static int paredes = 8;
    private static int tanque = 10;
    //private int layerMask = ( 8 << paredes) | (10 << tanque);
    private void Start ()
    {
    	proyectil = GetComponent<Rigidbody>();    	
        render = GetComponent<Renderer>();    	
        Destroy(gameObject, maxTiempoVida);
    }
    private void OnTriggerEnter (Collider other)
    {
        Muro pared = other.GetComponent<Muro>();
        if(pared)
        {
                pared.destruir();
                Destroy(gameObject);                
        }
        Arbol arbol = other.GetComponent<Arbol>();
        if(arbol)
        {
                arbol.CaidaArbol();
                Destroy(gameObject);                
        }
        render.enabled=false;
        proyectil.isKinematic=true;
        explosion.Play();
        explosionS.Play();
        Destroy(gameObject, explosionS.clip.length);

        /*Collider[] colisiones = Physics.OverlapSphere(transform.position, radioExplosion, Physics.AllLayers);

        for (int i = 0; i < colisiones.Length; i++)
        {          
            Rigidbody objeto = colisiones[i].GetComponent<Rigidbody> ();
            
            if (!objeto)
                continue;
             Muro muro = objeto.GetComponent<Muro>();
            if(muro)
            {
				muro.destruir();
            	Destroy(gameObject);
            	break;
            }
            Agente agente = objeto.GetComponent<Agente>();
            if(agente)
            {               	
            	agente.Destruir();
               	Destroy(gameObject);
               	break;
            }			
        }*/
    }
}