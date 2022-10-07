using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    public void checkHearts(int cont) 
    {       
      Destroy(GetComponent<Transform>().GetChild(cont).gameObject);                                                        
    }
}
