using UnityEngine;
using System.Collections;
using System;
public class Tank : MonoBehaviour 
{    
    private Rigidbody tanque;    
    private float velocidad = 12f;
    private float rotacion = 120f;
	private bool disparo;
	private float vertical;
	private float horizontal;    
	private Transform posicion;	
    private float fuerza = 50f;               
    private int primerDisparo=0;
    private DateTime ultimoDisparo;

	public Transform posicionCanion;
	public Rigidbody proyectil;               
	public AudioSource sonidoMovimiento;
	public AudioClip tanqueParado;
	public AudioClip tanqueAvance;    
	private void Start () 
	{
		tanque = GetComponent<Rigidbody>();
		posicion = GetComponent<Transform>();       
	}		
	private void Update ()
	{
		RevisarEntradas();
		manejadorSonido();		
		
		if(Input.GetButtonUp ("Fire1")  && primerDisparo==0)
        {                
            disparar();
            ultimoDisparo = DateTime.Now;
            primerDisparo=1;
        }
        else if(Input.GetButtonUp("Fire1") && (DateTime.Now >= ( ultimoDisparo + TimeSpan.FromSeconds(1))) )
        {                
            disparar();
            ultimoDisparo = DateTime.Now;            
        }
		/*if(Input.GetButtonDown ("Fire1"))
        {                
            disparo = false;
        }
        else if(Input.GetButtonUp("Fire1") && !disparo && primerDisparo==0)
        {                
            disparar();
            ultimoDisparo = DateTime.Now;
            primerDisparo=1;
        }
        else if(Input.GetButtonUp("Fire1") && !disparo && (DateTime.Now >= ( ultimoDisparo + TimeSpan.FromSeconds(1))))
        {
            disparar();
            ultimoDisparo = DateTime.Now;
        }*/
	}
	private void  FixedUpdate()
	{
        avanzar();
        girar();
        tanque.isKinematic=true;
        tanque.isKinematic=false;
        //tanque.velocity= new Vector3(0,0,0);
	}
	private void manejadorSonido()
	{
		if(Mathf.Abs(vertical) > 0.1f || Mathf.Abs(horizontal) > 0.1f)
		{
			if(sonidoMovimiento.clip==tanqueParado)
            {
            	sonidoMovimiento.clip = tanqueAvance;            	 
            	sonidoMovimiento.Play();
            }			
		}
		else 
		{
			if(sonidoMovimiento.clip==tanqueAvance)
            {
            	sonidoMovimiento.clip = tanqueParado;            	 
            	sonidoMovimiento.Play();
            }		
		}
		/*
		if(Mathf.Abs(vertical) > 0.1f || Mathf.Abs(horizontal) > 0.1f)
		{
			if(sonidoMovimiento.isPlaying==false)
            {
            	sonidoMovimiento.Play();
            }			
		}
		else 
		{
			if(sonidoMovimiento.isPlaying==true)
            {
            	sonidoMovimiento.Stop();
            }	
		}*/
	}
	private void girar()
	{
		if(Mathf.Abs(horizontal)>0.1f)
		{
			float girar = horizontal * rotacion * Time.deltaTime;	
        	Quaternion turnRotation = Quaternion.Euler (0f,girar, 0f);
        	tanque.MoveRotation(tanque.rotation *turnRotation);						
		}
	}		
	private void avanzar()
	{  
		if(Mathf.Abs(vertical)>0.1f)
		{            
			Vector3 movimiento = posicion.forward * vertical * velocidad * Time.deltaTime;			
            tanque.MovePosition(tanque.position + movimiento);
        }
	}
	private void RevisarEntradas()
	{
 		vertical = Input.GetAxis( "Vertical1" );
 		horizontal = Input.GetAxis( "Horizontal1" );	 	
	}
	public void detener()
	{
		//tanque.isKinematic=true;
		tanque.velocity= new Vector3(0,0,0);
		//tanque.isKinematic=false;
	}
	private void disparar()
	{
		disparo = true;        
        Rigidbody instanciaProyectil = Instantiate(proyectil, posicionCanion.position, posicionCanion.rotation) as Rigidbody;
        instanciaProyectil.velocity = fuerza * posicionCanion.forward;        
    }
}
