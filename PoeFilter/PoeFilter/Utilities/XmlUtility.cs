using CSharpTutorials;
using System.Xml;

namespace PoeFilter.Utilities;

public static class XmlUtility {
	public static void AssertAttributeExists(XmlElement x, string n, string objectName = "Object") {
		if (!x.HasAttribute(n)) throw new Exception($"{objectName} doesn't have {n} attribute");
	}

	public static string? GetOrDefaultAttribute(XmlElement element, string id, string? defaultValue) {
		return element.HasAttribute(id) ? element.GetAttribute(id) : defaultValue;
	}
}
