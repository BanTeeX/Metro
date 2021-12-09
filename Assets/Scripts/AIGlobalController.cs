using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class AIGlobalController : MonoBehaviour
{
	[SerializeField] private Transform _spawnPoint;
	[SerializeField] private int _numberOfNPCs;
	[SerializeField] private float _maxSpawnInterval;
	[SerializeField] private float _minSpawnInterval;
	[SerializeField] private List<TravelState> _travelOrder;
	[SerializeField] private List<GameObject> _npcPrefabs = new List<GameObject>();
	[SerializeField] private List<TravelPoint> _leftPlatformUpPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _leftPlatformDownPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _rightPlatformUpPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _rightPlatformDownPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _leftExitUpPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _leftExitDownPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _rightExitUpPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _rightExitDownPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _phonePoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _ATMPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _ticetmachinePoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _turnstileEntrancePoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _turnstileExitPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _trainLeftPlatformPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _trainRightPlatformPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _startLeftPoints = new List<TravelPoint>();
	[SerializeField] private List<TravelPoint> _startRightPoints = new List<TravelPoint>();

	public void Spawn()
	{
		Instantiate(_npcPrefabs[UnityEngine.Random.Range(0, _npcPrefabs.Count)], _spawnPoint.position, Quaternion.identity, transform);
	}

	public TravelState GetNextTravelState(TravelState travelState)
	{
		int index = _travelOrder.FindIndex(state => state == travelState) + 1;
		index = index == _travelOrder.Count ? 0 : index;
		return _travelOrder[index];
	}

	public List<TravelPoint> GetNextTravelPoints(TravelState travelState)
	{
		switch (travelState)
		{
			case TravelState.FromStart:
				return GetPathFromStart();
			case TravelState.Ticetmachine:
				return GetPathToTicetmachine();
			case TravelState.TurnstileEntrance:
				return GetPathToTurnstilesEntrance();
			case TravelState.TurnstileExit:
				return GetPathToTurnstilesExit();
			case TravelState.Platform:
				return GetPathPlatform();
			case TravelState.Phone:
				return GetPathToPhone();
			case TravelState.ATM:
				return GetPathToATM();
			case TravelState.ToStart:
				return GetPathToStart();
			default:
				return new List<TravelPoint>();
		}
	}

	private List<TravelPoint> GetPathToStart()
	{
		List<TravelPoint> output = new List<TravelPoint>();
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			output.AddRange(_leftExitUpPoints);
			output.Add(_startLeftPoints[UnityEngine.Random.Range(0, _startLeftPoints.Count)]);
			return output;
		}
		output.AddRange(_rightExitUpPoints);
		output.Add(_startRightPoints[UnityEngine.Random.Range(0, _startRightPoints.Count)]);
		return output;
	}

	private List<TravelPoint> GetPathToATM()
	{
		TravelPoint point = _ATMPoints.FirstOrDefault(point => !point.isOccupied);
		if (point != default)
		{
			point.isOccupied = true;
		}
		return new List<TravelPoint>() { point };
	}

	private List<TravelPoint> GetPathToPhone()
	{
		TravelPoint point = _phonePoints.FirstOrDefault(point => !point.isOccupied);
		if (point != default)
		{
			point.isOccupied = true;
		}
		return new List<TravelPoint>() { point };
	}

	private List<TravelPoint> GetPathPlatform()
	{
		List<TravelPoint> output = new List<TravelPoint>();
		TravelPoint point;
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			output.AddRange(_leftPlatformDownPoints);
			point = _trainLeftPlatformPoints.FirstOrDefault(point => !point.isOccupied);
			if (point != default)
			{
				point.isOccupied = true;
			}
			output.Add(point);
			output.AddRange(_leftPlatformUpPoints);
			return output;
		}
		output.AddRange(_rightPlatformDownPoints);
		point = _trainRightPlatformPoints.FirstOrDefault(point => !point.isOccupied);
		if (point != default)
		{
			point.isOccupied = true;
		}
		output.Add(point);
		output.AddRange(_rightPlatformUpPoints);
		return output;
	}

	private List<TravelPoint> GetPathToTurnstilesExit()
	{
		TravelPoint point = _turnstileExitPoints.FirstOrDefault(point => !point.isOccupied);
		if (point != default)
		{
			point.isOccupied = true;
		}
		return new List<TravelPoint>() { point };
	}

	private List<TravelPoint> GetPathToTurnstilesEntrance()
	{
		TravelPoint point = _turnstileEntrancePoints.FirstOrDefault(point => !point.isOccupied);
		if (point != default)
		{
			point.isOccupied = true;
		}
		return new List<TravelPoint>() { point };
	}

	private List<TravelPoint> GetPathToTicetmachine()
	{
		TravelPoint point = _ticetmachinePoints.FirstOrDefault(point => !point.isOccupied);
		if (point != default)
		{
			point.isOccupied = true;
		}
		return new List<TravelPoint>() { point };
	}

	private List<TravelPoint> GetPathFromStart()
	{
		List<TravelPoint> output = new List<TravelPoint>();
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			output.Add(_startLeftPoints[UnityEngine.Random.Range(0, _startLeftPoints.Count)]);
			output.AddRange(_leftExitDownPoints);
			return output;
		}
		output.Add(_startRightPoints[UnityEngine.Random.Range(0, _startRightPoints.Count)]);
		output.AddRange(_rightExitDownPoints);
		return output;
	}

	private void Start()
	{
		StartCoroutine(SpawningNPCs());
	}

	private IEnumerator SpawningNPCs()
	{
		for (int i = 0; i < _numberOfNPCs; i++)
		{
			Spawn();
			yield return new WaitForSeconds(UnityEngine.Random.Range(_minSpawnInterval, _maxSpawnInterval));
		}
	}

	[Serializable]
	public enum TravelState
	{
		FromStart,
		Ticetmachine,
		TurnstileEntrance,
		TurnstileExit,
		Platform,
		Phone,
		ATM,
		ToStart,
		End
	}

	[Serializable]
	public class TravelPoint
	{
		public Transform point;
		public bool isActionable;
		public float timeToStayInPoint;
		[HideInInspector] public bool isOccupied;
	}
}
