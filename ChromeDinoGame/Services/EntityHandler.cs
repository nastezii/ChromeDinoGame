﻿using ChromeDinoGame.Entities;
using System.Windows.Controls;

namespace ChromeDinoGame.Services
{
    class EntityHandler
    {
        private Canvas _canvas;
        private Dino _dino;
        private Random _random;

        private double _lineOfGround;
        private double _speedOfEntities;
        private double _speedInc;
        private readonly double _initialSpeedOfEntities;

        private ObstacleSpawner _obstaclesSpawner;
        private Action _onCollisionCallback;
        private List<Obstacle> _obstacles = new List<Obstacle>();
        private List<Cloud> _clouds = new List<Cloud>();
        private List<Road> _roads = new List<Road>();

        public EntityHandler(Canvas canvas, Dino dino, Random random, Action onCollisionCallback, double speedOfEntities, double lineOfGround, double sppedInc)
        {
            _canvas = canvas;
            _dino = dino;
            _random = random;
            _onCollisionCallback = onCollisionCallback;
            _lineOfGround = lineOfGround;
            _speedOfEntities = _initialSpeedOfEntities = speedOfEntities;
            _speedInc = sppedInc;

            _obstaclesSpawner = new ObstacleSpawner();
        }

        public void InitializeStartWindow()
        {
            _dino.RenderEntity();
            _roads.Add(new Road(_canvas, _random, _speedOfEntities, 0, _lineOfGround / 1.5));
            _roads[0].RenderEntity();
            _roads.Add(new Road(_canvas, _random, _speedOfEntities, _canvas.Width, _lineOfGround / 1.5));
            _roads[1].RenderEntity();
            _clouds.Add(new Cloud(_canvas, _random.Next(200,400), _random.Next(180, 300), _speedOfEntities / 10));
            _clouds[0].RenderEntity();
        }

        public void SetReplayCharacteristics()
        {
            _speedOfEntities = _initialSpeedOfEntities;
            _roads.Clear();
            _clouds.Clear();
            _obstacles.Clear();
        }

        public void UpdateEntities()
        {
            _speedOfEntities += _speedInc;

            _dino.MoveEntity();  
            UpdateObstacles();
            UpdateClouds();
            UpdateRoads();
        }

        private void CheckCollision(Obstacle obstacle)
        {
            _dino.SetCollisionBox();
            obstacle.SetCollisionBox();
            if (obstacle.CheckCollision(_dino.CollisionBox, obstacle.CollisionBox))
            {
                _onCollisionCallback.Invoke();
            }
        }

        private void UpdateObstacles()
        {
            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                if (_obstacles[i].IsInWindow())
                {
                    _obstacles[i].MoveEntity();
                    CheckCollision(_obstacles[i]);
                }
                else
                {
                    _obstacles[i].RemoveEntity();
                    _obstacles.RemoveAt(i);
                }
            }

            if (_obstacles.Count == 0 || _obstacles[_obstacles.Count - 1].PosX < _random.Next(30, 75))
            {
                _obstacles.Add(_obstaclesSpawner.GenerateObstacle(_canvas, _lineOfGround, _speedOfEntities));
                _obstacles[_obstacles.Count - 1].RenderEntity();
            }
        }

        private void UpdateClouds()
        {
            for (int i = _clouds.Count - 1; i >= 0; i--)
            {
                if (_clouds[i].IsInWindow())
                {
                    _clouds[i].MoveEntity();
                }
                else
                {
                    _clouds[i].RemoveEntity();
                    _clouds.RemoveAt(i);
                }
            }

            if (_clouds[_clouds.Count - 1].PosX < _random.Next(150, 300))
            {
                _clouds.Add(new Cloud(_canvas, _canvas.Width, _random.Next(180, 300), _speedOfEntities / 10));
                _clouds[_clouds.Count - 1].RenderEntity();
            }
        }

        private void UpdateRoads()
        {
            for (int i = _roads.Count - 1; i >= 0; i--)
            {
                if (_roads[i].IsInWindow())
                {
                    _roads[i].MoveEntity();
                }
                else
                {
                    _roads[i].RemoveEntity();
                    _roads.RemoveAt(i);
                    _roads.Add(new Road(_canvas, _random, _speedOfEntities, _canvas.ActualWidth, _lineOfGround / 1.5));
                    _roads[_roads.Count - 1].RenderEntity();
                }
            }
        }
    }
}
