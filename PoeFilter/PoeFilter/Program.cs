using System.Reflection;
using System.Xml;
using PoeFilter.Services;

namespace CSharpTutorials {

	class Program {

		static void Main(string[] args) {

			var styleService = new StyleService();

			var templateBuilderService = new TemplateBuilderService() {
				styleService = styleService
			};

			var currencyService = new CurrencyService() {
				styleService = styleService
			};

			var filterBuilder = new FilterBuilderService() {
				currencyService = currencyService,
				styleService = styleService,
				templateBuilderService = templateBuilderService
			};

			using (var configReader = new StreamReader("filter_builder.xml"))
			using (var filterWriter = new StreamWriter("result.filter")) {
				filterBuilder.BuildFilter(filterWriter, configReader);
			}
		}
	}
}