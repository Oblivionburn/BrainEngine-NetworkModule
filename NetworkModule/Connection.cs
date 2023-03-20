using System;
using System.Collections.Generic;

namespace NetworkModule
{
    public class Connection
    {
        public Guid ID;
        public List<Guid> Neurons = new List<Guid>();
        public float Weight;

        public Connection()
        {
            ID = Guid.NewGuid();
            Weight = 1;
        }

        public virtual void Decay()
        {
            Weight /= 2;

            //If decayed to a millionth or less of original weight, drop a random neuron
            if (Weight <= 0.000001f)
            {
                CryptoRandom random = new CryptoRandom();
                int choice = random.Next(0, Neurons.Count);
                Neurons.Remove(Neurons[choice]);

                //Reset weight so we're not dropping neurons too quickly
                Weight = 1;
            }
        }
    }
}