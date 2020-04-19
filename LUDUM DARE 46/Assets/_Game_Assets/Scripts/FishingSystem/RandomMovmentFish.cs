using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovmentFish : MonoBehaviour
{
	// Start is called before the first frame update

	public Vector3 target;
	public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target!= null)
		{
			//transform.LookAt(target);

			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(target.x, 0, target.z));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
			//transform.Find("FishSkin").rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right) * Quaternion.LookRotation(target);
			//transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right) * Quaternion.LookRotation

			target.y = 0;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
		}


    }
}
