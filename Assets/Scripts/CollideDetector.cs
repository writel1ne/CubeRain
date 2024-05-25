using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(ColorSetter))]

public class CollideDetector : MonoBehaviour
{
	private bool _coroutineIsRunning = false;
	private Spawner _spawner;
	private ColorSetter _colorSetter;

	private void Start()
	{
		_colorSetter = GetComponent<ColorSetter>();
		_spawner = GetComponentInParent<Spawner>();
	}

	private void OnEnable()
	{
		_colorSetter?.SetDefaultColor();
		_coroutineIsRunning = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_coroutineIsRunning == false && collision.gameObject.TryGetComponent(out Platform platform))
		{
			_colorSetter.SetRandomColor();
			StartCoroutine(SetRandomLifetime());
		}
	}

	private IEnumerator SetRandomLifetime()
	{
		_coroutineIsRunning = true;
		float lifetime = Random.Range(2f, 5f);

		yield return new WaitForSeconds(lifetime);

		_spawner.PoolRelease(gameObject);
	}
}
