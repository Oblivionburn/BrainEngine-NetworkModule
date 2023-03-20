﻿using System;
using System.Collections.Generic;

namespace NetworkModule
{
    public class Network
    {
        public Guid ID;
        private List<Neuron> Neurons = new List<Neuron>();

        public Network()
        {
            ID = Guid.NewGuid();
        }

        public virtual void AddNeuron(Neuron newNeuron)
        {
            bool found = false;

            int count = Neurons.Count;
            for (int i = 0; i < count; i++)
            {
                Neuron oldNeuron = Neurons[i];
                if (oldNeuron.ID == newNeuron.ID ||
                    Equals(oldNeuron.Data.Data, newNeuron.Data.Data))
                {
                    found = true;

                    //If neuron already exists in network, increase weight
                    oldNeuron.Weight++;
                }
                else
                {
                    //Decay all the other neurons
                    Decay(oldNeuron);
                }
            }

            if (!found)
            {
                Neurons.Add(newNeuron);
            }
        }

        public virtual Neuron GetNeuron(DataPacket dataPacket)
        {
            int count = Neurons.Count;
            for (int i = 0; i < count; i++)
            {
                Neuron existing = Neurons[i];
                if (Equals(existing.Data.Data, dataPacket.Data) &&
                    existing.Data.Type == dataPacket.Type)
                {
                    return existing;
                }
            }

            Neuron newNeuron = new Neuron(ID, dataPacket);
            Neurons.Add(newNeuron);
            return newNeuron;
        }

        public virtual bool DataMatch(Type type)
        {
            int count = Neurons.Count;
            for (int i = 0; i < count; i++)
            {
                Neuron neuron = Neurons[i];
                if (neuron.Data.Type == type)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void Decay(Neuron neuron)
        {
            neuron.Weight /= 2;

            //If decayed to a millionth or less of original weight, remove it
            if (neuron.Weight <= 0.000001f)
            {
                Neurons.Remove(neuron);
            }
        }
    }
}