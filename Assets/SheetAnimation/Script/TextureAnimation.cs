using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextureAnimation : MonoBehaviour
{
    public MeshFilter meshFilter;
	public int _tilesX = 1;
	public int _tilesY = 1;
	public int _framesCount;
	public float _fps = 10;

	private float _currentFrame = -1;

	private float _playStartTime;

	[Header("For Testing")]
	public bool callValidate = false;

	public Vector2 first,second,third,fourth;


	void Awake()
	{
		if(meshFilter == null) meshFilter = GetComponent<MeshFilter>();
		_playStartTime = Time.time;
		// _framesCount = _tilesX * _tilesY;
		// SetFrame(0);

		// Test();

	}

	#if UNITY_EDITOR
	private void OnValidate() {
		if(callValidate && UnityEditor.EditorApplication.isPlaying)
		{
			Test();
			callValidate = false;
		}
	}
	#endif

	void Test()
	{
		Vector2[] uv = new Vector2[] {
			first,
			second,
			third,
			fourth
		};

		meshFilter.mesh.uv = uv;
	}

	void Update()
	{
		if (_framesCount > 1)
		{
			int frame = (int)((Time.time - _playStartTime) * _fps);
			frame = frame % _framesCount;
			SetFrame(frame);
		}
		else {
			SetFrame (0);
		}
	}

	/// <summary>
	/// Sets the frame.
	/// </summary>
	/// <param name="frame">Frame.</param>
	public void SetFrame(int frame)
	{
		if (frame == _currentFrame)
		{
			//no change
			return;
		}

		//		if (frame > _framesCount - 1 || frame < 0) {
		//			return;
		//		}

		float xUnitSize = 1.0f / _tilesX;
		float yUnitSize = 1.0f / _tilesY;

		int xIndex = frame % _tilesX;//remainder
		int yIndex = frame / _tilesX;//
		yIndex = _tilesY - yIndex - 1;


		Vector2[] uv = new Vector2[] {
			new Vector2(xIndex * xUnitSize, yIndex * yUnitSize),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(xUnitSize, 0),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(0, yUnitSize),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(xUnitSize, yUnitSize),
		};

		meshFilter.mesh.uv = uv;

		_currentFrame = frame;
	}
}
