using System;
using System.CodeDom;
using System.Linq;
using MyGenerator.Generator.SpecflowPlugin;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Parser.SyntaxElements;
using TechTalk.SpecFlow.Utils;

[assembly: GeneratorPlugin(typeof(MyGeneratorPlugin))]

namespace MyGenerator.Generator.SpecflowPlugin
{
    public class MyGeneratorProvider : MsTest2010GeneratorProvider
    {
        public MyGeneratorProvider(CodeDomHelper codeDomHelper)
            : base(codeDomHelper)
        {
        }

        public override void SetTestClassInitializeMethod(TestClassGenerationContext generationContext)
        {
            var field = new CodeMemberField()
                            {
                                Name = "testContext",
                                Type = new CodeTypeReference("Microsoft.VisualStudio.TestTools.UnitTesting.TestContext"),
                                Attributes = MemberAttributes.Private
                            };
            generationContext.TestClass.Members.Add(field);

            field = new CodeMemberField()
                        {
                            Name = "testCase",
                            Type = new CodeTypeReference("TestLogConnector.TestCase"),
                            Attributes = MemberAttributes.Private
                        };
            generationContext.TestClass.Members.Add(field);

            var codeMemberProperty = new CodeMemberProperty();
            codeMemberProperty.Name = "TestContext";
            codeMemberProperty.Type = new CodeTypeReference("Microsoft.VisualStudio.TestTools.UnitTesting.TestContext");
            codeMemberProperty.Attributes = MemberAttributes.Public;
            codeMemberProperty.HasGet = true;
            codeMemberProperty.HasSet = true;
            codeMemberProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "testContext")));
            codeMemberProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "testContext"),
                    new CodePropertySetValueReferenceExpression()));


            generationContext.TestClass.Members.Add(codeMemberProperty);

            codeMemberProperty = new CodeMemberProperty();
            codeMemberProperty.Name = "TestCase";
            codeMemberProperty.Type = new CodeTypeReference("TestLogConnector.TestCase");
            codeMemberProperty.Attributes = MemberAttributes.Public;
            codeMemberProperty.HasGet = true;
            codeMemberProperty.HasSet = true;
            codeMemberProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "testCase")));
            codeMemberProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "testCase"),
                    new CodePropertySetValueReferenceExpression()));


            generationContext.TestClass.Members.Add(codeMemberProperty);

            base.SetTestClassInitializeMethod(generationContext);

            generationContext.TestInitializeMethod.Statements.Add(new CodeSnippetStatement(
                                                                      @"            if (TestContext != null)
            {
                TestCase = new TestLogConnector.TestCase(GetType(), TestContext.TestName);
            }
"));

            generationContext.TestCleanupMethod.Statements.Add(new CodeSnippetStatement(
                                                                       @"           if (TestContext != null)
            {
                TestCase.WriteFinishedTestToTestLog(TestContext.CurrentTestOutcome.ToString());
            }
"));
        }


        public override void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
            foreach (var scenario in generationContext.Feature.Scenarios)
            {
                if (scenario.Title == scenarioTitle)
                {
                    string title = scenarioTitle;

                    if (scenario.Tags != null)
                    {
                        Tag Author = scenario.Tags.FirstOrDefault(x => x.Name.StartsWith("Author"));
                        Tag jira = scenario.Tags.FirstOrDefault(x => x.Name.StartsWith("Jira"));

                        if (Author != null && jira != null)
                        {
                            scenario.Tags.Remove(Author);
                            scenario.Tags.Remove(jira);

                            var authorText = Author.Name.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries)[1];
                            var jiraText = jira.Name.Split(new[] { ':'}, StringSplitOptions.RemoveEmptyEntries) [1];

                            testMethod.CustomAttributes.Add(
                                new CodeAttributeDeclaration(
                                    "TestLogConnector.TestCaseAttribute",
                                    new CodeAttributeArgument(new CodePrimitiveExpression(title)),
                                    new CodeAttributeArgument(new CodePrimitiveExpression(authorText)),
                                    new CodeAttributeArgument(new CodePrimitiveExpression(jiraText))));
                        }
                    }

                    var text = string.Join("\n", scenario.Steps.Select(c => c.StepKeyword.ToString() + ":" + c.Text));
                    testMethod.CustomAttributes.Add(
                            new CodeAttributeDeclaration(
                                "TestLogConnector.TestCaseComment",
                                new CodeAttributeArgument(new CodePrimitiveExpression(text))));
                }
            }

            base.SetTestMethod(generationContext, testMethod, scenarioTitle);
        }
    }
}