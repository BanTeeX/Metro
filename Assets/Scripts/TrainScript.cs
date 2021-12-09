using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
	[SerializeField] private List<TravelPoint> _travelPoints = new List<TravelPoint>();
	[SerializeField] private bool _resetTravel;
	[SerializeField] private bool _infiniteLoop;
	private float _currentSpeed;
	private float _acceleration;

	private void Start()
	{
		transform.position = _travelPoints[0].point.position;
		_currentSpeed = _travelPoints[0].speed;
		StartCoroutine(TravelCoroutine());
	}

	private IEnumerator TravelCoroutine()
	{
		do
		{
			yield return Travel();
			if (_resetTravel)
			{
				ResetTravel();
			}
		} while (_infiniteLoop);
	}

	private IEnumerator Travel()
	{
		for (int i = 0; i < _travelPoints.Count; i++)
		{
			UpdateAcceleration(_travelPoints[i]);
			while (!CheckIfReached(_travelPoints[i]))
			{
				UpdateCurrentSpeed();
				transform.Translate(GetTranslationToPoint(_travelPoints[i]), Space.World);
				yield return new WaitForEndOfFrame();
			}
			_currentSpeed = _travelPoints[i].speed;
			yield return new WaitForSeconds(_travelPoints[i].timeToStayInPoint);
		}
	}

	private void UpdateAcceleration(TravelPoint nextTravelPoint)
	{
		float deltaSpeed = nextTravelPoint.speed - _currentSpeed;
		float distance = Vector3.Distance(transform.position, nextTravelPoint.point.position);
		if (distance <= 0 || deltaSpeed == 0)
		{
			_acceleration = 0;
			return;
		}
		_acceleration = deltaSpeed * deltaSpeed / 2 / distance;
		_acceleration = deltaSpeed < 0 ? -_acceleration : _acceleration;
	}

	private void UpdateCurrentSpeed()
	{
		_currentSpeed += _acceleration * Time.deltaTime;
	}

	private bool CheckIfReached(TravelPoint travelPoint)
	{
		float distance = Vector3.Distance(travelPoint.point.position, transform.position);
		float nextTranslation = GetTranslationToPoint(travelPoint).magnitude;
		float deltaSpeed = Mathf.Abs(travelPoint.speed - _currentSpeed);
		float nextSpeedStep = Mathf.Abs(_acceleration * Time.deltaTime);
		return distance <= nextTranslation || deltaSpeed < nextSpeedStep;
	}

	private Vector3 GetTranslationToPoint(TravelPoint travelPoint)
	{
		return (travelPoint.point.position - transform.position).normalized * _currentSpeed * Time.deltaTime;
	}

	private void ResetTravel()
	{
		transform.position = _travelPoints[0].point.position;
	}

	[Serializable]
	private struct TravelPoint
	{
		public Transform point;
		public float timeToStayInPoint;
		public float speed;
	}
}
