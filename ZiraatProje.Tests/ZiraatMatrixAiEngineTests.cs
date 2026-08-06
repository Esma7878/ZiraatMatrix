using Xunit;
using ZiraatProje.Business;

namespace ZiraatProje.Tests
{
    public class ZiraatMatrixAiEngineTests
    {
        [Fact]
        public void Instance_ShouldReturnNonNullSingleton()
        {
            // Act
            var engine1 = ZiraatMatrixAiEngine.Instance;
            var engine2 = ZiraatMatrixAiEngine.Instance;

            // Assert
            Assert.NotNull(engine1);
            Assert.Same(engine1, engine2);
            Assert.NotNull(engine1.Leaves);
            Assert.NotNull(engine1.Shifts);
            Assert.NotNull(engine1.Projects);
            Assert.NotNull(engine1.Digest);
            Assert.NotNull(engine1.Chat);
        }
    }
}
