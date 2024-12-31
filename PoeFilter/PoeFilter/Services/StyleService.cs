using System.Xml;
using PoeFilter.Models;

namespace PoeFilter.Services;
public class StyleService {

	public Dictionary<string, Style> styles = new Dictionary<string, Style>();

	public void ParseStyleXml(XmlNode root) {
		foreach (XmlNode styleNode in root.ChildNodes) {
			var style = Style.Parse((XmlElement) styleNode);
			styles.Add(style.id, style);
		}

		if (!styles.ContainsKey("base")) {
			throw new Exception("No style with name \"base\" in config file");
		}

		var baseStyle = styles["base"];
		foreach (var style in styles.Values) {
			if (style.id == "base") continue;
			style.parent = styles.GetValueOrDefault(style.parentId ?? "base", baseStyle);
		}
	}
}
