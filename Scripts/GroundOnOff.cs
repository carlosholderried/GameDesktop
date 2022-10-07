using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundOnOff : MonoBehaviour
{
	public GameObject blocos; 
	
    // Start is called before the first frame update
    void Start()
    {
        blocos.SetActive(false);    
    }
	
	public void BarrierOff()
	{
		blocos.SetActive(true);
	}
	
}
