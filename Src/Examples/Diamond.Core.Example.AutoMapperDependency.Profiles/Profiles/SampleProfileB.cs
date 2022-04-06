using AutoMapper;

namespace Diamond.Core.Example.AutoMapperDependency.Profiles
{
	public class SampleProfileB : Profile
	{
		public SampleProfileB()
		{
			this.CreateMap<SampleModelB, SampleModelA>()
				.ForMember(d => d.Name, o => o.MapFrom(s => s.Emplyee))
				.ForMember(d => d.Description, o => o.MapFrom(s => s.Position));
		}
	}
}
