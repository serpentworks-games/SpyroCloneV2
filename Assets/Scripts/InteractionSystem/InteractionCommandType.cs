namespace ScalePact.InteractionSystem
{
    /// <summary>
    /// The command type acts as a way to differentiate interactions
    /// as well as link up senders and receivers
    /// </summary>
    [System.Serializable]
    public enum InteractionCommandType
    {
        None,
        Activate,
        Deactivate,
        Open,
        Close,
        Spawn,
        Destroy,
        Start,
        Stop
    }

    /// <summary>
    /// The activation type further filters out interactions that need specific
    /// requirements when trying to fire
    /// </summary>
    [System.Serializable]
    public enum InteractionActivationType
    {
        None, 
        ChestKey,
        BasicKey,
        BossKey
    }
}