using System.Collections;
using System;
using UnityEngine;
public class RedNeuronal: MonoBehaviour
{
    private double[,] pesosEntradaOculto = new double[8, 9];//8 entradas, 9 nodos ocultos
    private double[,] pesosOcultoSalida = new double[9, 2];//9 ocultos, 2 nodos salida
    private double[] sesgosOcultos = new double[9];
    private double[] sesgosSalidas = new double[2];
    private double[] salidaOculto = new double[9];
    private double[] salidaRed = new double[2];
    private double[,] deltaOcultoSalida = { { 0.011, 0.011 }, { 0.011, 0.011 }, { 0.011, 0.011 }, { 0.011, 0.011 }, { 0.011, 0.011 }, { 0.011, 0.011 }, { 0.011, 0.011 }, { 0.011, 0.011 }, { 0.011, 0.011 } };
    private double[,] deltaEntradaOculto = { { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 }, { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 }, { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 }, { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 }, { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 }, { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 }, { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 }, { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 } };
    private double[] deltaSesgoOculto = { 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011, 0.011 };
    private double[] deltaSesgoSalida = { 0.011, 0.011 };
    public DatosEntrenamiento datos = new DatosEntrenamiento();
    public void Start()
    {
        inicializarPesosYSesgos();
    }
    public void Update() { }
    private void inicializarPesosYSesgos()
    {
        System.Random aleatorio = new System.Random(0);
        for (int f = 0; f < 8; f++)// se recorre las filas de los pesos entrada-oculto
        {
            for (int c = 0; c < 9; c++)//se recorre las columnas
            {
                pesosEntradaOculto[f, c] = Math.Round((-1 + 2 * aleatorio.NextDouble()), 3);
            }
        }
        for (int o = 0; o < 9; o++)//sesgos nodos ocultos
        {
            sesgosOcultos[o] = Math.Round((-1 + 2 * aleatorio.NextDouble()), 3);
        }
        for (int f = 0; f < 9; f++)// se recorre las filas de los pesos oculto-salida
        {
            for (int c = 0; c < 2; c++)//se recorre las columnas
            {
                pesosOcultoSalida[f, c] = Math.Round((-1 + 2 * aleatorio.NextDouble()), 3);
            }
        }
        for (int o = 0; o < 2; o++)//sesgos nodos salida
        {
            sesgosSalidas[o] = Math.Round((-1 + 2 * aleatorio.NextDouble()), 3);
        }
    }
    public double[,] GetPesosEO()
    {
        return pesosEntradaOculto;
    }
    public double[] getSesgosOculto()
    {
        return sesgosOcultos;
    }
    public double[,] GetPesosOS()
    {
        return pesosOcultoSalida;
    }
    public double[] getSesgosSalida()
    {
        return sesgosSalidas;
    }
    public void computarSalida(string[] entrada)//8 entradas
    {
        double[] entradaN = new double[8];
        entradaN = datos.normalizarDatos(datos.codificarDatos(entrada));
        //computar la salida de la capa oculta con la funcion tanh()
        for (int o = 0; o < 9; o++)//nodo oculto
        {
            for (int e = 0; e < 8; e++)
            {
                salidaOculto[o] = entradaN[e] * pesosEntradaOculto[e, o] + salidaOculto[o];
            }
            salidaOculto[o] = salidaOculto[o] + sesgosOcultos[o];
            //aplico la funcion de activacion
            if (salidaOculto[o] < -20.0)
            {
                salidaOculto[o] = -1.0;
            }
            else if (salidaOculto[o] > 20.0)
            {
                salidaOculto[o] = 1.0;
            }
            else
            {
                salidaOculto[o] = Math.Tanh(salidaOculto[o]);
            }
        }
        //se computa la salida de la capa de salida
        for (int s = 0; s < 2; s++)// nodos salida
        {
            for (int o = 0; o < 9; o++)//nodos ocultos
            {
                salidaRed[s] = salidaOculto[o] * pesosOcultoSalida[o, s] + salidaRed[s];
            }
            salidaRed[s] = salidaRed[s] + sesgosSalidas[s];
            if (salidaRed[s] < -45.0)
                salidaRed[s] = 0.0;
            else if (salidaRed[s] > 45.0)
                salidaRed[s] = 1.0;
            else
                salidaRed[s] = 1.0 / (1.0 + Math.Exp(-salidaRed[s]));
        }
    }
    private void computarSalidaEntrenamiento(double[] entrada)//9 entradas
    {
        //computar la salida de la capa oculta con la funcion tanh()
        for (int o = 0; o < 9; o++)//nodo oculto
        {
            for (int e = 0; e < 8; e++)//capa de entrada
            {
                salidaOculto[o] = entrada[e] * pesosEntradaOculto[e, o] + salidaOculto[o];
            }
            salidaOculto[o] = salidaOculto[o] + sesgosOcultos[o];
            //aplico la funcion de activacion
            if (salidaOculto[o] < -20.0)
            {
                salidaOculto[o] = -1.0;
            }
            else if (salidaOculto[o] > 20.0)
            {
                salidaOculto[o] = 1.0;
            }
            else
            {
                salidaOculto[o] = Math.Tanh(salidaOculto[o]);
            }
        }
        //se computa la salida de la capa de salida
        for (int s = 0; s < 2; s++)// nodos salida
        {
            for (int o = 0; o < 9; o++)//nodos ocultos
            {
                salidaRed[s] = salidaOculto[o] * pesosOcultoSalida[o, s] + salidaRed[s];
            }
            salidaRed[s] = salidaRed[s] + sesgosSalidas[s];
            if (salidaRed[s] < -45.0)
                salidaRed[s] = 0.0;
            else if (salidaRed[s] > 45.0)
                salidaRed[s] = 1.0;
            else
                salidaRed[s] = 1.0 / (1.0 + Math.Exp(-salidaRed[s]));
        }
    }
    public void propagacionHaciaAtras(double[] esperado, double tasaAprendizaje, double momentum, double[] entradaN)
    {       
        double[] gradienteSalida = new double[2];
        double[] gradienteOculto = new double[9];
        for (int s=0;s<2;s++)//se calcula la gradiente del nodo de salida
        {
            gradienteSalida[s] = (esperado[s] - salidaRed[s]) * (salidaRed[s] * (1 - salidaRed[s]));
        }
        //se calcula la gradiente de los nodos ocultos
        for(int o = 0 ; o < 9 ; o++)
        {
            gradienteOculto[o] = (1-salidaOculto[o])*(1 + salidaOculto[o]);
            double sum = 0;
            for(int s=0;s < 2;s++)
            {
                sum = gradienteSalida[s] * pesosOcultoSalida[o,s] + sum;
            }
            gradienteOculto[o] = gradienteOculto[o] * sum;
        }
        //se calcula delta y se actualizan los pesos de entre las capas oculto - salida
        for (int o = 0; o < 9; o++)
        {
            deltaOcultoSalida[o, 0] = tasaAprendizaje * gradienteSalida[0] * salidaOculto[o] + (momentum * deltaOcultoSalida[o, 0]);
            deltaOcultoSalida[o, 1] = tasaAprendizaje * gradienteSalida[1] * salidaOculto[o] + (momentum * deltaOcultoSalida[o, 1]);

            pesosOcultoSalida[o, 0] = Math.Round((pesosOcultoSalida[o, 0] + deltaOcultoSalida[o, 0]), 3);
            pesosOcultoSalida[o, 1] = Math.Round((pesosOcultoSalida[o, 1] + deltaOcultoSalida[o, 1]), 3);
        }
        //se actualizan los sesgos de la capa de salida
        for(int s=0;s<2;s++)
        {
            deltaSesgoSalida[s] = tasaAprendizaje * gradienteSalida[s] + (momentum* deltaSesgoSalida[s]);
            sesgosSalidas[s] = Math.Round((sesgosSalidas[s] + deltaSesgoSalida[s]), 3);
        }
        //se calcula delta y se actualizan los pesos de entre las capas entrada - oculto
        for (int e = 0; e < 8; e++)
        {
            for (int o = 0; o < 9; o++)
            {
                deltaEntradaOculto[e, o] = tasaAprendizaje * gradienteOculto[o] * entradaN[e] + (momentum * deltaEntradaOculto[e, o]);
                pesosEntradaOculto[e, o] = Math.Round((pesosEntradaOculto[e, o] + deltaEntradaOculto[e, o]), 3);
            }
        }
        //se actualizan los sesgos de la capa oculta
        for (int o = 0; o < 9; o++)
        {
            deltaSesgoOculto[o] = tasaAprendizaje * gradienteOculto[o] + (momentum * deltaSesgoOculto[o]);
            sesgosOcultos[o] = Math.Round((sesgosOcultos[o] + deltaSesgoOculto[o]), 3);
        }
    }
    public void entrenamiento(int limiteIteraciones)
    {
       // Debug.Log("pesos y sesgos iniciales: ");
        //mostrarPesosYSesgos();
        //Debug.Log("------------------------");
        int con = 0;
        double[] error = new double[datos.datosE.Count];
        double promedio = 0;
        bool mediaError = false;
        double[] esperado = new double[2];
        while (con < limiteIteraciones && mediaError == false )//|| promedio < 0.20)
        {
            int[] indices = datos.barajarIndices(datos.datosE);           
            for (int f = 0; f < indices.Length; f++)
            {                
                computarSalidaEntrenamiento(datos.datosE[f]);// el resultado se guarda en la variable salida red
                //Debug.Log("salida oculto: " + salidaOculto[0] + " " + salidaOculto[1] + " " + salidaOculto[2] + " " + salidaOculto[3] + " " + salidaOculto[4] + " " + salidaOculto[5] + " " + salidaOculto[6] + " " + salidaOculto[7] + " " + salidaOculto[8]);
                //Debug.Log("salida red: " + salidaRed[0] + " " + salidaRed[1]);               
                if ((datos.datosE[f][8]) == 0)
                {
                    esperado[0] = 0;
                    esperado[1] = 1;                    
                }
                else if (datos.datosE[f][8] == 1)
                {
                    esperado[0] = 1;
                    esperado[1] = 0;                  
                }
                //Debug.Log("salida red: " + salidaRed);
                //Debug.Log("valor esperado: " + esperado);
                propagacionHaciaAtras(esperado, 0.15, 0.05, datos.datosE[f]);
                error[f] = errorCuadrado(esperado, salidaRed);                
            }
            promedio = errorPromedio(error);
            if (promedio <0.06)
            {
                mediaError = true;
            }
           // Debug.Log("error media cuadrada: " + promedio);
            con++;           
        }
        //Debug.Log("pesos y sesgos actualizados: ");
        mostrarPesosYSesgos();
    }
    public double exactitud()
    {
        double numCorrectos = 0;
        double porcentaje = 0;
        double[] esperado = new double[2];
        foreach (double[] dato in datos.datosP)
        {
            computarSalidaEntrenamiento(dato);
            if ((dato[8]) == 0)
            {
                esperado[0] = 0;
                esperado[1] = 1;
            }
            else if (dato[8] == 1)
            {
                esperado[0] = 1;
                esperado[1] = 0;
            }
            int[] aux = normalizarSalida(salidaRed);
            if ((esperado[0] == aux[0]) && (esperado[1] == aux[1]))
            {
                numCorrectos++;
            }
            Debug.Log("salida red: " + salidaRed[0] + " " + salidaRed[1]);
            Debug.Log("salida red normalizado: " + aux[0] + " " + aux[1]);
            Debug.Log("valor esperado: " + esperado[0] + " " + esperado[1]);
            Debug.Log("__________________________");
        }
        porcentaje = Math.Round((numCorrectos / datos.datosP.Count)*100,3);
        return porcentaje;
    }
    private double errorPromedio(double[] error)
    {
        double res = 0;
        for (int f=0;f<error.Length;f++)
        {
            res = error[f] + res;
        }
        res = res / error.Length;
        return res;
    }
    private double errorCuadrado(double[] esperado, double[] obtenido)
    {
        double res = 0;
        res = Math.Pow((esperado[0] - obtenido[0]), 2) + Math.Pow((esperado[1] - obtenido[1]), 2);
        return res;
    }
    public void mostrarPesosYSesgos()
    {
        for (int f = 0; f < 8; f++)
        {
            Debug.Log("pesos E-O: " + pesosEntradaOculto[f, 0] + " " + pesosEntradaOculto[f, 1] + " " + pesosEntradaOculto[f, 2] + " " + pesosEntradaOculto[f, 3] + " " + pesosEntradaOculto[f, 4] + " " + pesosEntradaOculto[f, 5] + " " + pesosEntradaOculto[f, 6] + " " + pesosEntradaOculto[f, 7] + " " + pesosEntradaOculto[f, 8]);
        }
        Debug.Log("--------------------------------------------------------");
        Debug.Log("sesgos O: " + sesgosOcultos[0] + " " + sesgosOcultos[1] + " " + sesgosOcultos[2] + " " + sesgosOcultos[3] + " " + sesgosOcultos[4] + " " + sesgosOcultos[5] + " " + sesgosOcultos[6] + " " + sesgosOcultos[7] + " " + sesgosOcultos[8]);
        Debug.Log("--------------------------------------------------------");
        for (int f = 0; f < 9; f++)
        {
            Debug.Log("pesos O-S: " + pesosOcultoSalida[f, 0] + " " + pesosOcultoSalida[f, 1]);
        }
        Debug.Log("--------------------------------------------------------");
        Debug.Log("sesgos S: " + sesgosSalidas[0] + " " + sesgosSalidas[1]);
        Debug.Log("--------------------------------------------------------");
    }
    public int[] normalizarSalida(double[] salida)
    {
        int[] res = new int[2];
        /*for (int i = 0; i < 2; i++)
        {
            if (salida[i] >= 0.5)
            {
                res[i] = 1;
            }
            else
            {
                res[i] = 0;
            }
        }*/
        if (salida[0] > salida[1])
        {
            res[0] = 1;
            res[1] = 0;
        }
        else
        {
            res[0] = 0;
            res[1] = 1;
        }
        return res;
    }
    public double[] getSalida()
    {
        return salidaRed;
    }
}
