namespace WriteTextOnPdf
{
    public class Args
    {
        public string InputPath { get; }
        public string OutputPath { get; }
        public string Text { get; }

        public Args(string[] args)
        {
            InputPath = args[0];
            OutputPath = args[1];
            Text = args[2];
        }
    }
}