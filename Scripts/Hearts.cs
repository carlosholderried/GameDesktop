using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    /*
    public Transform player;
    public Transform hearts;
       
    public void Update()
    {
        hearts.transform.position = player.transform.position;
    }
    */

    public void checkHearts(int cont) 
    {       
      Destroy(GetComponent<Transform>().GetChild(cont).gameObject);                                                        
    }
}
