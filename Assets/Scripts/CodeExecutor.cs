using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeExecutor : MonoBehaviour
{
    private CodeSave code;
    private int flowBlockIndex;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public static void Run(CodeSave code)
    {
        Instance.code = code;
        Instance.flowBlockIndex = code.UpdateBlock;
        Instance.enabled = true;
    }
    public static void Stop()
    {
        Instance.enabled = false;
    }


    private void Update()
    {
        ExecuteFlowBlock(code.GetBlock(flowBlockIndex), code);
    }

    private void ExecuteFlowBlock(BlockSave block, CodeSave code)
    {
        ExecuteBlock(block, code);

        BlockSave[] flow = code.GetFlowBlocks(block);
        for (int i = 0; i < flow.Length; i++)
        {
            ExecuteFlowBlock(flow[i], code);
        }
    }
    private Parameter ExecuteBlock(BlockSave block, CodeSave code)
    {
        KeyValuePair<int, BlockSave>[] dataBlocks = code.GetBlocksConnectedToInputs(block);
        Parameter[] data = new Parameter[block.InputValues.Length];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = block.InputValues[i];
        }
        for (int i = 0; i < dataBlocks.Length; i++)
        {
            data[dataBlocks[i].Key] = ExecuteBlock(dataBlocks[i].Value, code);
        }

        return ExecuteSingleBlock(block.ID, data);
    }
    private Parameter ExecuteSingleBlock(BlockID block, Parameter[] inputData)
    {
        switch (block)
        {
            case BlockID.Rotate:
                player.Rotate(((ParamFloat)inputData[1]).GetValue());
                break;
            case BlockID.Move:
                float[] xy = ((ParamVector2)inputData[1]).GetValue();
                player.Move(new Vector3(xy[0], 0, xy[1]));
                break;
            case BlockID.If:
                break;
            case BlockID.LineOfSight:
                return new ParamBool(true);
            case BlockID.NearestPickup:
                return new ParamPickup(Objective.PickupNearestTo(player.transform.position));
            case BlockID.DirectionTo:
                Vector3 direction = ((ParamPickup)inputData[0]).GetValue().transform.position - player.transform.position;
                return new ParamVector2(direction.x, direction.z);
            default:
                break;
        }
        return null;
    }

    public static CodeExecutor Instance { get { return Database.Instance.CodeExecutor; } }
}
