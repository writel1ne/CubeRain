using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class ColorSetter : MonoBehaviour
{
	[SerializeField] private Color _defaultColor = Color.gray;

	private MeshRenderer _meshRenderer;

	private void Start()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
	}

	public void SetRandomColor()
	{
		float r = Random.Range(0f, 1f);
		float g = Random.Range(0f, 1f);
		float b = Random.Range(0f, 1f);

		_meshRenderer.material.color = new Color(r, g, b);
	}

	public void SetDefaultColor()
	{
		_meshRenderer.material.color = _defaultColor;
	}
}
