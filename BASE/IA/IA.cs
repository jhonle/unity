using UnityEngine;
using System;
using System.Collections;
public class IA : MonoBehaviour
{
    public GameObject proyectil;
    public Transform objetivo;
    public Transform canion;
    private Rigidbody tanque;

    private AudioSource explosionTanque;
    private DateTime ultimoDisparo;
    private NavMeshAgent agente;
    private Vector3 adelante;
    private Vector3 atras;
    private Quaternion rotacion;

    private float fuerza = 50f;
    private float girarDerecha;
    private float girarIzquierda;

    private int primerDisparo = 0;
    private Estrategias estrategia;
    private RedNeuronal red;
    private ControlPosiciones posiciones;
    private void Start()
    {
        tanque = GetComponent<Rigidbody>();
        agente = GetComponent<NavMeshAgent>();
        explosionTanque = GetComponent<AudioSource>();
        agente = GetComponent<NavMeshAgent>();
        girarDerecha = 120f * Time.deltaTime;
        girarIzquierda = -120f * Time.deltaTime;
        estrategia = new Estrategias();
        posiciones = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControlPosiciones>();
        red = GameObject.FindGameObjectWithTag("red").GetComponent<RedNeuronal>();
    }
    private void Awake()
    {
        // red.entrenamiento(100);
        //red.exactitud();
        //red.mostrarPesosYSesgos();
    }
    private void Update()
    {
        if (estrategia.GetEstrategia() == false)// no hay una estrategia activa
        {
            estrategia.nombre = escogerEstrategia();
            estrategia.SetEstrategia(true);
        }
        else
        {
            switch (estrategia.nombre)
            {
                case "buscarYDisparar":
                    if (EstrategiaBuscarYDisparar() == true)
                    {
                        estrategia.SetEstrategia(false);
                    }
                    break;
                case "escondereseDisparar":
                    if (EstrategaEsconderseDisparar() == true)
                    {
                        estrategia.SetEstrategia(false);
                    }
                    break;
                case "disparoSincronizado":
                    if (EstrategiaDisparoSincronizado() == true)
                    {
                        estrategia.SetEstrategia(false);
                    }
                    break;
                case "esquivar":
                    if (EstrategiaEsquivar() == true)
                    {
                        estrategia.SetEstrategia(false);
                    }
                    break;
                case "buscarBosque":
                    if (EstrategaEsconderseBosque() == true)
                    {
                        estrategia.SetEstrategia(false);
                    }
                    break;
                case "buscarPared":
                    if (EstrategaEsconderseMuro() == true)
                    {
                        estrategia.SetEstrategia(false);
                    }
                    break;
            }
        }
    }
    private void BuscarObjetivo(Vector3 obj)
    {
        agente.destination = obj;
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
    private bool Avanzar(double distancia)
    {
        if (estrategia.getPosicionInicial() == false)
        {
            estrategia.setPosicionInicial(transform);
            CambioControlManual();
            return false;
        }
        else if (!estrategia.distanciaRecorrida(transform, distancia))
        {
            adelante = transform.forward * 10f * Time.deltaTime;
            tanque.MovePosition(transform.position + adelante);
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool Retroceder(double distancia)
    {
        if (estrategia.getPosicionInicial() == false)
        {
            estrategia.setPosicionInicial(GetComponent<Transform>());
            CambioControlManual();
        }
        if (!estrategia.distanciaRecorrida(GetComponent<Transform>(), distancia))
        {
            adelante = transform.forward * -10f * Time.deltaTime;
            tanque.MovePosition(transform.position + adelante);
            return false;
        }
        else
        {
            CambioControlAgente();
            //estrategia.reiniciarPocisionRotacion();
            return true;
        }
    }
    private bool Girar(double angulo)
    {
        if (estrategia.getRotacionInicial() == false)
        {
            CambioControlManual();
            estrategia.setRotacionInicial(transform);
            return false;
        }
        else if (estrategia.rotacion(transform, angulo) == false)
        {
            if (angulo >= 0)
            {
                rotacion = Quaternion.Euler(0f, girarDerecha, 0f);
            }
            else
            {
                rotacion = Quaternion.Euler(0f, girarIzquierda, 0f);
            }
            tanque.MoveRotation(tanque.rotation * rotacion);
            return false;
        }
        else
        {
            return true;
        }
    }
    private void Disparar()
    {
        if (primerDisparo == 0)
        {
            GameObject bala = Instantiate(proyectil, canion.position, canion.rotation) as GameObject;
            bala.GetComponent<Rigidbody>().velocity = fuerza * canion.forward;
            ultimoDisparo = DateTime.Now;
            primerDisparo = 1;
        }
        else if (DateTime.Now >= (ultimoDisparo + TimeSpan.FromSeconds(1)))
        {
            GameObject bala = Instantiate(proyectil, canion.position, canion.rotation) as GameObject;
            bala.GetComponent<Rigidbody>().velocity = fuerza * canion.forward;
            ultimoDisparo = DateTime.Now;
        }
    }
    public void Destruir()
    {
        explosionTanque.Play();
        Destroy(gameObject, explosionTanque.clip.length);
    }
    private bool proyectilCercano()
    {
        bool cerca = false;
        GameObject[] proyectiles = GameObject.FindGameObjectsWithTag("proyectil");
        if (proyectiles != null)
        {
            foreach (GameObject p in proyectiles)
            {
                double distancia = estrategia.distanciaEntreObjetos(transform, p.transform);
                if (distancia < 30)
                {
                    cerca = true;
                    break;
                }
            }
        }
        return cerca;
    }
    private double apuntar()
    {
        Vector3 vector = objetivo.position - transform.position;
        float angulo = Vector3.Angle(vector, transform.forward);
        Vector3 direccion = Vector3.Cross(vector, transform.forward);
        // Debug.Log("angulo : " + angulo);
        //Debug.Log("direccion : " + direccion.y);
        if (direccion.y > 0)
        {
            angulo = -angulo;
        }
        return angulo;
    }
    private double distanciaObjetivo(Vector3 obj)
    {
        float x = obj.x - transform.position.x;
        float z = obj.z - transform.position.z;
        double distancia = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));
        return distancia;
    }
    private bool EstrategiaEsquivar()
    {
        if (Girar(60))
        {
            if (Avanzar(20))
            {
                estrategia.reiniciarPocisionRotacion();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private bool EstrategiaBuscarYDisparar()
    {
        if (estrategia.getPrimerIngreso() == false)
        {
            CambioControlAgente();
            BuscarObjetivo(objetivo.position);
            estrategia.setPrimerIngreso(true);
            estrategia.setPosicionInicial(objetivo.position);
            agente.stoppingDistance = 20;
            //Debug.Log("primer ingreso: "+ estrategia.getVector());
            return false;
        }
        else if (distanciaObjetivo(estrategia.getVector()) <= 30)//if (agente.stoppingDistance > agente.remainingDistance)
        {
            if (estrategia.getApuntar() == false)
            {
                //Debug.Log("siguiente ingreso apuntando: ");
                estrategia.setAngulo(apuntar());
                agente.Stop();
                return false;
            }
            else if (Girar(estrategia.getAngulo()))
            {
                Disparar();
                estrategia.reiniciarPocisionRotacion();
                estrategia.setPrimerIngreso(false);
                estrategia.setApuntar(false);
                // Debug.Log("disparo: ");
                return true;
            }
            else
            {
                //Debug.Log("girando: " + estrategia.getAngulo());
                return false;
            }
        }
        else
        {
            // Debug.Log("buscando: ");
            BuscarObjetivo(estrategia.getVector());// da problemas cuando despues de dispara se vuleve a mover el jugador
            return false;
        }
    }
    private bool EstrategaEsconderseMuro()
    {
        if (estrategia.getPrimerIngreso() == false)
        {
            CambioControlAgente();
            estrategia.setMuro(posiciones.getMuroCercano(transform));
            Vector3 posicion = posiciones.atrasMuro(estrategia.getMuro(), objetivo);
            Debug.Log("poscion destino: " + posicion);
            BuscarObjetivo(posicion);
            estrategia.setPrimerIngreso(true);
            agente.stoppingDistance = 1;
            return false;
        }
        else if (distanciaObjetivo(estrategia.getMuro().position) <= 1)
        {
            estrategia.reiniciarPocisionRotacion();
            estrategia.setPrimerIngreso(false);
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool EstrategaEsconderseBosque()
    {
        if (estrategia.getPrimerIngreso() == false)
        {
            CambioControlAgente();
            Vector3 bosque = posiciones.getBosqueCercano(transform);
            Debug.Log("poscion destino: " + bosque);
            estrategia.setBosque(bosque);
            BuscarObjetivo(bosque);
            estrategia.setPrimerIngreso(true);
            agente.stoppingDistance = 5;
            return false;
        }
        if (Vector3.Distance(transform.position, estrategia.getbosque()) <= 5)
        {

            estrategia.reiniciarPocisionRotacion();
            estrategia.setPrimerIngreso(false);
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool EstrategaEsconderseDisparar()
    {
        if (estrategia.getPrimerIngreso() == false)
        {
            CambioControlAgente();
            Vector3 bosque = posiciones.getBosqueCercano(transform);
            estrategia.setBosque(bosque);
            BuscarObjetivo(bosque);
            estrategia.setPrimerIngreso(true);
            agente.stoppingDistance = 5;
            return false;
        }
        if (Vector3.Distance(transform.position, estrategia.getbosque()) <= 5)
        {

            if (estrategia.getApuntar() == false)
            {
                estrategia.setAngulo(apuntar());
                agente.Stop();
                return false;
            }
            else if (Girar(estrategia.getAngulo()))
            {
                Disparar();
                estrategia.reiniciarPocisionRotacion();
                estrategia.setPrimerIngreso(false);
                estrategia.setApuntar(false);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
    private bool EstrategiaDisparoSincronizado()
    {
        return true;
    }
    private string escogerEstrategia()
    {
        string[] entrada = new string[8];

        GameObject[] proyectiles = GameObject.FindGameObjectsWithTag("proyectil");
        if (proyectiles.Length >= 1)//Proyectil cercano 
        {
            entrada[0] = "si";
        }
        else
        {
            entrada[0] = "no";
        }
        GameObject[] aliados = GameObject.FindGameObjectsWithTag("IA");
        if (aliados.Length > 1)//Nro.De aliados
        {
            entrada[1] = "" + (aliados.Length - 1);
        }
        else
        {
            entrada[1] = "0";
        }
        entrada[2] = "" + distanciaObjetivo(objetivo.position);//Distancia respecto del jugador 
        entrada[3] = "" + posiciones.getNroMuros();// Nro.De obstáculos        
        entrada[4] = "" + distanciaObjetivo(posiciones.getMuroCercano(transform).position);//Distancia obstáculo más cercano
        entrada[5] = "50"; //Tiempo promedio de destrucción de tanques
        entrada[6] = "pesado";// Modelo de tanque 
        entrada[7] = "" + Vector3.Distance(transform.position, posiciones.getBosqueCercano(transform));//Distancia escondite más cercano

        //Debug.Log("entrada: " + entrada[0] + " " + entrada[1] + " " + entrada[2] + " " + entrada[3] + " " + entrada[4] + " " + entrada[5] + " " + entrada[6] + " " + entrada[7]);        
        red.computarSalida(entrada);
        double[] salida = red.getSalida();
        //Debug.Log("salida : " + salida[0] +" "+ salida[1]);
        int[] s = red.normalizarSalida(salida);
        //Debug.Log("salida normalizada: "+s[0] + " " + s[1]);
        string e = "";
        if (s[0] == 0 && s[1] == 1)
        {
            e = "defensiva";
        }
        else if (s[0] == 1 && s[1] == 0)
        {
            e = "ofensiva";
        }
        string a = seleccionarEstrategia(e);
        Debug.Log("estrategia a tomar: " + a);
        return a;
    }
    private string seleccionarEstrategia(String est)
    {
        string salida = "";
        System.Random aleatorio = new System.Random();
        double r = aleatorio.NextDouble();
        //Debug.Log("random: " + r);
        if (est.Equals("ofensiva"))
        {
            if (r <= 0.40)
            {
                salida = "buscarYDisparar";
            }
            else if (r > 0.40 && r <= 0.80)
            {
                salida = "escondereseDisparar";
            }
            else if (r > 0.80)
            {
                salida = "disparoSincronizado";
            }
        }
        else if (est.Equals("defensiva"))
        {
            if (r <= 0.30)
            {
                salida = "esquivar";
            }
            else if (r > 0.30 && r <= 0.60)
            {
                salida = "buscarBosque";
            }
            else if (r > 0.60)
            {
                salida = "buscarPared";
            }
        }
        return salida;
    }
    public void destruir()
    {
        /*colision.enabled = false;
        caidaMuro.Play();
        animacion.Play();
        Destroy(obstaculo);
        ladrillo.enabled = false;
        Destroy(gameObject, animacion.clip.length);*/
    }
}
