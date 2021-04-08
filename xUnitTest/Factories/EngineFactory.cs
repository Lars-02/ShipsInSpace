using Data.Model;
using Moq;

namespace xUnitTest.Factories
{
    public static class EngineFactory
    {
        public static Mock<Engine> CreateEngine()
        {
            var engine = new Mock<Engine>();

            engine.Setup(e => e.Id).Returns(0);
            engine.Setup(e => e.Name).Returns("TestEngine");
            engine.Setup(e => e.Energy).Returns(5);
            engine.Setup(e => e.Weight).Returns(5);

            return engine;
        }
    }
}