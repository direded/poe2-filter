using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeFilter.Utilities;
public static class StringUtility {

	// from https://stackoverflow.com/a/10709874
	public static string? GetBetween(string strSource, string strStart, string strEnd) {
		if (strSource.Contains(strStart) && strSource.Contains(strEnd)) {
			int Start, End;
			Start = strSource.IndexOf(strStart, 0) + strStart.Length;
			End = strSource.IndexOf(strEnd, Start);
			return strSource.Substring(Start, End - Start);

		}
		return null;
	}
}
