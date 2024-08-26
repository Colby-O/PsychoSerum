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

        private Maze _maze;
        private GameObject _mazeHolder;

        private bool _isStarted = false;
        private bool _isTimerRunning = false;

        [SerializeField] private float _timer = 0;

        private void ClearMaze()
        {
            foreach (GameObject obj in _objects) Destroy(obj);
            _objects = new List<GameObject>();
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

            for (int y = 0; y < _maze.height; y++)
            {
                for (int x = 0; x < _maze.width; x++)
                {
                    GameObject floor = Instantiate(_floorPrefab);
                    floor.transform.position = transform.position + new Vector3(y * _scaleZ, 0, x * _scaleX);
                    floor.transform.parent = _mazeHolder.transform;
                    _objects.Add(floor);

                    floor.transform.GetChild(2).gameObject.SetActive(Random.Range(0, 100) % 2 == 1);

                    if (_maze[x, y] == 1)
                    {
                        floor.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.white;

                        if (x == endIndex.x && y == endIndex.y)
                        {
                            GameObject key = Instantiate(_keyPrefab);
                            key.transform.position = transform.position + new Vector3(y * _scaleZ, 1f, x * _scaleX);
                            key.transform.parent = _mazeHolder.transform;
                            _objects.Add(key);

                            floor.transform.GetChild(4).gameObject.SetActive(true);
                        } else if (x == 1 && y == 0)
                        {
                            floor.transform.GetChild(3).gameObject.SetActive(true);
                        }



                        continue;
                    }

                    floor.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
                    floor.transform.GetChild(2).gameObject.SetActive(false); // Turns off light for this cell.

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
                ),
                () => StartTimer()
            );
        }

        private void StartTimer()
        {
            _timer = 0;
            _isTimerRunning = true;
        }

        private void StopTimer()
        {
            _isTimerRunning = false;
        }

        public void Begin()
        {
            if (_isStarted) return;
           _isStarted = true;
            OpenGate();
        }

        public void End()
        {
            _isStarted = false;
            StopTimer();
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
                    else
                    {
                        GameObject floor = Instantiate(_floorPrefab);
                        floor.transform.position = transform.position + new Vector3(y * _scaleZ, 0, x * _scaleX);
                        floor.transform.parent = _mazeHolder.transform;
                        floor.transform.GetChild(2).gameObject.SetActive(Random.Range(0, 100) % 2 == 1);
                        _objects.Add(floor);
                    }
                }
            }

            GameManager.GetMonoSystem<IPuzzleMonoSystem>().FinishPuzzle(this, PuzzleType.Maze);
        }

        public override void StartPuzzle()
        {
            _maze = MazeGenerator.Generate(_mazeWidth, _mazeHeight);
            GenerateMaze();
        }

        private void Awake()
        {
            _objects = new List<GameObject>();

            _mazeHolder = new GameObject("Maze Holder");
            _mazeHolder.transform.parent = transform;

            _scaleX = _wallPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x;
            _scaleZ = _wallPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.z;

            StartPuzzle();
        }

        private void Update()
        {
            if (_isStarted && _isTimerRunning) OnUpdate();
        }
    }
}
