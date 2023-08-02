namespace HoneyPotter
{
    public class ConfigurationManager
    {
        public string GetInput(string message, bool optional)
        {
            string? input = string.Empty;
            while (!optional && string.IsNullOrEmpty(input))
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
            }

            return input;
        }
    }
}
