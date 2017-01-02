using UnityEngine;
using System.Collections;

public class bala : MonoBehaviour
{
    private ParticleSystem particulas;
    public AudioSource explosion;
    private Collider colision;
    private Renderer render;
    private Rigidbody cuerpo;
    void Start()
    {
        particulas = GetComponentInChildren<ParticleSystem>();
        colision = GetComponent<Collider>();        
        render = GetComponent<Renderer>();
        cuerpo = GetComponent<Rigidbody>();
        Destroy(gameObject, 2);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        render.enabled = false;
        cuerpo.isKinematic = true;
        particulas.Play();
        explosion.Play();
        Destroy(gameObject,particulas.duration);
    }
}