using UnityEngine;
using System;
using System.Diagnostics;
public class Agente : MonoBehaviour 
{
	
    public Transform objetivo;	
	public GameObject proyectil;    
	public Transform canion;
    private Rigidbody tanque;
    private AudioSource explosionTanque;
    private float fuerza = 50f;
	private DateTime ultimoDisparo;
	private int primerDisparo=0;
    private NavMeshAgent agente;
    private Vector3 adelante;
    private Vector3 atras;
    private float girarDerecha;
    private float girarIzquierda;
    private Quaternion rotacion;
    private int con=0;
    private String accion="avanzar";
    private Stopwatch control = new Stopwatch();
    private double distancia = 20;
    private double tiempo=0;
    private Boolean completo = false;
   
    private void Start () 
	{
		tanque = GetComponent<Rigidbody>();
        agente = GetComponent<NavMeshAgent>();
        explosionTanque = GetComponent<AudioSource>();
        adelante = transform.forward * 10f * Time.deltaTime;
        atras = transform.forward * -10f * Time.deltaTime;
        girarDerecha = 90f * Time.deltaTime;
        girarIzquierda= -90f * Time.deltaTime;        
    }
    private void Awake()
    {
    }
    private void Update()
    {
        
    }
    private void BuscarObjetivo()
    {
        agente.destination = objetivo.position;
    }
    private void CambioControlManual()
    {
        tanque.isKinematic = false;
        agente.enabled = false;
    }
    private void CambioControlAgente()
    {
        agente.enabled = true;
        tanque.isKinematic = true;
    }
    private Boolean Avanzar(int distancia)
    {
        if (con == 0)
        {
            tiempo = distancia * 2 / 10;
            tanque.MovePosition(tanque.position + adelante);
            con++;
            control.Start();
            return false;
        }
        else if (control.Elapsed < TimeSpan.FromSeconds(tiempo))
        {
            tanque.MovePosition(tanque.position + adelante);
            return false;
        }
        else
        {
            con = 0;
            control.Stop();
            tiempo = 0;
            return true;
        }
    }
    private Boolean Retroceder()
	{
        if (con == 0)
        {
            tiempo = distancia * 2 / 10;
            tanque.MovePosition(tanque.position + atras);
            con++;
            control.Start();
            return false;
        }
        else if (control.Elapsed < TimeSpan.FromSeconds(tiempo))
        {
            tanque.MovePosition(tanque.position + atras);
            return false;
        }
        else
        {
            con = 0;
            control.Stop();
            tiempo = 0;            
            return true;
        }
    }
	private Boolean GirarDerecha(int grados)
	{
        if (con == 0)
        {
            rotacion = Quaternion.Euler(0f, girarDerecha, 0f);
            tiempo = (grados*2.81) / 90;
            control.Start();
            con++;
            tanque.MoveRotation(tanque.rotation * rotacion);
            completo = false;
        }
        else if (control.Elapsed < TimeSpan.FromSeconds(tiempo))
        {
            tanque.MoveRotation(tanque.rotation * rotacion);
            completo = false;
        }
        else if (control.Elapsed > TimeSpan.FromSeconds(tiempo))
        {
            con = 0;
            //CambioControlAgente();
            control.Stop();
            tiempo = 0;
            completo = true;
        }
        return completo;
    }
	private Boolean GirarIzquierda(int grados)
	{        
        if (con == 0)
        {
            rotacion = Quaternion.Euler(0f, girarIzquierda, 0f);
            tiempo = (grados * 2.81) / 90;
            control.Start();
            con++;
            tanque.MoveRotation(tanque.rotation * rotacion);
            completo = false;
        }
        else if (control.Elapsed < TimeSpan.FromSeconds(tiempo))
        {
            tanque.MoveRotation(tanque.rotation * rotacion);
            completo = false;
        }
        else if (control.Elapsed > TimeSpan.FromSeconds(tiempo))
        {
            con = 0;
            //CambioControlAgente();
            control.Stop();
            tiempo = 0;
            completo = true;
        }
        return completo;
    }
	private void Disparar()
	{                
        if (primerDisparo == 0)
		{
            GameObject bala = Instantiate(proyectil, canion.position, canion.rotation) as GameObject;
            bala.GetComponent<Rigidbody>().velocity = fuerza * canion.forward;
            ultimoDisparo = DateTime.Now;
            primerDisparo =1;
		}
		else if(DateTime.Now >= ( ultimoDisparo + TimeSpan.FromSeconds(1)))
		{
			GameObject bala = Instantiate (proyectil, canion.position, canion.rotation) as GameObject;
        	bala.GetComponent<Rigidbody>().velocity = fuerza * canion.forward;
        	ultimoDisparo = DateTime.Now;            
        }
	}
    public void Destruir()
    {
        explosionTanque.Play();
        Destroy(gameObject, explosionTanque.clip.length);
    }
    private void EstrategiaEsquivo()
    {
        Boolean accion1 = false;
        Boolean accion2 = false;

        if (con == 0)
        {
            CambioControlManual();
            accion1 = GirarDerecha(60);
            con++;
        }
        else if (accion1 == false)
        {
            accion1 = GirarDerecha(60);
        }
        else if (accion1 == true)
        {
            accion2 = Avanzar(20);
        }

        
           
    }
}
