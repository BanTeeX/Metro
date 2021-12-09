using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static AIGlobalController;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCAI : MonoBehaviour
{
	[SerializeField] private float _pointTolerance = 1f;
	[SerializeField] private float _speedTolerance = 0.1f;
	private TravelState _travelState = TravelState.FromStart;
	private List<TravelPoint> _travelPoints = new List<TravelPoint>();
	private AIGlobalController _controller;
	private NavMeshAgent _agent;

	public bool IsWalking { get => _agent.velocity.magnitude > _speedTolerance; }
	public bool InAction { get; private set; }

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		_controller = GetComponentInParent<AIGlobalController>();
		_travelPoints = _controller.GetNextTravelPoints(_travelState);
		_agent.Warp(_travelPoints[0].point.position);
		StartCoroutine(Travel());
	}

	private IEnumerator Travel()
	{
		while (_travelState != TravelState.End)
		{
			if (_travelPoints.Any(point => point == default))
			{
				if (_travelState == TravelState.ATM || _travelState == TravelState.Phone)
				{
					_travelState = _controller.GetNextTravelState(_travelState);
					_travelPoints = _controller.GetNextTravelPoints(_travelState);
					continue;
				}
				_travelPoints = _controller.GetNextTravelPoints(_travelState);
				yield return null;
				continue;
			}
			for (int i = 0; i < _travelPoints.Count; i++)
			{
				_agent.SetDestination(_travelPoints[i].point.position);
				_agent.isStopped = false;
				while (!CheckIfReached(_travelPoints[i]))
				{
					yield return null;
				}
				_agent.isStopped = true;
				if (_travelPoints[i].isActionable)
				{
					InAction = true;
				}
				yield return new WaitForSeconds(_travelPoints[i].timeToStayInPoint);
				InAction = false;
				_travelPoints[i].isOccupied = false;
			}
			_travelState = _controller.GetNextTravelState(_travelState);
			_travelPoints = _controller.GetNextTravelPoints(_travelState);
		}
		_controller.Spawn();
		Destroy(gameObject);
	}

	private bool CheckIfReached(TravelPoint travelPoint)
	{
		return Vector3.Distance(transform.position, travelPoint.point.position) <= _pointTolerance;
	}
}
