using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huntrox.Games.LD46;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{

	[SerializeField] private int mapindex;
	// Start is called before the first frame update

	private void Awake()
	{
		SceneManager.LoadSceneAsync(mapindex,LoadSceneMode.Additive);
	}

	void Start()
    {
		SceneManager.LoadSceneAsync(mapindex);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
