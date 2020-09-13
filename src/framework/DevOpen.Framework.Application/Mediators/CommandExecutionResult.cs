namespace DevOpen.Framework.Application.Mediators
{
    public struct CommandExecutionResult
    {
        public int Code { get; }
        public string Description { get; }

        private CommandExecutionResult(int code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly CommandExecutionResult Ok = new CommandExecutionResult(0, "Ok");
        public static readonly CommandExecutionResult NotFound = new CommandExecutionResult(400, "NotFound");
    }
}