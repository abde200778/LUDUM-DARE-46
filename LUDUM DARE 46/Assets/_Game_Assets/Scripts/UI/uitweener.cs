using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class uitweener : MonoBehaviour
{

	[SerializeField] private Tween m_tween;
	[SerializeField] private Ease scaleEaseType;
	[SerializeField] private Ease rotationEaseType;
	[SerializeField] private Vector3 targetscale;
	[SerializeField] private float scaleDuration;
	[SerializeField] private float esr;
	[SerializeField] private int vebrato;
	[SerializeField] private Vector3 targetRotation;
	[SerializeField] private float rotationDuration;
	[SerializeField] private RotateMode rotateMode;



	public void OnClickUI()
	{

		transform.DOPunchScale(targetscale, scaleDuration, vebrato, esr).SetEase(scaleEaseType);


	}



}
