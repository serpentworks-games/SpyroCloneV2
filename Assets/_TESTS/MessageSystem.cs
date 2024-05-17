namespace ScalePact.Utils
{
    public enum MessageType
    {
        DAMAGED, DEAD, RESPAWN
    }

    public interface IMessageReceiver
    {
        void OnReceiveMessage(MessageType type, object sender, object msg);
    }
}