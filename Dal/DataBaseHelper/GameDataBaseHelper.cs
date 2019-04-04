using System;
using System.Data.SqlTypes;
using System.Linq;
using Dal.Encryption;
using Dal.Model;

namespace Dal.DataBaseHelper
{
    public class GameDataBaseHelper : IDisposable
    {
        public GameContext DataBase;

        public GameDataBaseHelper(GameContext dataBase)
        {
            DataBase = dataBase;
        }

        public bool TryGetPlayerFromDB(int playerId)
        {
            var player1 = DataBase.Players.Find(playerId);
            return player1 != null;
        }

        public Player GetPlayerFromDb(int playerId)
        {
            return DataBase.Players.Find(playerId);
        }

        public Player GetPlayerFromDb(string username)
        {
            return DataBase.Players.SingleOrDefault(player => player.Username == username);
        }

        /// <summary>
        /// Add player to DataBase and return id of player.
        /// </summary>
        /// <param name="player">Player need to add</param>
        /// <returns>Return Id of new player</returns>
        public long AddPlayerToDb(Player player)
        {
            DataBase.Players.Add(player);
            DataBase.SaveChanges();
            return DataBase.Players.First(p => p.Username == player.Username).Id;
        }

        public Player LoginToDB(string username, string password)
        {
            var player = GetPlayerFromDb(username);

            if (player == null || Cryptography.Decrypt(player.Password) != password)
                return null;

            var stat = DataBase.Stats.SingleOrDefault(idPlayer => idPlayer.PlayerId == player.Id);

            player.Statistic = stat ?? throw new SqlNullValueException("Player doesn't have statistics");

            return player;

        }

        public Player Registrate(string username, string password)
        {
            
            var player = new Player
            {
                Username = username,
                Password = Cryptography.Encrypt(password),
                Statistic = new Statistic()
                
            };
            DataBase.Players.Add(player);
            DataBase.SaveChanges();
            return player;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }

        public void SaveChanges(params Player[] players)
        {
            foreach (var player in players)
            {
                var result = DataBase.Players.SingleOrDefault(x => x.Username == player.Username);
                if (result!=null)
                {
                    result = player;
                    DataBase.SaveChanges();
                }
            }
        }
    }
}
