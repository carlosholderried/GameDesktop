using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemFeedbackScript : MonoBehaviour
{
    // função para destruir as particulas dos pickups
    public void DestroyObject()
    {	
		// destroi o gameobject ao qual o script esta anexado
		Destroy(gameObject);
    }
}
