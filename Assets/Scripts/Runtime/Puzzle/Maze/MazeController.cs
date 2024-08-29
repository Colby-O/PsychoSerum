using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PlazmaGames.Core;
using PlazmaGames.MonoSystems.Animation;
using PsychoSerum.MonoSystem;

namespace PsychoSerum.Puzzle
{
	internal class MazeController : PuzzleController
	{
		[Header("Maze Settings")]
		[SerializeField][Min(7)] private int _mazeWidth;
		[SerializeField][Min(2)] private int _mazeHeight;
        [SerializeField] private float _timeLimit = 60;

        [Header("Maze Prefabs")]
		[SerializeField] private GameObject _wallPrefab;
		[SerializeField] private GameObject _floorPrefab;
		[SerializeField] private GameObject _keyPrefab;

		[Header("Gate Settings")]
		[SerializeField] private Transform _gate;
		[SerializeField] private Vector2 _gateLimits;
		[SerializeField] private AudioSource _gateAudioSrc;
		[SerializeField] private AudioClip _gateAudioClip;

		private List<GameObject> _objects;

		private float _scaleX, _scaleZ;
		private Vector3 _position;

		private Maze _maze;
		private GameObject _mazeHolder;

		private bool _isStarted = false;
		private bool _isTimerRunning = false;

		[SerializeField] private float _timer = 0;

		public Maze GetMaze() { return _maze; }
		public Vector3 GetMazePosition() { return _position; }
		public float GetMazeScale() { return _scaleX; }

		private void ClearMaze()
		{
			if (_objects.Count == 0) return;
			float baseHeight = _objects[0].transform.localPosition.y;
			const float displacement = 5f;
			GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
				this,
				_gateAudioClip.length,
				(float t) => {
					foreach (GameObject obj in _objects)
					{
						Vector3 p = obj.transform.localPosition;
						obj.transform.localPosition = new Vector3(p.x, baseHeight - (t*t) * displacement, p.z);
					}
				},
				() => {
					foreach (GameObject obj in _objects) Destroy(obj);
					_objects = new List<GameObject>();
				}
			);
		}

		private Vector2 GetEndIndex(Maze maze)
		{
			Vector2 maxIndex = Vector2.zero;

			int rows = maze.width;
			int cols = maze.height;

			for (int i = 1; i < rows - 1; i++)
			{
				for (int j = 1; j < cols - 1; j++)
				{
					if (maze[i, j] == 1)
					{
						if (i > maxIndex.x || (i == maxIndex.x && j > maxIndex.y))
						{
							maxIndex = new Vector2(i, j);
						}
					}
				}
			}

			return maxIndex;
		}

		private void GenerateMaze()
		{
			if (_maze == null)
			{
				Debug.LogWarning("Trying to instantiate a maze without a Maze object being generated. Try Calling StartPuzzle instead!");
				return;
			}

			ClearMaze();

			Vector2 endIndex = GetEndIndex(_maze);

			bool placedCenter = false;
			bool placedTR = false;
			bool placedBR = false;

			for (int y = 0; y < _maze.height; y++)
			{
				for (int x = 0; x < _maze.width; x++)
				{
					GameObject floor = Instantiate(_floorPrefab);
					floor.transform.position = transform.position + new Vector3(y * _scaleZ, 0, x * _scaleX);
					floor.transform.parent = _mazeHolder.transform;

					if (Random.value > 0.25f)
					{
                        floor.transform.GetChild(2).gameObject.SetActive(Random.value < 0.1f);
                    }
					else
					{
                        floor.transform.GetChild(6).gameObject.SetActive(Random.value < 0.1f);

                    }


					if (_maze[x, y] == 1)
					{
						floor.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.white;

						if (x == endIndex.x && y == endIndex.y)
						{
							GameObject key = Instantiate(_keyPrefab);
							key.transform.position = transform.position + new Vector3(y * _scaleZ, 1f, x * _scaleX);
							key.transform.parent = _mazeHolder.transform;
							_objects.Add(key);
                            floor.transform.GetChild(2).gameObject.SetActive(true);
                            floor.transform.GetChild(4).gameObject.SetActive(true);
						} else if (x == 1 && y == 0)
						{
							floor.transform.GetChild(3).gameObject.SetActive(true);
						}

						if (
							!placedCenter &&
							x >= _maze.width / 4 && x <= _maze.width / 4 + 1 &&
							y >= _maze.height / 4 && y <= _maze.height / 4 + 1
						)
						{
							placedCenter = true;
							floor.transform.GetChild(5).gameObject.SetActive(true);
						}

						if (
							!placedTR &&
							x >= _maze.width / 4.0f * 3.0f && x <= _maze.width / 4.0f * 3.0f + 1 &&
							y >= _maze.height / 4.0f * 3.0f && y <= _maze.height / 4.0f * 3.0f + 1
						)
						{
							placedTR = true;
							floor.transform.GetChild(5).gameObject.SetActive(true);
						}

						if (
							!placedBR &&
							x >= _maze.width / 4.0f * 3.0f && x <= _maze.width / 4.0f * 3.0f + 1 &&
							y >= _maze.height / 4.0f * 1.0f && y <= _maze.height / 4.0f * 1.0f + 1
						)
						{
							placedBR = true;
							floor.transform.GetChild(5).gameObject.SetActive(true);
						}

						continue;
					}

					floor.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
					floor.transform.GetChild(2).gameObject.SetActive(false); // Turns off light for this cell.
                    floor.transform.GetChild(6).gameObject.SetActive(false);

                    GameObject wall = Instantiate(_wallPrefab);
					wall.transform.position = transform.position + new Vector3(y * _scaleZ, 0, x * _scaleX);
					wall.transform.parent = _mazeHolder.transform;
					_objects.Add(wall);
				}
			}
		}

		private void OnUpdate()
		{
			_timer += Time.deltaTime;
		}

		private void OpenGateStep(float t, Transform gate, float start, float end)
		{
			gate.localPosition = new Vector3(gate.localPosition.x, gate.localPosition.y, Mathf.Lerp(start, end, t));
		}

		private void OpenGate()
		{
			_gateLimits.x = _gate.localPosition.z;
            float start = _gate.localPosition.z;
			float end = _gateLimits.y;
			_gateAudioSrc.PlayOneShot(_gateAudioClip);
			GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
				this,
				_gateAudioClip.length,
				(float t) => OpenGateStep(
					t,
					_gate,
					start,
					end
				)
			);
		}

        public void CloseGate()
        {
            float start = _gate.localPosition.z;
            float end = _gateLimits.x;
            _gateAudioSrc.PlayOneShot(_gateAudioClip);
            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                this,
                _gateAudioClip.length,
                (float t) => OpenGateStep(
                    t,
                    _gate,
                    start,
                    end
                )
            );
        }

        public void End()
		{
			_isStarted = false;
			_gate.gameObject.SetActive(false);
			ClearMaze();
			for (int y = 0; y < _maze.height; y++)
			{
				for (int x = 0; x < _maze.width; x++)
				{
					if ((x != 0 && y != 0) && (x == 0 || y == 0 || x == _maze.width - 1 || y == _maze.height - 1))
					{
						GameObject wall= Instantiate(_wallPrefab);
						wall.transform.position = transform.position + new Vector3(y * _scaleZ, 0, x * _scaleX);
						wall.transform.parent = _mazeHolder.transform;
						_objects.Add(wall);
					}
				}
			}

            GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
            GameManager.GetMonoSystem<IEventMonoSystem>().RunEvent((_timeLimit > GameManager.GetMonoSystem<IUIMonoSystem>().GetView<TimerView>().GetTime()) ? 14 : 15);
        }

		public void Begin()
		{
			if (!_isStarted) return;
            OpenGate();
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<TimerView>(this);
            GameManager.GetMonoSystem<IUIMonoSystem>().GetView<TimerView>().StartTimer();
        }

		public override void StartPuzzle()
		{
            if (_isStarted) return;
            _isStarted = true;
        }

		private void Awake()
		{
			_objects = new List<GameObject>();

			_mazeHolder = new GameObject("Maze Holder");
			_mazeHolder.transform.parent = transform;

			_scaleX = _wallPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;
			_scaleZ = _wallPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.z;

			_position = transform.position;

            _maze = MazeGenerator.Generate(_mazeWidth, _mazeHeight);
            GenerateMaze(); ;
		}

		private void Update()
		{
			if (_isStarted && _isTimerRunning) OnUpdate();
		}
	}
}
