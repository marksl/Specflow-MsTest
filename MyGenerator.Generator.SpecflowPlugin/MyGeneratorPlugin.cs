using BoDi;
using MyGenerator.Generator.SpecflowPlugin;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;

[assembly: GeneratorPlugin(typeof(MyGeneratorPlugin))]

namespace MyGenerator.Generator.SpecflowPlugin
{
    public class MyGeneratorPlugin : IGeneratorPlugin
    {
        public void RegisterDependencies(ObjectContainer container)
        {
        }

        public void RegisterCustomizations(ObjectContainer container, SpecFlowProjectConfiguration generatorConfiguration)
        {
            container.RegisterTypeAs<MyGeneratorProvider, IUnitTestGeneratorProvider>();
        }
        
        public void RegisterConfigurationDefaults(SpecFlowProjectConfiguration specFlowConfiguration)
        {
        }
    }
}