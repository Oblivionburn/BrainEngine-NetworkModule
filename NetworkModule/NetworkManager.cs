using System;
using System.Collections.Generic;

namespace NetworkModule
{
    public static class NetworkManager
    {
        public static List<Network> Networks = new List<Network>();
        public static List<Connection> NeuronConnections = new List<Connection>();

        public static void ReceiveData(List<DataPacket> dataPackets)
        {
            List<Neuron> neurons = new List<Neuron>();

            int dataCount = dataPackets.Count;
            for (int i = 0; i < dataCount; i++)
            {
                DataPacket packet = dataPackets[i];

                //Get network for this data type
                Network network = NetworkMatch(packet.Type);

                //Get new or existing neuron from network
                Neuron neuron = network.GetNeuron(packet);

                neurons.Add(neuron);
            }

            int count = neurons.Count;
            if (count > 1)
            {
                //Check if these neurons are already all in a connection
                if (!ConnectionExists(neurons))
                {
                    //Associate neurons in this set with each other
                    NewConnection(neurons);
                }
            }
        }

        public static Network NetworkMatch(Type type)
        {
            int count = Networks.Count;
            for (int i = 0; i < count; i++)
            {
                Network existing = Networks[i];
                if (existing.DataMatch(type))
                {
                    return existing;
                }
            }

            //If none found, start a new network for this data type
            Network network = new Network();
            Networks.Add(network);

            return network;
        }

        private static bool ConnectionExists(List<Neuron> neurons)
        {
            List<Connection> connections = neurons[0].GetConnections(NeuronConnections);

            int connectionCount = connections.Count;
            for (int i = 0; i < connectionCount; i++)
            {
                Connection connection = connections[i];

                bool connected = true;

                int neuronCount = neurons.Count;
                for (int j = 0; j < neuronCount; j++)
                {
                    Neuron existing = neurons[j];
                    if (!connection.Neurons.Contains(existing.ID))
                    {
                        connected = false;
                        connection.Decay();
                        break;
                    }
                    else
                    {
                        connection.Weight++;
                    }
                }

                if (connected)
                {
                    return true;
                }
            }

            return false;
        }

        private static void NewConnection(List<Neuron> neurons)
        {
            Connection connection = new Connection();

            int count = neurons.Count;
            for (int i = 0; i < count; i++)
            {
                Neuron existing = neurons[i];
                existing.Connections.Add(connection.ID);

                if (!connection.Neurons.Contains(existing.ID))
                {
                    connection.Neurons.Add(existing.ID);
                }
            }

            NeuronConnections.Add(connection);
        }
    }
}
