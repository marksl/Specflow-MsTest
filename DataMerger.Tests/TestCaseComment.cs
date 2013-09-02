using System;

namespace TestLogConnector
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestCaseComment : Attribute
    {
        public TestCaseComment(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; set; }
    }
}