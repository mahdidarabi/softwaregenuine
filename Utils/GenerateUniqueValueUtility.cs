namespace Licensing.Utils;

    public class GenerateUniqueValueUtility
    {
        private Random random;
        public GenerateUniqueValueUtility()
        {
            random = new Random();
        }

        public string Token(int section, int length = 8)
        {
            string token = "";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < section; i++)
            {
                token += new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                if (i != section -1 )
                {
                    token += "-";
                }
            }
            return token; 
        }
    }
