
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class ScreenToolTip : MonoBehaviour
{

	public static ScreenToolTip inctanse;

	private TextMeshProUGUI Tmp;
	private float timer;
	private bool reset;
	private Vector3 targetscale;
	private float p_duration;
	private int vebrato;
	private float esr;
	private Ease scaleEaseType;

	private void Awake() => inctanse = this;



	// Start is called before the first frame update
	void Start()
    {
		Tmp = GetComponent<TextMeshProUGUI>();

	}

    // Update is called once per frame
    void Update()
    {
		if(timer <= 0)
		{
			if (!reset)
			{
				transform.DOScale(0, .5f);
				Tmp.text = ""; reset = true;
			}


		}
		else
		{
			timer -= Time.deltaTime;
			

		}

	}


	public  void SetTooltip(string p_text)
	{

		reset = false;
		Tmp.text = p_text;
		timer = 3f;
		transform.DOScale(1, .5f).OnComplete(()=>punch());



	}

	void punch()
	{

		transform.DOPunchScale(targetscale, p_duration, vebrato, esr).SetEase(scaleEaseType);

	}

	
}
