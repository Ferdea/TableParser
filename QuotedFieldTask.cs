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
        [TestCase("teststring\"hello'wo\\\\'rld\"wfmtla", 10, "hello'wo\\'rld", 16)]
        [TestCase("aboba'hello'wo\\\\'rld\"wfmtla", 5, "hello", 7)]
        [TestCase("abcd\"rhmm' rhmlr \"\"ehel ''", 4, "rhmm' rhmlr ", 14)]
        [TestCase("thjn' \" rhm aw ht \\\\\\' ' ht", 4, " \" rhm aw ht \\' ", 20)]
        [TestCase("'\\\\'", 0, "\\", 4)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static (string, int) GetField(string line, int startIndex)
        {
            var field = new StringBuilder();
            var startChar = line[startIndex];
            var length = 1;
            for (var i = startIndex + 1; i < line.Length; i++)
            {
                length++;
                if (line[i] == startChar)
                    break;
                if (line[i] == '\\')
                {
                    length++;
                    i += 1;
                }
                field.Append(line[i]);
            }

            return (field.ToString(), length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            string field;
            int length;
            (field, length) = GetField(line, startIndex);

            return new Token(field, startIndex, length);
        }
    }
}
