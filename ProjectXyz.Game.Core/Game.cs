﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProjectXyz.Api.Interface;
using ProjectXyz.Application.Interface.Maps;
using ProjectXyz.Game.Core.Binding;
using ProjectXyz.Game.Core.Binding.Explore;
using ProjectXyz.Game.Interface;
using ProjectXyz.Utilities;

namespace ProjectXyz.Game.Core
{
    public sealed class Game : IGame
    {
        #region Fields
        private readonly IApiManager _apiManager;
        private readonly IGameManager _gameManager;
        #endregion

        #region Constructors
        private Game(
            IGameManager gameManager,
            IApiManager apiManager)
        {
            _gameManager = gameManager;
            _apiManager = apiManager;


        }
        #endregion

        #region Methods
        public static IGame Create(
            IGameManager gameManager,
            IApiManager apiManager)
        {
            var game = new Game(
                gameManager,
                apiManager);
            return game;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (MapApiBinder.Create(
                _apiManager, 
                _gameManager.ApplicationManager.Maps))
            {
                await AsyncGameLoop(cancellationToken);
            }
        }

        private async Task AsyncGameLoop(CancellationToken cancellationToken)
        {
            await CreateGameLoopTask(cancellationToken);
        }

        private Task CreateGameLoopTask(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var mapLastUpdatedCache = new ConcurrentDictionary<IMap, DateTime>();
                    using (var gate = new RateGate(60, TimeSpan.FromSeconds(1)))
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            gate.WaitToProceed();

                            var activeMaps = _gameManager.WorldManager.World.Maps.ToArray();
                            PruneInactiveMapsFromCache(
                                mapLastUpdatedCache,
                                activeMaps);

                            Parallel.ForEach(
                                activeMaps,
                                map =>
                                {
                                    var utcNow = DateTime.UtcNow;
                                    if (!mapLastUpdatedCache.ContainsKey(map))
                                    {
                                        // TODO: think about what this implies for a freshly loaded map
                                        mapLastUpdatedCache[map] = DateTime.UtcNow;
                                        return;
                                    }

                                    var elapsedTime = utcNow - mapLastUpdatedCache[map];
                                    mapLastUpdatedCache[map] = utcNow;

                                    map.UpdateElapsedTime(elapsedTime);
                                });
                        }
                    }
                },
                cancellationToken);
        }

        private void PruneInactiveMapsFromCache(
            IDictionary<IMap, DateTime> cache,
            IEnumerable<IMap> activeMaps)
        {
            foreach (var inactiveMap in cache
                .Keys
                .ToArray()
                .Except(activeMaps))
            {
                cache.Remove(inactiveMap);
            }

        }
        #endregion
    }
}
