using ScripterSharpCommon;

namespace ExampleModule
{
    public class ExampleModule : BaseModule
    {
        public override string Name => "Example Module";

        public override void OnLoad()
        {
            Console.WriteLine($"Hello from the module!");
        }
    }
}
