using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] {"text"})]
        [TestCase("hello world", new[] {"hello", "world"})]
        [TestCase("\"hello world\"", new[] {"hello world"})]
        [TestCase("text    space", new[] {"text", "space"})]
        [TestCase("\"hello \\\"world\"", new[] {"hello \"world"})]
        [TestCase("\"hello \\\\world\"", new[] {"hello \\world"})]
        [TestCase("\"hello 'world\"", new[] {"hello 'world"})]
        [TestCase("\"hello \\'world\"", new[] {"hello 'world"})]
        [TestCase("hello world", new[] {"hello", "world"})]
        [TestCase("'hello world'", new[] {"hello world"})]
        [TestCase("'hello \\\"world'", new[] {"hello \"world"})]
        [TestCase("'hello \\\\world'", new[] {"hello \\world"})]
        [TestCase("'hello \\'world'", new[] {"hello 'world"})]
        [TestCase("'hello \\'world'", new[] {"hello 'world"})]
        [TestCase("'hello \\'world'", new[] {"hello 'world"})]
        [TestCase("text \"rlh\"", new[] {"text", "rlh"})]
        [TestCase("\"rlh\" text", new[] {"rlh", "text"})]
        [TestCase("\"hello world", new[] {"hello world"})]
        [TestCase("text\"rlh\"", new[] {"text", "rlh"})]
        [TestCase("\"hello world ", new[] {"hello world "})]
        [TestCase(" text", new[] {"text"})]
        [TestCase("", new string[] {})]
        [TestCase("''", new[] {""})]
        [TestCase("'\\\\'", new[] {"\\"})]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var result = new List<Token>();
            var i = 0;
            while (i < line.Length)
            {
                if (line[i] == ' ')
                {
                    i++;
                    continue;
                }
                
                if (line[i] == '\'' || line[i] == '\"')
                {
                    result.Add(ReadQuotedField(line, i));
                }
                else
                {
                    result.Add(ReadField(line, i));
                }

                i += result[result.Count - 1].Length;
            }

            return result;
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            var field = new StringBuilder();
            var length = 0;
            for (var i = startIndex; i < line.Length; i++)
            {
                if (line[i] == '\"' || line[i] == '\'' || line[i] == ' ')
                    break;
                length++;
                field.Append(line[i]);
            }
            return new Token(field.ToString(), startIndex, length);
        }

        private static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}