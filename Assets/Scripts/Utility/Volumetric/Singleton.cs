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

    public class DarkZones
    {
        private static readonly DarkZones instance = new DarkZones();
        private List<DarkZone> zones;

        private DarkZones()
        {
            zones = new List<DarkZone>();
        }


        public static void Add(DarkZone zone)
        {
            instance.zones.Add(zone);
        }

        public static List<DarkZone> Get()
        {
            return instance.zones;
        }

        public static List<Vector4> Positions()
        {
            List<Vector4> positions = new List<Vector4>();

            foreach (var zone in Get())
                positions.Add(zone.Position);

            return positions;
        }

        public static List<float> CloseRadius()
        {
            List<float> radius = new List<float>();

            foreach (var zone in Get())
                radius.Add(zone.CloseRadius);

            return radius;
        }

        public static List<float> FarRadius()
        {
            List<float> radius = new List<float>();

            foreach (var zone in Get())
                radius.Add(zone.FarRadius);

            return radius;
        }

        public static List<Vector4> Colors()
        {
            List<Vector4> colors = new List<Vector4>();

            foreach (var zone in Get())
                colors.Add(zone.Color);

            return colors;
        }
    }
}