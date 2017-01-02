using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class prueba : MonoBehaviour
{
    private string tecla= "space";
	// Use this for initialization
	void Start ()
    {
	
	}
   /* void OnGUI()
    {
        GUI.Label(new Rect(25, 15, 100, 30), "etiqueta");
    }*/
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(tecla))
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
	}
}
