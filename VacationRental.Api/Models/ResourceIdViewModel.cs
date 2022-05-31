namespace VacationRental.Api.Models
{
	public class ResourceIdViewModel
	{
		#region Contructors
		public ResourceIdViewModel() { }

		private ResourceIdViewModel(int id)
		{
			Id = id;
		}
		#endregion Contructors

		#region Properties
		public int Id { get; set; }
		#endregion Properties

		#region Static methods
		public static ResourceIdViewModel Create(int id)
		{
			ResourceIdViewModel result = new ResourceIdViewModel(id);
			return result;
		}
		#endregion Static methods
	}
}
