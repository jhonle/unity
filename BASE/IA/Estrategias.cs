
using UnityEngine;
using System.Collections;

public class Estrategias
{
    private Vector3 posicionInicial = new Vector3(0, 0, 0);
    private Vector3 rotacionInicial = new Vector3(0, 0, 0);
    private bool posicionAsignada = false;
    private bool rotacionAsignada = false;
    private bool estrategiaActiva = false;
    private bool primerIngreso = false;
    private bool apuntar = false;
    private double angulo = 0;
    private Transform muro;
    private Vector3 bosque;
    public string nombre="";
    public Estrategias()
    {        
    }
    public void setMuro(Transform m)
    {
        muro = m;
    }
    public Vector3 getbosque()
    {
        return bosque;
    }
    public void setBosque(Vector3 b)
    {
        bosque = b;
    }
    public Transform getMuro()
    {
        return muro;
    }
    public void setAngulo(double ang)
    {
        apuntar = true;
        angulo = ang;
    }
    public bool getApuntar()
    {
        return apuntar;
    }
    public void setApuntar(bool b)
    {
        apuntar= b;
    }
    public double getAngulo()
    {
        return angulo;
    }
    public bool getPrimerIngreso()
    {
        return primerIngreso;
    }
    public void setPrimerIngreso(bool aux)
    {
        primerIngreso = aux;
    }
    public void SetEstrategia(bool est)
    {
        estrategiaActiva = est;
    }
    public bool GetEstrategia()
    {
        return estrategiaActiva;
    }
    public void setPosicionInicial(Transform posicion)
    {
        this.posicionInicial = posicion.position;      
        posicionAsignada = true;
    }
    public void setPosicionInicial(Vector3 posicion)
    {
        this.posicionInicial = posicion;
        posicionAsignada = true;
    }
    public void setRotacionInicial(Transform rotacion)
    {
        this.rotacionInicial = rotacion.localRotation.eulerAngles;
        rotacionAsignada = true;
    }
    public void reiniciarPocisionRotacion()
    {
        posicionAsignada = false;
        rotacionAsignada = false;
    }
    public bool getPosicionInicial()
    {
        return posicionAsignada;
    }
    public Vector3 getVector()
    {        
        return posicionInicial;
    }
    public bool getRotacionInicial()
    {
        return rotacionAsignada;
    }
    public bool distanciaRecorrida(Transform posicionFinal,double distancia)
    {
        double recorrido = calcularDistancia(posicionFinal);
        if (recorrido < distancia)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private double calcularDistancia(Transform b)
    {
        float x = b.position.x - posicionInicial.x;
        float z = b.position.z - posicionInicial.z;
        double distancia = Mathf.Sqrt( Mathf.Pow(x,2) + Mathf.Pow(z, 2) );
        return distancia;
    }
    public bool rotacion(Transform posicionFinal,double angulo)//no funciona con los angulos 0 y 360
    {
        double rotacionFinal = rotacionInicial.y + angulo;//rotacion a la que quiero llegar
        //Debug.Log("rotacion inicial " + rotacionInicial);
       // Debug.Log("rotacion final " + rotacionFinal);
        if (rotacionFinal > 360)
        {
            rotacionFinal = rotacionFinal - 360;
          //  Debug.Log("rotacion final modificado: " + rotacionFinal);
        }
        else if (rotacionFinal < 0)
        {
            rotacionFinal = 360 + rotacionFinal;
//            Debug.Log("rotacion final modificado 2: " + rotacionFinal);
        }
        double limSup = rotacionFinal + 2;
        double limInf = rotacionFinal - 2;
        if ((limInf <= posicionFinal.localRotation.eulerAngles.y) && (posicionFinal.localRotation.eulerAngles.y <= limSup))
        {         
            return true;
        }
        else
        {
            return false;
        }
    }
    public double distanciaEntreObjetos(Transform obj1, Transform obj2)
    {
        double distancia= Vector3.Distance(obj1.position,obj2.position);        
        return distancia;
    }
}