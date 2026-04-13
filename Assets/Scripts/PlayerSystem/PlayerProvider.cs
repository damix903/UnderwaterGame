using System;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerProvider : IPlayerProvider, IPlayerRegisterable
    {
        private Player _player;

        public event Action<Player> OnPlayerChanged;

        public bool TryGetPlayer(out GameObject player)
        {
            player = null;
            if (_player == null) return false;

            player = _player.gameObject;
            return true;
        }

        public bool TryGetPlayerClass(out Player player)
        {
            player = null;
            if (_player == null) return false;
            
            player = _player;
            return true;
        }

        public void SetPosition(Vector3 position)
        {
            if (_player == null) return;
            
            _player.transform.position = position;
        }

        public void RegisterPlayer(Player player)
        {
            _player = player;
            OnPlayerChanged?.Invoke(player);
        }

        public void UnregisterPlayer()
        {
            _player = null;
            OnPlayerChanged?.Invoke(null);
        }
    }
}