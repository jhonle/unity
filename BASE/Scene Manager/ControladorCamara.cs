using UnityEngine;
using System.Collections;

public class ControladorCamara : MonoBehaviour 
{
 private float limSuperior=15f;
 private float limInferior=-16f;
 private float radio=12f;
 private Vector3 velocidad;
 public Rigidbody tanque;
	void Start () 
	{	 
	}
	void FixedUpdate () 
	{

		Vector3 nuevaPosicion = new Vector3(tanque.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, nuevaPosicion, ref velocidad, 0.4f);
 	/*
        if (transform.position.z < 0 && (transform.position.z - radio) > limInferior)
        {
            Vector3 nuevaPosicion = new Vector3(transform.position.x, transform.position.y, tanque.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, nuevaPosicion, ref velocidad, 0.2f);
        }
        else if (transform.position.z > 0 && (transform.position.z + radio) < limSuperior)
        {
            Vector3 nuevaPosicion = new Vector3(transform.position.x, transform.position.y, tanque.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, nuevaPosicion, ref velocidad, 0.2f);
        }*/
	}
}