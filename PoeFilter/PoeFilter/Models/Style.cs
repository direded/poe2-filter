using System.Xml;
using PoeFilter.Utilities;

namespace PoeFilter.Models;

public class Style {

	public string id;
	public bool show = true;
	public string? parentId;
	public string? font;
	public string? textColor;
	public string? borderColor;
	public string? backgroundColor;
	public string? alert;
	public string? effect;
	public string? icon;
	public string? customAlertOpt;

	public Style? parent;

	public string ConvertToFilterRuleStyleString() {
		var result = string.Empty;
		var c = ComputedStyle;
		if (c.font != null) result += $"SetFontSize {c.font}\n";
		if (c.textColor != null) result += $"SetTextColor {c.textColor}\n";
		if (c.borderColor != null) result += $"SetBorderColor {c.borderColor}\n";
		if (c.backgroundColor != null) result += $"SetBackgroundColor {c.backgroundColor}\n";
		if (c.alert != null) result += $"PlayAlertSound {c.alert}\n";
		if (c.effect != null) result += $"PlayEffect {c.effect}\n";
		if (c.icon != null) result += $"MinimapIcon {c.icon}\n";
		if (c.customAlertOpt != null) result += $"CustomAlertSoundOptional {c.customAlertOpt}\n";
		return result;
	}

	public static Style Parse(XmlElement root) {

		XmlUtility.AssertAttributeExists(root, "id", "Style");

		var style = new Style();
		style.id = root.GetAttribute("id");
		style.show = bool.Parse(XmlUtility.GetOrDefaultAttribute(root, "show", "true")!);
		style.parentId = XmlUtility.GetOrDefaultAttribute(root, "parent", null);
		style.font = XmlUtility.GetOrDefaultAttribute(root, "font", null);
		style.customAlertOpt = XmlUtility.GetOrDefaultAttribute(root, "customAlertOpt", null);
		style.icon = XmlUtility.GetOrDefaultAttribute(root, "icon", null);
		style.effect = XmlUtility.GetOrDefaultAttribute(root, "effect", null);
		style.alert = XmlUtility.GetOrDefaultAttribute(root, "alert", null);
		style.backgroundColor = XmlUtility.GetOrDefaultAttribute(root, "background", null);
		style.borderColor = XmlUtility.GetOrDefaultAttribute(root, "border", null);
		style.textColor = XmlUtility.GetOrDefaultAttribute(root, "text", null);
		if (style.customAlertOpt != null) style.customAlertOpt = style.customAlertOpt.Replace("'", "\"");
		return style;
	}

	private Style? _computedStyle;
	private Style ComputeStyle() {
		var style = new Style();
		var fieldsToCopy = new string[] {
			"font",
			"textColor",
			"borderColor",
			"backgroundColor",
			"alert",
			"effect",
			"icon",
			"customAlertOpt"
		};
		var type = GetType();
		foreach (var fieldName in fieldsToCopy) {
			type.GetField(fieldName)!.SetValue(style, GetComputedParameter(fieldName));
		}

		return style;
	}

	public string? GetComputedParameter(string parameterName) {
		var parameterField = GetType().GetField(parameterName)!;
		var value = (string?) parameterField.GetValue(this);
		if (value == "null") return null;
		if (parent == null) return value;
		var parentStyle = parent.ComputedStyle;
		return value == null ? (string?) parameterField.GetValue(parent) : value;
	}

	public Style ComputedStyle {
		get {
			if (_computedStyle == null) {
				_computedStyle = ComputeStyle();
			}
			return _computedStyle;
		}
	}
}