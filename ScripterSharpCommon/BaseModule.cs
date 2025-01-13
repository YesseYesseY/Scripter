namespace ScripterSharpCommon
{
    public abstract class BaseModule
    {
        /// <summary>
        /// Name of the module
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Called as soon as the module gets initialized, Before Setup().
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// Get called after Setup(). Aka it gets called after Objects, ProcessEvent etc. is initialized
        /// </summary>
        public virtual void OnLoad() { }

        /// <summary>
        /// Called every imgui frame
        /// </summary>
        public virtual void OnGui() { }
    }
}
