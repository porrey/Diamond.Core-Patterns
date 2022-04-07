using AutoMapper;

namespace Diamond.Core.Example.AutoMapperDependency.Profiles
{
	public class SampleProfileA : Profile
	{
		public SampleProfileA()
		{
			this.CreateMap<SampleModelA, SampleModelB>()
				.ForMember(d => d.Emplyee, o => o.MapFrom(s => s.Name))
				.ForMember(d => d.Position, o => o.MapFrom(s => s.Description));
		}
	}
}
