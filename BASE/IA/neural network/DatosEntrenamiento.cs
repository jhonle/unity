using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
public class DatosEntrenamiento
{
    public List<double[]> datosE = new List<double[]>();
    public List<double[]> datosP = new List<double[]>();

    public DatosEntrenamiento()
    {
        List<double[]> aux = cargarDatos();
        separarDatosEntrenamientoPruebas(aux);
        
    }
    public List<double[]> getDatosEntrenamiento()
    {
        return datosE;
    }
    private List<double[]> cargarDatos()
    {
        List<string[]> datos = new List<string[]>();
        List<double[]> datosC = new List<double[]>();
        List<double[]> datosN = new List<double[]>();
        StreamReader archivo = new StreamReader("Assets/Scripts/BASE/IA/neural network/datosEntrenamiento.txt");
        string linea = "";
        string[] informacion = new string[9];
        while ((linea=archivo.ReadLine()) != null)
        {
            informacion = linea.Split(' ');
            datos.Add(informacion);
        }
        archivo.Close();
        foreach(string[] dato in datos)
        {
            datosC.Add(codificarDatosE(dato));
        }
        foreach (double[] dato in datosC)
        {
            datosN.Add(normalizarDatosE(dato));
        }
        return datosN;
    }
    public double[] codificarDatosE(string[] dato)//9
    {
        double[] entrada= new double[9];                
        if (dato[0].Equals("si"))//proyectil cercano
        {
            entrada[0] = 1;
         }
        else
        {
            entrada[0] = -1;
        }
        entrada[1] = double.Parse(dato[1]);//nro de aliados
        entrada[2] = double.Parse(dato[2]); //distancia respecto del jugador
        entrada[3] = double.Parse(dato[3]);//nro de obstaculos
        entrada[4] = double.Parse(dato[4]);//distancia obstaculo mas cercano
        entrada[5] = double.Parse(dato[5]);// tiempo promedio de destruccion de tanques

        if (dato[6].Equals("ligero"))//modelo de tanque
        {
            entrada[6] = 1;
        }
        else
        {
            entrada[6] = -1;
        }
        entrada[7] = double.Parse(dato[7]);//distancia escondite mas cercano

        if (dato[8].Equals("defensiva"))//modelo de tanque
        {
            entrada[8] = 0;
        }
        else
        {
            entrada[8] = 1;
        }
        return entrada; 
    }
    public double[] normalizarDatosE(double[] codificado)//normalizacion min max
    {
        //si 0 50 9 7 60 pesado 5 defensiva
        //1 0 50 9 7 60 -1 5 0
        double[] normalizado = new double[9];       
        normalizado[0] = Math.Round(codificado[0],3);
        normalizado[1] = Math.Round((codificado[1]/4),3);
        normalizado[2] = Math.Round(((codificado[2] - 10) /90),3);
        normalizado[3] = Math.Round(((codificado[3] - 6) / 14),3);
        normalizado[4] = Math.Round(((codificado[4] - 5) / 95),3);
        normalizado[5] = Math.Round((codificado[5]/ 100),3);
        normalizado[6] = Math.Round(codificado[6],3);
        normalizado[7] = Math.Round(((codificado[7] - 5) / 95),3);
        normalizado[8] = Math.Round(codificado[8],3);

        return normalizado;        
    }
    public double[] codificarDatos(string[] dato)
    {
        double[] entrada = new double[8];
        if (dato[0].Equals("si"))//proyectil cercano
        {
            entrada[0] = 1;
        }
        else
        {
            entrada[0] = -1;
        }
        entrada[1] = double.Parse(dato[1]);//nro de aliados
        entrada[2] = double.Parse(dato[2]); //distancia respecto del jugador
        entrada[3] = double.Parse(dato[3]);//nro de obstaculos
        entrada[4] = double.Parse(dato[4]);//distancia obstaculo mas cercano
        entrada[5] = double.Parse(dato[5]);// tiempo promedio de destruccion de tanques

        if (dato[6].Equals("ligero"))//modelo de tanque
        {
            entrada[6] = 1;
        }
        else
        {
            entrada[6] = -1;
        }
        entrada[7] = double.Parse(dato[7]);//distancia escondite mas cercano
        
        return entrada;
    }
    public double[] normalizarDatos(double[] codificado)//normalizacion min max
    {
        //si 0 50 9 7 60 pesado 5 defensiva
        //1 0 50 9 7 60 -1 5 0
        double[] normalizado = new double[8];
        normalizado[0] = Math.Round(codificado[0], 3);
        normalizado[1] = Math.Round((codificado[1] / 4), 3);
        normalizado[2] = Math.Round(((codificado[2] - 10) / 90), 3);
        normalizado[3] = Math.Round(((codificado[3] - 6) / 14), 3);
        normalizado[4] = Math.Round(((codificado[4] - 5) / 95), 3);
        normalizado[5] = Math.Round((codificado[5] / 100), 3);
        normalizado[6] = Math.Round(codificado[6], 3);
        normalizado[7] = Math.Round(((codificado[7] - 5) / 95), 3);

        return normalizado;
    }
    public int[] barajarIndices(List<double[]> datos)
    {
        int[] indice = new int[datos.Count];
        for (int f=0;f<indice.Length;f++)
        {
            indice[f] = f;
        }
        System.Random aleatorio = new System.Random();

        for (int n = 0; n < datos.Count; n++)
        {
            int r = aleatorio.Next(n, datos.Count);
            int tmp = indice[r];
            indice[r] = indice[n];
            indice[n] = tmp;
        }
        return indice;
    }
    public void mostrarIndices(int[] indices)
    {
        string cadena = " indices: ";
        for (int f = 0; f < indices.Length; f++)
        {
            cadena = cadena + " " + indices[f];
        }
        Debug.Log(cadena);
    }
    public void mostrarDatos(double[] datos)
    {
        string cadena = " datos: ";
        for (int f = 0; f < datos.Length; f++)
        {
            cadena = cadena + " " + datos[f];
        }
        Debug.Log(cadena);
    }
    public void separarDatosEntrenamientoPruebas(List<double[]> datosN)
    {
        int indice = 0;
        int[] indices = barajarIndices(datosN);
        indice = (int)(datosN.Count * 0.8);
        for (int f=0; f<indice;f++)
        {
            datosE.Add(datosN[indices[f]]);
        }
        for (int f = indice; f < datosN.Count; f++)
        {
            datosP.Add(datosN[indices[f]]);
        }
    }
    public void mostrarDatosEP()
    {
        foreach(double[] datoE in datosE)
        {
            Debug.Log("dato de E: " + datoE[0] + " " + datoE[1] + " " + datoE[2] + " " + datoE[3] + " " + datoE[4] + " " + datoE[5] + " " + datoE[6] + " " + datoE[7] + " " + datoE[8]);
        }
        Debug.Log("-------------------------------------------");
        foreach (double[] datoP in datosP)
        {
            Debug.Log("dato de P: " + datoP[0] + " " + datoP[1] + " " + datoP[2] + " " + datoP[3] + " " + datoP[4] + " " + datoP[5] + " " + datoP[6] + " " + datoP[7] + " " + datoP[8]);
        }
        Debug.Log("-------------------------------------------");
    }
}