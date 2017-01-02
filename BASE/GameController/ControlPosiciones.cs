using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ControlPosiciones : MonoBehaviour
{
   
    private GameObject[] bosques;
    void Start()
    {        
        bosques = GameObject.FindGameObjectsWithTag("bosque");        
    }
    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
    }
    public void mostrarNombre(GameObject[] arreglo)
    {
        for (int f = 0; f < arreglo.Length; f++)
        {
            Debug.Log("" + arreglo[f].name);
        }
    }
    public void mostrarNombre(List<GameObject> arreglo)
    {
        foreach(GameObject objeto in arreglo)
        {
            Debug.Log("" + objeto.name);
        }
    }
    public Transform getMuroCercano(Transform agente)
    {
        GameObject[] muros = GameObject.FindGameObjectsWithTag("muro");        
        Transform res = agente;
        double distancia = 200;
        double aux = 0;
        foreach(GameObject muro in muros) 
        {
            aux = calcularDistancia(agente, muro.GetComponent<Transform>());
            if (aux < distancia)
            {
                distancia = aux;
                res = muro.GetComponent<Transform>();
            }
        }
        //Debug.Log("distancia al muro es: " + distancia);
        return res;
    }
    public Vector3 atrasMuro(Transform muro, Transform jugador)
    {
        Vector3 res = new Vector3();
        float x = (muro.position.x + 5) - jugador.position.x;//atras
        float x1 = (muro.position.x - 5) - jugador.position.x;//delante
        if (x > x1)
        {
            res.Set((muro.position.x - 5), muro.position.y, muro.position.z);
        }
        else
        {
            res.Set((muro.position.x + 5), muro.position.y, muro.position.z);
        }
        return res;
    }
    private double calcularDistancia(Transform a, Transform b)
    {
        float x = b.position.x - a.position.x;
        float z = b.position.z - a.position.z;
        double distancia = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));
        return distancia;
    }
    public Vector3 getBosqueCercano(Transform agente)
    {
        Vector3 res = new Vector3();
        double distancia = 200;
        double aux = 0;
        for (int f = 0; f < bosques.Length; f++)
        {
            aux = calcularDistancia(agente, bosques[f].GetComponent<Transform>());
            if (aux < distancia)
            {
                distancia = aux;
                res = bosques[f].GetComponent<Transform>().position;
            }
        }
        //Debug.Log("distancia al muro es: " + distancia);
        return res;
    }    
    public int getNroMuros()
    {
        GameObject[] muros = GameObject.FindGameObjectsWithTag("muro");
        return muros.Length;
    }
}
