using System.Collections.Generic;
using System.Linq;

namespace SurveyToCS
{
	public class ModelProperty
	{
		public bool IsComplexObject => Properties.Any();
		public bool IsList { get; set; } = false;
		public Element Element { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public List<ModelProperty> Properties { get; set; } = new List<ModelProperty>();
	}
}
