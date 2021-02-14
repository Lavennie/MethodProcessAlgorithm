using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Database : MonoBehaviour, IEnumerable<KeyValuePair<BlockID, BlockData>>
{
    public const float CIRCUIT_WIDTH = 3.0f;
    public const float CIRCUIT_DOT_RADIUS = 8.0f;

    private static Database instance;

    [SerializeField] private ColorPalette colorPalette;

    [Header("Connector sprites")]
    [SerializeField] private Sprite transparentSprite;
    [SerializeField] private Sprite connectorFlowSprite;
    [SerializeField] private Sprite connectorFlowTrueSprite;
    [SerializeField] private Sprite connectorFlowFalseSprite;
    [SerializeField] private Sprite connectorNumberSprite;
    [SerializeField] private Sprite connectorTrueFalseSprite;
    [SerializeField] private Sprite connectorDirection2Sprite;
    [SerializeField] private Sprite connectorDirection3Sprite;
    [SerializeField] private Sprite connectorPickupSprite;
    [SerializeField] private Sprite connectorEnemySprite;

    [Header("Block part prefabs")]
    public Block blockPrefab;
    public Connector inputPrefab;
    public Connector outputPrefab;
    public IntegratedParameter integratedParamPrefab;
    public CodeBlockMenuEntry menuBlockEntryPrefab;
    public Link linkPrefab;

    [Header("In scene references")]
    [SerializeField] private CodeWindow codeWindow;
    [SerializeField] private CodeExecutor codeExecutor;
    [SerializeField] private Objective objective;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private Player player;

    private Dictionary<BlockID, BlockData> data = new Dictionary<BlockID, BlockData>
    {
        { BlockID.Update, new BlockData("Update", "Control", new ConnectorID[0], new ConnectorID[1] { ConnectorID.FlowNormal })},
        { BlockID.If, new BlockData("If", "Control", new ConnectorID[2] { ConnectorID.FlowNormal, ConnectorID.Bool }, new ConnectorID[2] { ConnectorID.FlowIfTrue, ConnectorID.FlowIfFalse })},
        { BlockID.Rotate, new BlockData("Rotate", "Transform", new ConnectorID[2] { ConnectorID.FlowNormal, ConnectorID.Float }, new ConnectorID[1] { ConnectorID.FlowNormal })},
        { BlockID.Move, new BlockData("Move", "Transform", new ConnectorID[2] { ConnectorID.FlowNormal, ConnectorID.Direction2 }, new ConnectorID[1] { ConnectorID.FlowNormal })},
        { BlockID.LineOfSight, new BlockData("Line of Sight", "Vision", new ConnectorID[0] { }, new ConnectorID[1] { ConnectorID.Bool })},
        { BlockID.NearestPickup, new BlockData("Nearest pickup", "Vision", new ConnectorID[0] { }, new ConnectorID[1] { ConnectorID.Pickup })},
        { BlockID.DirectionTo, new BlockData("Direction To", "Vision", new ConnectorID[1] { ConnectorID.Pickup }, new ConnectorID[1] { ConnectorID.Direction2 })},
    };

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator<KeyValuePair<BlockID, BlockData>> GetEnumerator()
    {
        foreach (var block in data)
        {
            yield return block;
        }
    }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    public static Sprite GetSprite(ConnectorID dataType)
    {
        switch (dataType)
        {
            case ConnectorID.FlowNormal:
                return Instance.connectorFlowSprite;
            case ConnectorID.FlowIfTrue:
                return Instance.connectorFlowTrueSprite;
            case ConnectorID.FlowIfFalse:
                return Instance.connectorFlowFalseSprite;
            case ConnectorID.Int:
                return Instance.connectorNumberSprite;
            case ConnectorID.Float:
                return Instance.connectorNumberSprite;
            case ConnectorID.Bool:
                return Instance.connectorTrueFalseSprite;
            case ConnectorID.Direction2:
                return Instance.connectorDirection2Sprite;
            case ConnectorID.Direction3:
                return Instance.connectorDirection3Sprite;
            case ConnectorID.Pickup:
                return Instance.connectorPickupSprite;
            case ConnectorID.Enemy:
                return Instance.connectorEnemySprite;
            default:
                return Instance.transparentSprite;
        }
    }
    public static Color GetColor(ColorPalette.Slot colorSlot) { return Instance.colorPalette.GetColorFromSlot(colorSlot); }

    public static Database Instance { get { return instance; } }
    public BlockData this[BlockID id] { get { return data[id]; } }
    public CodeWindow CodeWindow { get { return codeWindow; } }
    public CodeExecutor CodeExecutor { get { return codeExecutor; } }
    public Objective Objective { get { return objective; } }
    public EndScreen EndScreen { get { return endScreen; } }
    public Player Player { get { return player; } }
}

public enum BlockID : uint
{
    Update,
    Rotate,
    Move,
    If,
    LineOfSight,
    NearestPickup,
    DirectionTo,
}
public enum ConnectorID : uint
{
    FlowNormal,
    FlowIfTrue,
    FlowIfFalse,
    Int,
    Float,
    Bool,
    Direction2,
    Direction3,
    Pickup,
    Enemy,
}
public struct BlockData
{
    private string name;
    private string group;
    private ConnectorID[] inputs;
    private ConnectorID[] outputs;

    public BlockData(string name, string group, ConnectorID[] inputs, ConnectorID[] outputs) 
    {
        this.name = name;
        this.group = group;
        this.inputs = inputs;
        this.outputs = outputs;
    }

    public string Name { get { return name; } }
    public string Group { get { return group; } }
    public int InputCount { get { return inputs.Length; } }
    public int OutputCount { get { return outputs.Length; } }
    public ConnectorID Input(int index) { return inputs[index]; }
    public ConnectorID Output(int index) { return outputs[index]; }
}
