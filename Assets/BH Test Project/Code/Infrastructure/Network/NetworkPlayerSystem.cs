using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.Player.Systems;
using BH_Test_Project.Code.Runtime.Player.UI;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem : NetworkBehaviour
    {
        private readonly List<PlayerScores> _players = new();
        private Dictionary<int, string> _playerNames;
        private PlayerHUD _playerHUD;

        private PlayerStaticData _playerStaticData;

        private float _gameRestartDelay;
        private int _gameEndScore;

        public void RegisterHandlers()
        {
            //NetworkClient.RegisterHandler<PlayerConnectedMessage>(OnPlayerConnected);
         
            NetworkClient.RegisterHandler<PlayerHitSuccessMessage>(OnPlayerHitSucceed);
        }

        public void UnregisterHandlers()
        {
           // NetworkClient.UnregisterHandler<PlayerConnectedMessage>();
            NetworkServer.UnregisterHandler<PlayerAskHitMessage>();
            NetworkClient.UnregisterHandler<PlayerHitSuccessMessage>();
        }

        public void Init(WorldStaticData worldStaticData, PlayerStaticData playerStaticData)
        {
            _gameEndScore = worldStaticData.GameEndScore;
            _gameRestartDelay = worldStaticData.GameRestartDelay;
            _playerStaticData = playerStaticData;
            ResetPlayersScore();
        }

        private void ResetPlayersScore()
        {
           // for (int i = 0; i < _players.Count; i++)
            //    _players[i].ResetScore();
        }

        private void Start()
        {
            //if (isClient)
            //CmdInitPlayers(_playerStaticData);
        }

        [Command(requiresAuthority = false)]
        private void CmdInitPlayers(PlayerStaticData playerStaticData)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity == null)
                    return;
                if (PlayerWasInitialized(conn.identity.netId))
                    continue;
                //if (conn.identity.TryGetComponent(out PlayerBehavior player))
                   // player.RpcInitializePlayer();
            }
        }

        private bool PlayerWasInitialized(uint identityNetId)
        {
            return _players.Any(player => player.NetID == identityNetId);
        }

        private bool PlayerAlreadyAdded(PlayerConnectedMessage MSG)
        {
            return _players.Any(pl => pl.Name == MSG.PlayerName && pl.NetID == MSG.NetId);
        }


        private void SendPlayerHit(uint hitRecipientConnId, int hitSenderConnId)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.netId == hitRecipientConnId)
                {
                    conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                   // playerBehavior.TargetPlayerHit(hitSenderNetId);
                }
            }
        }

        private void OnPlayerHitSucceed(PlayerHitSuccessMessage msg)
        {
            foreach (var player in _players.Where(player => player.NetID == msg.HitSenderNetId))
            {
               // player.IncreasePlayerScore();
                UpdatePlayersScoreUI(msg.HitSenderNetId, player.Score);
                CheckGameEndConditions(player);
            }
        }

        private void UpdatePlayersScoreUI(uint successPlayerNetId, int newScore)
        {
            _playerHUD.UpdatePlayerScore(successPlayerNetId, newScore);
        }

        private void CheckGameEndConditions(PlayerScores player)
        {
            if (player.Score == _gameEndScore)
                NotifyGameEnded(player);
        }

        private void NotifyGameEnded(PlayerScores player)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                playerBehavior.TargetGameEnd();
            }

            _playerHUD.EnableEndGamePanel(player.Name);
            if (isServer)
                StartCoroutine(RestartGameRoutine());
        }

        private IEnumerator RestartGameRoutine()
        {
            yield return new WaitForSeconds(_gameRestartDelay);
            CmdRestartGame();
        }

        [Command(requiresAuthority = false)]
        private void CmdRestartGame() =>
            NetworkServer.SendToAll(new GameRestartMessage());
        
    }
}