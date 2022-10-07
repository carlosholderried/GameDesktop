using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
	public GameObject barrier; 


	public void BarrierOff()
	{
		barrier.SetActive(false);
	}
}
