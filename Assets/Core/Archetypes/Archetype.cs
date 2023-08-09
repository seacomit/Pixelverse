using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core.Archetypes
{
    // Notes:
    //
    // - A weapon archetype will need to specify where the weapon is held, the handle anchor.
    // - Weapon swing time and damage will be determined by the literal voxel size, voxel type, weapon archetype
    //   potentially and any magic properties.

    public class Archetype
    {
        public string id;
        public Socket rootSocket; // Forms a socket graph.
    }

    public class Socket
    {
        public Vector3 position;
        public List<Connection> connections;
    }

    public class Connection
    {
        public Component component; // The thing that makes the connection.
        public Vector3 orientation; // Default vector direction of connection.
        public Socket socket; // The socket being connected to if one is present.
    }

    public class Component
    {
        // A component can map to another archetype or primitive.
        public string id;
        public Component baseComponent; // Can be null if is base super class.
        
        // Properties can be overwritten from base component.
        public Dictionary<string, ComponentProperty> properties;

        public Component() {
            properties = new Dictionary<string, ComponentProperty>();
        }
    }

    public class ComponentProperty
    {
        public ComponentPropertyType type;
        public IntRange intRange;
        public RelativeIntRange relativeIntRange;

        public ComponentProperty(ComponentPropertyType type)
        {
            this.type = type;
        }

        public static ComponentProperty newIntRange(int min, int max)
        {
            ComponentProperty property = new ComponentProperty(ComponentPropertyType.Integer);
            property.intRange = new IntRange(min, max);
            return property;
        }

        public static ComponentProperty newRelativeIntRange(string propertyName, int minPercent, int maxPercent)
        {
            ComponentProperty property = new ComponentProperty(ComponentPropertyType.Integer);
            property.relativeIntRange = new RelativeIntRange(propertyName, minPercent, maxPercent);
            return property;
        }
    }

    public enum ComponentPropertyType
    {
        Integer = 0,
        RelativeInt = 1,
    }

    public class IntRange
    {
        public int min;
        public int max;

        public IntRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    public class RelativeIntRange
    {
        public string property;
        public int minPercent;
        public int maxPercent;

        public RelativeIntRange(string property, int minPercent, int maxPercent)
        {
            this.property = property;
            this.minPercent = minPercent;
            this.maxPercent = maxPercent;
        }
    }

    //public class Primitive
    //{
    //    public List<Operation> operations;
    //}
}