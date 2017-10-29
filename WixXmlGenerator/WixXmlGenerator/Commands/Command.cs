namespace WixXmlGenerator.Commands
{
    public abstract class Command
    {
        protected int NumberOfArgs;

        protected Command(int numberOfArgs)
        {
            NumberOfArgs = numberOfArgs;
        }

        public int GetNumberOfArgs()
        {
            return NumberOfArgs;
        }
    }
}
