using System;

namespace TestLogConnector
{
    public class TestCaseAttribute : Attribute
    {
        public TestCaseAttribute(string title, string author, string jira)
        {
        }
    }
}