using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huntrox.Games.LD46
{

	public class SpawnHandler : MonoBehaviour
	{
		public static SpawnHandler inctanse;
		[SerializeField] private float _Boatsraduis;
		[SerializeField] private float _raduis;
		[SerializeField] private int ammount;
		[SerializeField] private GameObject fishSpotprefab;
		[SerializeField] private GameObject fishermanPrefab;
		[SerializeField] private Transform fisherman_startingpoint;


		void Awake() => inctanse = this;






		void Start()
		{
			//	fishspotSpawnPos = transform;//GameObject.Find("fishspotSpawnPos").transform;
			fisherman_startingpoint = GameObject.Find("fisherman_startingpoint").transform;
			
		}


		public void SpawnFishspots()
		{

			for (int i = 0; i < ammount; i++)
			{
				Vector3 randomposition = Random.insideUnitSphere * _raduis;
				randomposition.y = 0;
				GameObject tmp_go = Instantiate(fishSpotprefab, transform.position, Quaternion.identity, transform);
				tmp_go.transform.localPosition = randomposition;
				tmp_go.GetComponent<FishingSpot>().Init();
			}

		}


		public void Respawn(GameObject p_go,bool GroundDetected)
		{

			Vector3 randomposition = Random.insideUnitSphere * _raduis;
			randomposition.y = 0;
			p_go.transform.localPosition = randomposition;
			if (!GroundDetected)
			{
			p_go.GetComponent<FishingSpot>().Init();

			}

		}

		public void SpawnFisherman(int unit_id)
		{

		GameObject t_go=Instantiate(fishermanPrefab, fisherman_startingpoint.position, Quaternion.identity);
		t_go.GetComponent<NpcFisherman>().Init(unit_id);

		}
		public Vector3 GetRandomPosition()
		{


			Vector3 randomposition = Random.insideUnitSphere * _Boatsraduis;
			randomposition = transform.TransformPoint(randomposition);
			return randomposition;
		}
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, _raduis);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, _Boatsraduis);
		}

	

	}


}