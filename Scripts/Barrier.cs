using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
	public GameObject barrier; // aa

	// Start is called before the first frame update
	void Start()
	{
		//barrier.SetActive(true);
	}

	public void BarrierOff()
	{
		barrier.SetActive(false);
	}
}
