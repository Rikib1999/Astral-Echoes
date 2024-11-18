using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace Assets.Scripts.SpaceSystem
{
    [Serializable]
    public class SystemDataBag : NetworkBehaviour
    {
        [SerializeField] public NetworkVariable<SpaceObjectDataBag> CentralObject = new NetworkVariable<SpaceObjectDataBag>();// { get => CentralObject.Value; set => CentralObject.Value = value; }
        //public SpaceObjectDataBag CentralObject { get => centralObject.Value; set => centralObject.Value = value; }
        [field: SerializeField] public NetworkList<SpaceObjectDataBag> SatelliteObjects;// { get => SatelliteObjects.Value; set => SatelliteObjects.Value = value; }
        //public NetworkList<SpaceObjectDataBag> SatelliteObjects { get => satelliteObjects.Value; set => satelliteObjects.Value = value; }
        public void Awake()
        {
            //CentralObject = new NetworkVariable<SpaceObjectDataBag>();
            SatelliteObjects = new NetworkList<SpaceObjectDataBag>();
            //Debug.Log("Awake");
        }
    }

    [Serializable]
    public struct SpaceObjectDataBag : INetworkSerializeByMemcpy, System.IEquatable<SpaceObjectDataBag>
    {
        [SerializeField] public FixedString64Bytes Name ;//{ get; set; }
        [SerializeField] public eSpaceObjectType Type ;//{ get; set; }
        [SerializeField] public int SubType ;//{ get; set; }
        [SerializeField] public float Size ;//{ get; set; }
        [SerializeField] public Vector2 Coordinates ;//{ get; set; }
        [SerializeField] public Vector2 RelativePosition ;//{ get; set; }
        [SerializeField] public float OrbitRadius ;//{ get; set; }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out Name);
                reader.ReadValueSafe(out Type);
                reader.ReadValueSafe(out SubType);
                reader.ReadValueSafe(out Size);
                reader.ReadValueSafe(out Coordinates);
                reader.ReadValueSafe(out RelativePosition);
                reader.ReadValueSafe(out OrbitRadius);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(Name);
                writer.WriteValueSafe(Type);
                writer.WriteValueSafe(SubType);
                writer.WriteValueSafe(Size);
                writer.WriteValueSafe(Coordinates);
                writer.WriteValueSafe(RelativePosition);
                writer.WriteValueSafe(OrbitRadius);
            }
        }

        public bool Equals(SpaceObjectDataBag other)
        {
            return Name == other.Name
            && Type == other.Type
            && SubType == other.SubType
            && Size == other.Size
            && Coordinates == other.Coordinates
            && RelativePosition == other.RelativePosition
            && OrbitRadius == other.OrbitRadius;
        }
    }
}