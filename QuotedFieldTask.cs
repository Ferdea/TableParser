using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("\"a\"", 0, "a", 3)]
        [TestCase("\"\\\\a\"", 0, "\\a", 5)]
        [TestCase("'\"a\"'", 0, "\"a\"", 5)]
        [TestCase("'a b'", 0, "a b", 5)]
        [TestCase("'abc", 0, "abc", 4)]
        [TestCase("\"a \"", 0, "a ", 4)]
        [TestCase("' a'", 0, " a", 4)]
        [TestCase("a a'a'", 3, "a", 3)]
        [TestCase("a a\"a\"", 3, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var field = new StringBuilder();
            var startChar = line[startIndex];
            var length = 1;
            for (var i = startIndex + 1; i < line.Length; i++)
            {
                length++;
                if (line[i] == startChar && line[i - 1] != '\\')
                    break;
                field.Append(line[i]);
            }

            var result = new StringBuilder();
            for (var i = 0; i < field.Length; i++)
            {
                if (field[i] == '\\')
                {
                    result.Append(field[i + 1]);
                    i += 1;
                    continue;
                }
                result.Append(field[i]);
            }
            
            return new Token(result.ToString(), startIndex, length);
        }
    }
}
