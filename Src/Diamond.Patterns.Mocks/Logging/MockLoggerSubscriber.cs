using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockLoggerSubscriber : ILoggerSubscriber
	{
		public LoggerDelegate Logger { get; set; }
	}
}
