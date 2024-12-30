using System.Xml;
using PoeFilter.Utilities;

namespace PoeFilter.Models;

public class Style {

	public string id;
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
		style.parentId = XmlUtility.GetOrDefaultAttribute(root, "parent", null);
		style.font = XmlUtility.GetOrDefaultAttribute(root, "font", null);
		style.customAlertOpt = XmlUtility.GetOrDefaultAttribute(root, "customAlertOpt", null);
		style.icon = XmlUtility.GetOrDefaultAttribute(root, "icon", null);
		style.effect = XmlUtility.GetOrDefaultAttribute(root, "effect", null);
		style.alert = XmlUtility.GetOrDefaultAttribute(root, "alert", null);
		style.backgroundColor = XmlUtility.GetOrDefaultAttribute(root, "bg", null);
		style.borderColor = XmlUtility.GetOrDefaultAttribute(root, "border", null);
		style.textColor = XmlUtility.GetOrDefaultAttribute(root, "text", null);
		if (style.customAlertOpt != null) style.customAlertOpt = style.customAlertOpt.Replace("'", "\"");
		return style;
	}

	private Style? _computedStyle;
	private Style ComputeStyle() {
		var style = new Style();
		style.font = font ?? style.parent?.ComputedStyle.font;
		style.textColor = textColor ?? style.parent?.ComputedStyle.textColor;
		style.borderColor = borderColor ?? style.parent?.ComputedStyle.borderColor;
		style.backgroundColor = backgroundColor ?? style.parent?.ComputedStyle.backgroundColor;
		style.alert = alert ?? style.parent?.ComputedStyle.alert;
		style.effect = effect ?? style.parent?.ComputedStyle.effect;
		style.icon = icon ?? style.parent?.ComputedStyle.icon;
		style.customAlertOpt = customAlertOpt ?? style.parent?.ComputedStyle.customAlertOpt;
		return style;
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