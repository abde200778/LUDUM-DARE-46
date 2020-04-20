using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huntrox.Games.LD46;

public class FishingSpot : MonoBehaviour
{

	[SerializeField] private int fishcount;
	[SerializeField] private float timer;
	[SerializeField] private float _raduis;
	[SerializeField] private bool _isfishingable;
	[SerializeField] private bool MainMenu;

	public bool isfishingable { get { return _isfishingable; } }

	[SerializeField] private GameObject[] _fishesPrefab;
	[SerializeField] private List<GameObject> _fishes = new List<GameObject>();
	GameObject m_go;



	// Start is called before the first frame update
	void Start()
	{
		GetComponent<SphereCollider>().radius = _raduis;
		Init();
	}

	// Update is called once per frame
	void Update()
	{
		if (timer >= 0)
		{
			timer -= Time.deltaTime;

		}
		else
		{
			//resetMethod
			_isfishingable = false;
			for (int i = 0; i < _fishes.Count; i++)
			{
				GameObject t_go = _fishes[i];
				_fishes.Remove(t_go);
				Destroy(t_go);


			}
		}

		if(!_isfishingable)
		{
			SpawnHandler.inctanse.Respawn(gameObject,false);
			_isfishingable = true;
		}
	}


	public void Init()
	{
		if (MainMenu)
			timer = 6000f;
		else
			timer = Random.Range(180f, 300f);
		fishcount = Random.Range(4, 11);
		_isfishingable = true;
		spawnfishes();
	
		StartCoroutine(RandomMovment());
	}









	void spawnfishes()
	{


		for (int i = 0; i < fishcount; i++)
		{
			GameObject fish = Instantiate(_fishesPrefab[Random.Range(0, _fishesPrefab.Length)], transform.position, Quaternion.identity, transform);
			float newScale = Random.Range(0.2f, .5f);
			fish.transform.localScale = fish.transform.localScale * newScale;
			Vector3 randomposition = Random.insideUnitSphere * _raduis;

			//randomposition.z = randomposition.y;
			fish.GetComponent<RandomMovmentFish>().target = randomposition;
			fish.GetComponent<RandomMovmentFish>().speed = Random.Range(3, 5);
			_fishes.Add(fish);
		}

	}

	IEnumerator RandomMovment()
	{


		while (true)
		{
			for (int i = 0; i < _fishes.Count; i++)
			{

				Vector3 randomposition = Random.insideUnitSphere * _raduis;
				//randomposition.z = randomposition.y;

				_fishes[i].GetComponent<RandomMovmentFish>().target = randomposition;
				_fishes[i].GetComponent<RandomMovmentFish>().speed = Random.Range(3, 5);
				yield return new WaitForSeconds(0.05f);
				//_fishes[i].transform.localPosition = Vector3.MoveTowards(_fishes[i].transform.localPosition, randomposition, Random.Range(3, 5) * Time.deltaTime);
				//_fishes[i].transform.LookAt(randomposition);
			}

			if(fishcount <= 0)
			{
				_isfishingable = false;
				// reset spot
			}


			yield return new WaitForSeconds(.7f);
		}

	}







	public void removeFish(int state)
	{
		switch (state)
		{
			case 0:
				m_go = _fishes[Random.Range(0, _fishes.Count)];
				m_go.SetActive(false);
				break;
			case 1:

				if (m_go != null)
					m_go.SetActive(true);
				break;
			case 2:
				fishcount--;
				if (m_go != null) _fishes.Remove(m_go); Destroy(m_go);
				break;
			case 3:
				GameObject t_go = _fishes[Random.Range(0, _fishes.Count)];
				if (fishcount >0 ) _fishes.Remove(t_go); Destroy(t_go);
				fishcount--;
				break;
		}

	}

		private void OnTriggerEnter(Collider other)
		{

			if (other.tag == "Ground")
			{

				SpawnHandler.inctanse.Respawn(gameObject,true);

			}else if (other.tag == "FishingSpot")
			SpawnHandler.inctanse.Respawn(gameObject, true);


     	}

	private void OnTriggerStay(Collider other)
	{

		if (other.CompareTag( "Ground"))
		{

			SpawnHandler.inctanse.Respawn(gameObject, true);

		}
		else if (other.CompareTag( "FishingSpot"))
			SpawnHandler.inctanse.Respawn(gameObject, true);


	}





	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _raduis);
	}
}
