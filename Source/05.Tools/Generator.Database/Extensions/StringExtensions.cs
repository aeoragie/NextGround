using System.Text;

namespace Generator.Database.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            if (input.Length == 1)
            {
                return input.ToUpper();
            }
                
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        public static string ToFirstLower(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
                
            if (input.Length == 1)
            {
                return input.ToLower();
            }

            return char.ToLower(input[0]) + input.Substring(1);
        }

        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            

            var parts = input.Split(new[] { '_', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();

            foreach (var part in parts)
            {
                if (part.Length > 0)
                {
                    result.Append(char.ToUpper(part[0]));
                    if (part.Length > 1)
                    {
                        result.Append(part.Substring(1).ToLower());
                    }
                }
            }

            return result.ToString();
        }

        public static string ToCamelCase(this string input)
        {
            var pascalCase = input.ToPascalCase();
            return pascalCase.ToFirstLower();
        }

        public static StringBuilder Tab(this StringBuilder sb, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                sb.Append("    "); // 4개 공백으로 탭 표현
            }
            return sb;
        }
    }
}
