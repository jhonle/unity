using UnityEngine;
using System.Collections;

public class Gravedad : MonoBehaviour 
{
	public Vector3 gravedad = new Vector3(0,-12.81f,0);
	
	void Start () 
	{
 		Physics.gravity=gravedad;
 		this.enabled=false;
	}
}