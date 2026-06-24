using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

namespace ClientTelegram.Security
{
    public class NonceCounterStore
    {
        private readonly string _connectionString;
        private const int KeyId = 1; //active primary key

        public NonceCounterStore(string connectionString)
        {
            _connectionString = connectionString;
        }


        /// <summary>
        /// Reserved atomic value of BlockSize
        /// Return the first value from the reserved block.
        /// Transaction: first action write on db and after
        /// retrun the value. If the process dead after, the value in
        /// reserved block ll gone and never ll be using in next
        /// process
        /// </summary>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public long ReserveBlock(int blockSize)
        {
            const string sql = @"
                UPDATE NonceCounter
                SET NextValue = NextValue + @BlockSize
                OUTPUT deleted.NextValue
                WHERE KeyId = @KeyId;";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@BlockSize", blockSize);
            cmd.Parameters.AddWithValue("@KeyId", KeyId);

            conn.Open();
            object? result = cmd.ExecuteScalar();

            if (result is null)
                throw new InvalidOperationException(
                    $"Nessuna riga NonceCounter per KeyId={KeyId}. Inizializza la tabella.");

            return Convert.ToInt64(result);
        }
    }
}
