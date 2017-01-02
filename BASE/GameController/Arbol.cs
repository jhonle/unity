using UnityEngine;
using System.Collections;

public class Arbol : MonoBehaviour
{	
    private Animation caida;
	private AudioSource sonido;

	// Use this for initialization
	void Start () 
	{
        caida = GetComponent<Animation>();
		sonido = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	public void CaidaArbol()
	{        
        caida.Play();
		sonido.Play();
		Destroy(gameObject,sonido.clip.length);
	}
}
