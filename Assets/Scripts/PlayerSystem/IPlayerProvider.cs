using System;
using UnityEngine;

namespace PlayerSystem
{
    public interface IPlayerProvider
    {
        public event Action<Player> OnPlayerChanged;
        public bool TryGetPlayer(out GameObject player);
        public bool TryGetPlayerClass(out Player player);
    }
    
    public interface IPlayerRegisterable
    {
        void RegisterPlayer(Player player);
        void UnregisterPlayer();
    }
}