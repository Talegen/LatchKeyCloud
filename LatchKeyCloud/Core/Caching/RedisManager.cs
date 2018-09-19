namespace LatchKeyCloud.Core.Caching
{
    using StackExchange.Redis;

    /// <summary>
    /// This class is used to interact with the redis server.
    /// </summary>
    public static class RedisManager
    {
        /// <summary>
        /// Gets the connection multiplexer
        /// </summary>
        public static IConnectionMultiplexer Connection { get; private set; }

        /// <summary>
        /// Initializes the multiplexer with the specified connection string.
        /// </summary>
        /// <param name="connectionString">Contains the connection string to connect to.</param>
        /// <returns>Returns the new Redis multiplexer object.</returns>
        public static IConnectionMultiplexer Initialize(string connectionString)
        {
            // if no multiplexer has been defined...
            if (Connection == null || !Connection.IsConnected)
            {
                Connection = ConnectionMultiplexer.Connect(connectionString);
            }

            return Connection;
        }

        /// <summary>
        /// This method is used to publish a message to a specified channel.
        /// </summary>
        /// <param name="channelName">Contains the channel name to send to.</param>
        /// <param name="message">Contains the message to send.</param>
        /// <param name="commandFlags">Contains optional command flags.</param>
        /// <returns>Returns the count of subscribers the message was sent to.</returns>
        public static long Publish(string channelName, string message, CommandFlags commandFlags = CommandFlags.None)
        {
            long result = 0;

            if (Connection != null)
            {
                var pub = Connection.GetSubscriber();
                var channel = new RedisChannel(channelName, RedisChannel.PatternMode.Literal);
                result = pub.Publish(channel, message, commandFlags);
            }

            return result;
        }
    }
}
