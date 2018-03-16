using UnityEngine;

/// <summary>
/// This can be used as a way to return to the main unity thread 
/// when using multiple threads with async methods.
/// </summary>
public class WaitForUpdate : CustomYieldInstruction
{
    public override bool keepWaiting => false;
}
