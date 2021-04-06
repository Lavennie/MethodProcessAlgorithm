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

    public void ResetToIdle()
    {
        player.speedInThisFrame = 0;
    }


    private void Update()
    {
        ExecuteFlowBlock(code.GetBlock(flowBlockIndex), code);
    }

    private void ExecuteFlowBlock(BlockSave block, CodeSave code)
    {
        ExecuteBlock(block, code, out bool continueFlow);

        // condintionaly continiue the flow
        BlockSave[] flow = code.GetFlowBlocks(block, continueFlow);
        for (int i = 0; i < flow.Length; i++)
        {
            ExecuteFlowBlock(flow[i], code);
        }
    }
    private Parameter ExecuteBlock(BlockSave block, CodeSave code, out bool continueFlow)
    {
        KeyValuePair<int, BlockSave>[] dataBlocks = code.GetBlocksConnectedToInputs(block);
        Parameter[] data = new Parameter[block.InputValues.Length];
        // get data from integrated parameters
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = block.InputValues[i];
        }
        // get data from blocks connected to inputs
        for (int i = 0; i < dataBlocks.Length; i++)
        {
            data[dataBlocks[i].Key] = ExecuteBlock(dataBlocks[i].Value, code, out bool unused);
        }

        return ExecuteSingleBlock(block.ID, data, out continueFlow);
    }
    private Parameter ExecuteSingleBlock(BlockID block, Parameter[] inputData, out bool continueFlow)
    {
        Parameter result = null;
        continueFlow = true;

        switch (block)
        {
            case BlockID.Move:
                float[] xy = ((ParamVector2)inputData[1]).GetValue();
                player.Move(new Vector3(xy[0], 0, xy[1]));
                break;
            case BlockID.Rotate:
                player.Rotate(((ParamInteger)inputData[1]).GetValue());
                break;
            case BlockID.RotateToward:
                float[] dir = ((ParamVector2)inputData[1]).GetValue();
                player.Rotate(new Vector3(dir[0], 0, dir[1]));
                break;
            case BlockID.DirectionTo:
                GameObject nearest = Objective.PickupNearestTo(player.transform.position);
                if (nearest != null)
                {
                    Vector3 direction = nearest.transform.position - player.transform.position;
                    result = new ParamVector2(direction.x, direction.z);
                }
                else
                {
                    result = new ParamVector2(0, 0);
                }
                break;
            case BlockID.OnTrigger:
                if (TriggerPlate.Triggered != null)
                {
                    TriggerPlate.TriggerColor triggerColor = TriggerPlate.Triggered.Color;
                    TriggerPlate.Triggered = null;
                    result = new ParamColor(triggerColor);
                    break;
                }
                else
                {
                    continueFlow = false;
                    result = new ParamColor(TriggerPlate.LastColor);
                    break;
                }
            case BlockID.LastTrigger:
                result = new ParamColor(TriggerPlate.LastColor);
                break;
            case BlockID.CompareColor:
                if (((ParamColor)inputData[1]).GetValue() != ((ParamColor)inputData[2]).GetValue())
                {
                    continueFlow = false;
                }
                break;
            case BlockID.Debug:
                Debug.Log(((ParamColor)inputData[1]).GetValue());
                break;
            default:
                break;
        }
        return result;
    }

    public static CodeExecutor Instance { get { return Database.Instance?.CodeExecutor; } }
}
