namespace ScripterSharpCommon
{
    public abstract class BaseModule
    {
        public abstract string Name { get; }

        public virtual void OnLoad() { }
        public virtual void OnGui() { }
    }
}
