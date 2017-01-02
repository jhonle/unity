using UnityEngine;
using System.Collections;

public class ExtensionPersonalizadaMonoBehavior : MonoBehaviour
{
// This class is used to add some common variables to
// MonoBehavior, rather than constantly repeating
// the same declarations in every class.
  public Transform myTransform;
  public GameObject myGO;
  public Rigidbody myBody;
  public bool didInit;
  public bool canControl;
  public int id;
  public Vector3 tempVEC;  
  public Transform tempTR;
  
  public virtual void SetID( int anID )
  {
     id= anID;
  }
}