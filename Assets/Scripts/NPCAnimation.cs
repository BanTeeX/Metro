using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NPCAI))]
public class NPCAnimation : MonoBehaviour
{
    private Animator _animator;
	private NPCAI _npcAI;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_npcAI = GetComponent<NPCAI>();
	}

	private void Start()
	{
		StartCoroutine(MonitorState());
	}

	private IEnumerator MonitorState()
	{
		while (gameObject.activeSelf)
		{
			_animator.SetBool("isWalking", _npcAI.IsWalking);
			_animator.SetBool("isClicking", _npcAI.InAction);
			yield return null;
		}
	}
}
