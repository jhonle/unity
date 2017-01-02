using UnityEngine;
using System.Collections;

public class Muro : MonoBehaviour
 {
    private AudioSource caidaMuro;          
    private Animation animacion;
    public Renderer ladrillo;
    private BoxCollider colision;
    private NavMeshObstacle obstaculo;
	void Start () 
	{		
		animacion = GetComponent<Animation>();
		caidaMuro = GetComponent<AudioSource>();
		colision = GetComponent<BoxCollider>();
		obstaculo = GetComponent<NavMeshObstacle>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	public void destruir()
	{
		colision.enabled=false;
		caidaMuro.Play();
		animacion.Play();
		Destroy(obstaculo);
		ladrillo.enabled = false;      
		Destroy(gameObject, animacion.clip.length);
	}
}
