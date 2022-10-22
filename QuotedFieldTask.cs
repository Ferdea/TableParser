using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("teststring\"hello'wo\\\\'rld\"wfmtla", 10, "hello'wo\\'rld", 16)]
        [TestCase("aboba'hello'wo\\\\'rld\"wfmtla", 5, "hello", 7)]
        [TestCase("abcd\"rhmm' rhmlr \"\"ehel ''", 4, "rhmm' rhmlr ", 14)]
        [TestCase("thjn' \" rhm aw ht \\\\\\' ' ht", 4, " \" rhm aw ht \\' ", 20)]
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
            for (var i = startIndex + 1; i < line.Length; i++)
            {
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
            
            return new Token(result.ToString(), startIndex, field.Length + 2);
        }
    }
}
