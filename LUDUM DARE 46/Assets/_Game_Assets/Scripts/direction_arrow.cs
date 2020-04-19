using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class direction_arrow : MonoBehaviour
{


	[SerializeField]private Transform target;
	[SerializeField]private Transform playertarget;
    // Start is called before the first frame update
    void Start()
    {
		playertarget = GameObject.Find("Character").transform;


	}










    // Update is called once per frame
    void Update()
    {


		transform.position = playertarget.position;

		if (target != null)
		{

			Vector3 direction = (target.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

		}

        
    }

	public void SetTarget(Transform p_transform)
	{

		target = p_transform;
	}

}
