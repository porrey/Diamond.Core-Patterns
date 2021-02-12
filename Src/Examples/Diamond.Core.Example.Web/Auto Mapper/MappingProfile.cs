using AutoMapper;

namespace Diamond.Core.Example.Web.Mappings {
	/// <summary>
	/// 
	/// </summary>
	public class MappingProfile : Profile {
		/// <summary>
		/// 
		/// </summary>
		public MappingProfile() {
			CreateMap<Invoice, IInvoice>();
			CreateMap<IInvoice, Invoice>();
		}
	}
}
