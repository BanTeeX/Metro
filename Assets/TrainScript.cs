using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
	[SerializeField] private List<TravelPoint> _travelPoints = new List<TravelPoint>();
	[SerializeField] private bool _teleportAtEnd;
	[SerializeField] private float _speed;
	[SerializeField] private float _pointsTolerance;
	[SerializeField] private bool _infiniteLoop;
	private Transform _transform;

	private void Awake()
	{
		_transform = GetComponent<Transform>();
	}

	private void Start()
	{
		StartCoroutine(Travel());
	}

	private IEnumerator Travel()
	{
		while (_infiniteLoop)
		{
			for (int i = 0; i < _travelPoints.Count;)
			{
				Vector3 translation = (_travelPoints[i].point.position - _transform.position).normalized * _speed * Time.deltaTime;
				_transform.Translate(translation);
				_travelPoints[i].isReached = Vector3.Distance(_travelPoints[i].point.position, _transform.position) < _pointsTolerance;
				if (_travelPoints[i].isReached)
				{
					i++;
					yield return new WaitForSeconds(_travelPoints[i - 1].timeToStayInPoint);
				}
				else
				{
					yield return new WaitForEndOfFrame();
				}
			}
			if (_teleportAtEnd)
			{
				_transform.position = _travelPoints[0].point.position;
				foreach (TravelPoint point in _travelPoints)
				{
					point.isReached = false;
				}

			}
		}
	}

	[Serializable]
	private class TravelPoint
	{
		public Transform point;
		public bool isReached;
		public float timeToStayInPoint;
	}
}
