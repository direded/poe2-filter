using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeFilter.Models;
using System.Xml;
using PoeFilter.Utilities;

namespace PoeFilter.Services;

public class TemplateBuilderService {

	public required StyleService styleService;
	public List<string> templatePaths = new List<string>();

	public void ParseTemplatesXml(XmlNode root) {
		foreach (XmlNode n in root.ChildNodes) {
			templatePaths.Add(((XmlElement) n).GetAttribute("path"));
		}
	}

	public string CreateFilterRules() {
		var result = string.Empty;
		foreach (string path in templatePaths) {
			using (var reader = new StreamReader(path)) {
				string? line;
				while ((line = reader.ReadLine()) != null) {
					var placeholder = StringUtility.GetBetween(line, "{% ", " %}");
					if (placeholder != null) {
						result += styleService.styles[placeholder.Split(' ')[1]].ConvertToFilterRuleStyleString();
					} else {
						result += line + "\n";
					}
				}
			}
		}
		return result;
	}
}
