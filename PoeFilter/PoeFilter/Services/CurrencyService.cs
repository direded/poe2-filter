using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTutorials;
using System.Xml;
using PoeFilter.Models;

namespace PoeFilter.Services;
public class CurrencyService {

	public required StyleService styleService;

	public List<Currency> currencies = new List<Currency>();

	public void ParseCurrencyXml(XmlNode root) {
		foreach (XmlNode currencyNode in root.ChildNodes) {
			var currency = Currency.Parse((XmlElement) currencyNode);
			currencies.Add(currency);
		}
	}

	public string CreateFilterRules() {
		currencies.Sort((a, b) => -a.price.CompareTo(b.price));
		var groups = new List<(List<Currency>, Style)>();
		foreach (var c in currencies) {
			bool itemAssigned = false;
			var style = GetStyle(c);
			foreach (var g in groups) {
				if (style == g.Item2) {
					g.Item1.Add(c);
					itemAssigned = true;
				}
			}

			if (!itemAssigned) {
				var group = (new List<Currency>(), style);
				group.Item1.Add(c);
				groups.Add(group);
			}
		}

		string result = string.Empty;
		foreach (var g in groups) {
			var baseTypeText = "";
			g.Item1.ForEach(item => {
				baseTypeText += $" \"{item.baseType}\"";
			});
			result += (g.Item2.show ? "Show" : "Hide") + "\n";
			result += "Class \"Currency\"\n";
			result += $"BaseType{baseTypeText}\n";
			result += g.Item2.ConvertToFilterRuleStyleString() + "\n";
		}

		result += GetHideRule();
		return result;
	}

	public Style GetStyle(Currency c) {
		var price = c.price;
		var styles = styleService.styles;
		if (c.styleOverrideId != null) {
			return styles[c.styleOverrideId];
		} else if (price > 50) {
			return styles["S"];
		} else if (price > 7) {
			return styles["A"];
		} else if (price >= 3f) {
			return styles["B"];
		} else if (price >= 1f) {
			return styles["C"];
		} else if (price >= 0.8f) {
			return styles["D"];
		} else if (price >= 0.4f) {
			return styles["E"];
		} else if (price >= 0.13f) {
			return styles["F"];
		} else {
			return styles["hide"];
		}
	}

	public string GetHideRule() {
		return
@"Hide
BaseType == ""Gold""

# Unknown currency
Show
Class ""Currency""
SetTextColor 255 0 0
SetBorderColor 0 255 0
SetBackgroundColor 50 50 180
PlayAlertSound 7 300
MinimapIcon 1 Yellow Pentagon
";
	}
}
