using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
    public class Volumes
    {
        private static readonly Volumes instance = new Volumes();
        private List<Volumetric> volumes;

        private Volumes()
        {
            volumes = new List<Volumetric>();
        }


        public static void Add(Volumetric volume)
        {
            instance.volumes.Add(volume);
        }

        public static List<Volumetric> Get()
        {
            return instance.volumes;
        }
    }
}