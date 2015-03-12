using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nortal.Utilities.AssemblyVersioning.Generators
{
	internal static class DateToVersionNumberCalculation
	{
		internal static int BuildDatePart(DateTime forTime)
		{
			//last digit mod 6 (max value in version component is 65536). Ex: 2016 -> 6; 2017 -> 0
			int yearpart = forTime.Year % 10 % 7;
			return yearpart * 10000 + 100 * forTime.Month + forTime.Day;
		}

		internal static int BuildTimePart(DateTime forTime)
		{
			return forTime.Hour * 100 + forTime.Minute;
		}
	}
}
