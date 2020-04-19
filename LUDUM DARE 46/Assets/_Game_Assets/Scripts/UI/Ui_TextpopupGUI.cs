using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class Ui_TextpopupGUI : MonoBehaviour
{

	private bool m_bInit = false;

	private TextMeshProUGUI tmp;
	RectTransform rectTransform;


	void Start()
	{

	}

	public void Init(string popupTEXT, Color p_color)
	{

		rectTransform = GetComponent<RectTransform>();
		Destroy(this.gameObject, 1.8f);

		tmp = GetComponent<TextMeshProUGUI>();
		tmp.text = popupTEXT;
		tmp.color = p_color;

		transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f).OnComplete(ScaleDown);



		DOTween.PlayAll();

		m_bInit = true;
	}

	void Update()
	{
		
		if (m_bInit)
		{
			float textmeshalpha = tmp.color.a;
			tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, textmeshalpha -= (0.5f * Time.deltaTime));
		}
	}

	void ScaleDown()
	{
		float maxX = rectTransform.position.x + 20.5f;
		rectTransform.DOLocalMoveX(maxX, 2f);

		transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1.0f);
	}

}
