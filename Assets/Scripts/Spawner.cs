using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
	[SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();
	[SerializeField] private GameObject _cubePrefab;

	[SerializeField] private int _poolCapacity = 10;
	[SerializeField] private int _poolMaxSize = 10;
	[SerializeField] private float[] _startTorqueRange = new float[2] { -10f, 10f };

	private ObjectPool<GameObject> _pool;
	private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.05f);

	private void Awake()
	{
		_pool = new ObjectPool<GameObject>(
			createFunc: () => Instantiate(_cubePrefab, GenerateStartPosition(), Quaternion.identity, gameObject.transform),
			actionOnGet: (cube) => SetStartState(cube),
			actionOnRelease: (cube) => cube.SetActive(false),
			actionOnDestroy: (cube) => Destroy(cube),
			collectionCheck: true,
			defaultCapacity: _poolCapacity,
			maxSize: _poolMaxSize);
	}

	private void Start()
	{
		StartCoroutine(SpawningCubes());
	}

	public void PoolRelease(GameObject cube)
	{
		_pool.Release(cube);
	}

	private void SetStartState(GameObject cube)
	{
		cube.TryGetComponent(out Rigidbody rigidbodyOfCube);

		SetStartPosition(rigidbodyOfCube);
		cube.SetActive(true);
		SetStartTorque(rigidbodyOfCube);
	}

	private void SetStartPosition(Rigidbody cube)
	{
		Vector3 spawnpoint = GenerateStartPosition();
		cube.transform.position = spawnpoint;
		cube.linearVelocity = Vector3.zero;
	}

	private void SetStartTorque(Rigidbody cube)
	{
		var startTorque = new Vector3(Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
							Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
							Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()));

		cube.transform.rotation = Quaternion.Euler(startTorque);
		cube.AddTorque(startTorque, ForceMode.Impulse);
	}

	private Vector3 GenerateStartPosition()
	{
		Vector3 randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].transform.position;
		return randomPoint;
	}

	private IEnumerator SpawningCubes()
	{
		while (true)
		{
			_pool.Get();
			yield return _waitForSeconds;
		}
	}
}
