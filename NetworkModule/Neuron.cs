using System;
using System.Collections.Generic;

namespace NetworkModule
{
    public class Neuron
    {
        public Guid ID;
        public Guid NetworkID;
        public List<Guid> Connections = new List<Guid>();
        public float Weight;
        public DataPacket Data;

        public Neuron(Guid networkID, DataPacket data)
        {
            ID = Guid.NewGuid();
            Weight = 1;
            NetworkID = networkID;
            Data = data;
        }

        public List<Connection> GetConnections(List<Connection> connections)
        {
            List<Connection> results = new List<Connection>();

            int count = connections.Count;
            for (int i = 0; i < count; i++)
            {
                Connection connection = connections[i];
                if (connection.Neurons.Contains(ID))
                {
                    results.Add(connection);
                }
            }

            return results;
        }
    }
}