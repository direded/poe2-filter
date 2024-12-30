using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTutorials;
using System.Xml;
using PoeFilter.Utilities;
using System.Globalization;

namespace PoeFilter.Models;

public class Currency {

	public string baseType;
	public double price;
	public string? styleOverrideId;

	public static Currency Parse(XmlElement root) {

		XmlUtility.AssertAttributeExists(root, "name", "Currency");
		XmlUtility.AssertAttributeExists(root, "price", "Currency");

		var c = new Currency();
		c.baseType = root.GetAttribute("name");
		c.price = ParseTextPrice(root.GetAttribute("price"));
		c.styleOverrideId = XmlUtility.GetOrDefaultAttribute(root, "style", null);
		return c;
	}

	/// <summary>
	/// Parse fraction price value to double
	/// </summary>
	/// <param name="message">Values like: 0, 1, 1/2, 34/54</param>
	/// <returns>price to exalted orb</returns>
	static double ParseTextPrice(string message) {
		if (message.Contains("/")) {
			var parts = message.Split('/');
			return double.Parse(parts[0], CultureInfo.InvariantCulture) / double.Parse(parts[1], CultureInfo.InvariantCulture);
		} else {
			return double.Parse(message, CultureInfo.InvariantCulture);
		}
	}
}
