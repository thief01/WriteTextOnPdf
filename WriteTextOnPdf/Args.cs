namespace WriteTextOnPdf
{
    public class TextData
    {
        public string TextName { get; set; }
        public string Text { get; set; }
    }
    public class Args
    {
        public string InputPath { get; }
        public string OutputPath { get; }
        public string Text { get; }

        public List<TextData> Texts { get; set; } = new List<TextData>();

        public Args(string[] args)
        {
            InputPath = args[0];
            OutputPath = args[1];
            for (int i = 2; i < args.Length; i++)
            {
                var splited = args[i].Split('=');
                Console.WriteLine(args[i]);
                Console.WriteLine(splited.Length);
                Texts.Add(new TextData(){TextName = splited[0], Text =splited[1]});
            }
            // Text = args[2];
        }
    }
}