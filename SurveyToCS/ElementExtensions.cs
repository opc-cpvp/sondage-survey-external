using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyToCS
{
	public static class ElementExtensions
	{
		public static string GetNormalizedType(this Element element)
		{
			return element.cellType ?? element.type;
		}

		public static string GetNormalizedName(this Element element)
		{
			return element.valueName ?? element.name;
		}

		public static bool IsOptional(this Element element)
		{
			return !element.isRequired && string.IsNullOrWhiteSpace(element.defaultValue);
		}
	}
}
