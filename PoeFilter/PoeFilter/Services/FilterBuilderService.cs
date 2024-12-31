using System.Xml;

namespace PoeFilter.Services;


class FilterBuilderService {

	public required StyleService styleService;
	public required CurrencyService currencyService;
	public required TemplateBuilderService templateBuilderService;

	public void BuildFilter(StreamWriter filterWriter, StreamReader cr) {

		XmlDocument configReader = new XmlDocument();
		configReader.Load(cr);

		if (configReader.FirstChild == null) {
			throw new Exception("no root found");
		}

		templateBuilderService.ParseTemplatesXml(configReader.FirstChild.SelectSingleNode("includeFiles")!);
		styleService.ParseStyleXml(configReader.FirstChild.SelectSingleNode("styles")!);
		currencyService.ParseCurrencyXml(configReader.FirstChild.SelectSingleNode("currencies")!);

		filterWriter.WriteLine(currencyService.CreateFilterRules());
		filterWriter.WriteLine(templateBuilderService.CreateFilterRules());
	}

	public void BuildFilter(string targetFilterPath, string configPath) {
		using (var streamReader = new StreamReader(configPath))
		using (var filterWriter = new StreamWriter(targetFilterPath)) {
			BuildFilter(filterWriter, streamReader);
		}
	}

}
