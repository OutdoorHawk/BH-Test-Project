using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.Player.UI;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem : NetworkBehaviour
    {
        private readonly List<PlayerOnServer> _players = new();
        private PlayerHUD _playerHUD;

        private int _gameEndScore;
        private float _gameRestartDelay;
        private PlayerStaticData _playerStaticData;

        public void RegisterHandlers()
        {
            NetworkClient.RegisterHandler<PlayerConnectedMessage>(OnPlayerConnected);
            NetworkServer.RegisterHandler<PlayerAskHitMessage>(OnPlayerHit);
            NetworkClient.RegisterHandler<PlayerHitSuccessMessage>(OnPlayerHitSucceed);
        }

        public void UnregisterHandlers()
        {
            NetworkClient.UnregisterHandler<PlayerConnectedMessage>();
            NetworkServer.UnregisterHandler<PlayerAskHitMessage>();
            NetworkClient.UnregisterHandler<PlayerHitSuccessMessage>();
        }

        public void Init(PlayerHUD playerHUD, WorldStaticData worldStaticData,
            PlayerStaticData playerStaticData)
        {
            _gameEndScore = worldStaticData.GameEndScore;
            _gameRestartDelay = worldStaticData.GameRestartDelay;
            _playerStaticData = playerStaticData;
            _playerHUD = playerHUD;
            _playerHUD.Init(_gameRestartDelay);
            ResetPlayersScore();
            Debug.Log("InitSystem");
           // Debug.Log(isClient); //Null?
            
           // if (isClient) 
            //    CmdInitPlayers(_playerStaticData); for instantiate
        }

        private void ResetPlayersScore()
        {
            for (int i = 0; i < _players.Count; i++)
                _players[i].ResetScore();
        }

        private void Start()
        {
            if (isClient)
                CmdInitPlayers(_playerStaticData);
        }

        [Command(requiresAuthority = false)]
        private void CmdInitPlayers(PlayerStaticData playerStaticData)
        {
            Debug.Log("InitializePlayers");
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity == null)
                    return;
                if (!PlayerWasInitialized(conn.identity.netId))
                {
                    if (conn.identity.TryGetComponent(out PlayerBehavior player))
                    {
                        Debug.Log("InitPlayer");
                        player.TargetInitPlayer(playerStaticData);
                    }
                }
            }
        }

        private bool PlayerWasInitialized(uint identityNetId)
        {
            return _players.Any(player => player.NetID == identityNetId);
        }

        private void OnPlayerConnected(PlayerConnectedMessage MSG)
        {
            _playerHUD.AddPlayerToScoreTable(MSG);
            _players.Add(new PlayerOnServer(MSG.NetId, MSG.PlayerName));
        }

        private void OnPlayerHit(NetworkConnection connection, PlayerAskHitMessage message)
        {
            SendPlayerHit(message.HitRecipientNetId, message.HitSenderNetId);
        }

        private void SendPlayerHit(uint hitRecipientNetId, uint hitSenderNetId)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.netId == hitRecipientNetId)
                {
                    conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                    playerBehavior.TargetPlayerHit(hitSenderNetId);
                }
            }
        }

        private void OnPlayerHitSucceed(PlayerHitSuccessMessage msg)
        {
            foreach (var player in _players.Where(player => player.NetID == msg.HitSenderNetId))
            {
                player.IncreasePlayerScore();
                UpdatePlayersScoreUI(msg.HitSenderNetId, player.Score);
                CheckGameEndConditions(player);
            }
        }

        private void UpdatePlayersScoreUI(uint successPlayerNetId, int newScore)
        {
            _playerHUD.UpdatePlayerScore(successPlayerNetId, newScore);
        }

        private void CheckGameEndConditions(PlayerOnServer player)
        {
            if (player.Score == _gameEndScore)
                NotifyGameEnded(player);
        }

        private void NotifyGameEnded(PlayerOnServer player)
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